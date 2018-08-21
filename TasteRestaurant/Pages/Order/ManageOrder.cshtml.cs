using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utilities;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class ManageOrderModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ManageOrderModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new List<OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet()
        {
            List<OrderHeader> OrderHeaderList = _db.OrderHeader
                .Where(u => u.Status != StaticData.StatusCompleted && 
                            u.Status != StaticData.StatusReadyForPickup &&
                            u.Status != StaticData.StatusCancelled).OrderByDescending(u => u.PickupTime).ToList();
            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel order = new OrderDetailsViewModel();
                order.OrderHeader = item;
                order.OrderDetails = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();

                OrderDetailsViewModel.Add(order);
            }
        }

        public IActionResult OnPostOrderPrepare(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = StaticData.StatusBeingPrepared;
            _db.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = StaticData.StatusReadyForPickup;
            _db.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = StaticData.StatusCancelled;
            _db.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }
    }
}