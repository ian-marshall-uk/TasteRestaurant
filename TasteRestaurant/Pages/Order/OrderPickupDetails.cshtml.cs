using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderPickupDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public OrderPickupDetailsModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new OrderDetailsViewModel();
        }

        public void OnGet(int orderId)
        {
            OrderDetailsViewModel.OrderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);
            OrderDetailsViewModel.OrderHeader.ApplicationUser =
                _db.Users.FirstOrDefault(u => u.Id == OrderDetailsViewModel.OrderHeader.UserId);
            OrderDetailsViewModel.OrderDetails =
                _db.OrderDetail.Where(d => d.OrderId == OrderDetailsViewModel.OrderHeader.Id).ToList();
        }

        public IActionResult OnPost(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = Utilities.StaticData.StatusCompleted;
            _db.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");

        }
    }
}