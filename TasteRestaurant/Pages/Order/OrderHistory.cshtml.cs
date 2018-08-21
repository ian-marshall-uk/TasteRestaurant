using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public OrderHistoryModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        //id = 0 then display only last five orders else display all past orders
        public void OnGet(bool ShowAll = false)
        {
            //Get user id of the currently logged in user
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel = new List<OrderDetailsViewModel>();

            List<OrderHeader> orderHeaderList = _db.OrderHeader.Where(u => u.UserId == claim.Value)
                .OrderByDescending(u => u.OrderDate).ToList();

            if (!ShowAll && orderHeaderList.Count > 4)
            {
                orderHeaderList = orderHeaderList.Take(5).ToList();
            }

            foreach (OrderHeader item in orderHeaderList)
            {
                OrderDetailsViewModel order = new OrderDetailsViewModel();
                order.OrderHeader = item;
                order.OrderDetails = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();

                OrderDetailsViewModel.Add(order);
            }
        }
    }
}