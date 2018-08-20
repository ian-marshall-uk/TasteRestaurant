using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utilities;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OrderDetailsCart DetailCart { get; set; }

        public void OnGet()
        {
            DetailCart = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader()
            };
            DetailCart.OrderHeader.OrderTotal = 0;

            //Get user id of the currently logged in user
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                DetailCart.ListCart = cart.ToList();
            }

            foreach (var list in DetailCart.ListCart)
            {
                list.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == list.MenuItemId);
                DetailCart.OrderHeader.OrderTotal += (list.MenuItem.Price * list.Count);
                if (list.MenuItem.Description.Length > 100)
                {
                    list.MenuItem.Description = list.MenuItem.Description.Substring(0, 99) + "...";
                }
            }

            DetailCart.OrderHeader.PickupTime = DateTime.Now;
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _db.ShoppingCart.FirstOrDefault(c => c.Id == cartId);
            cart.Count++;
            _db.SaveChanges();

            return RedirectToPage("/Cart/Index");
        }

        public async Task<IActionResult> OnPostMinus(int cartId)
        {
            var cart = _db.ShoppingCart.FirstOrDefault(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                _db.SaveChanges();
                var count = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("CartCount", count);
            }
            else
            {
                cart.Count--;
                await _db.SaveChangesAsync();
            }


            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPost()
        {
            //Get user id of the currently logged in user
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            DetailCart.ListCart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToList();

            OrderHeader orderHeader = DetailCart.OrderHeader;
            DetailCart.OrderHeader.OrderDate = DateTime.Now;
            DetailCart.OrderHeader.UserId = claim.Value;
            DetailCart.OrderHeader.Status = StaticData.StatusSubmitted;
            _db.OrderHeader.Add(orderHeader);
            _db.SaveChanges();

            foreach (var item in DetailCart.ListCart)
            {
                item.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetail orderDetail = new OrderDetail()
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = orderHeader.Id,
                    Name = item.MenuItem.Name,
                    Description = item.MenuItem.Description,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                _db.OrderDetail.Add(orderDetail);
            }
            _db.ShoppingCart.RemoveRange(DetailCart.ListCart);
            HttpContext.Session.SetInt32("CartCount", 0);
            ;
            _db.SaveChanges();

            return RedirectToPage("../Index");
        }
    }
}