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

namespace TasteRestaurant.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty] public ShoppingCart CartObj { get; set; }
    
        public void OnGet(int id)
        {
            var menuItemFromDb = _db.MenuItem.Include(m => m.CategoryType).Include(m => m.FoodType).FirstOrDefault(m=>m.Id==id);

            CartObj = new ShoppingCart()
            {
                MenuItemId = menuItemFromDb.Id,
                MenuItem = menuItemFromDb
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                //Get user id of the currently logged in user
                var claimsIdentity = (ClaimsIdentity) this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                CartObj.ApplicationUserId = claim.Value;

                //ShoppingCart cartFromDb = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObj.ApplicationUserId // This was the course version but ReSharper suggested the following line
                //                                                      && c.MenuItemId == CartObj.MenuItemId).FirstOrDefault();
                ShoppingCart cartFromDb = _db.ShoppingCart.FirstOrDefault(c =>
                    c.ApplicationUserId == CartObj.ApplicationUserId &&
                    c.MenuItemId == CartObj.MenuItemId);

                if (cartFromDb == null)
                {
                    _db.ShoppingCart.Add(CartObj);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObj.Count;
                }

                _db.SaveChanges();

                // Add session and increment count

                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObj.ApplicationUserId).ToList()
                    .Count;
                HttpContext.Session.SetInt32("CartCount", count);

                return RedirectToPage("Index");

            }
            else
            {
                var menuItemFromDb = _db.MenuItem.Include(m => m.CategoryType).Include(m => m.FoodType).FirstOrDefault();

                CartObj = new ShoppingCart()
                {
                    MenuItemId = menuItemFromDb.Id,
                    MenuItem = menuItemFromDb
                };

                return Page();
            }

        }
    }
}