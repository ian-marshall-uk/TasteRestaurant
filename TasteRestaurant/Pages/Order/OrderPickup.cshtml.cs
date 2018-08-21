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
    public class OrderPickupModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public OrderPickupModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new List<OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet()
        {
            List<OrderHeader> orderHeaderList = _db.OrderHeader
                .Where(u => u.Status != StaticData.StatusReadyForPickup).OrderByDescending(u => u.PickupTime).ToList();
            foreach (OrderHeader item in orderHeaderList)
            {
                OrderDetailsViewModel order = new OrderDetailsViewModel // Alternative to the one in ManageOrder.cshtml.cs
                {
                    OrderHeader = item,
                    OrderDetails = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList()
                };

                OrderDetailsViewModel.Add(order);
            }
        }
    }
}