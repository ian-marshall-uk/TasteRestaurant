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

        public void OnGet(string option = null, string search = null)
        {
            if (search != null)
            {
                var user = new ApplicationUser();
                List<OrderHeader> orderHeaders = new List<OrderHeader>();

                if (option == "order")
                {
                    orderHeaders = _db.OrderHeader.Where(o => o.Id == Convert.ToInt32(search)).ToList();
                }
                else
                {
                    if (option == "email")
                    {
                        user = _db.Users.FirstOrDefault(u => u.Email.ToLower().Contains(search.ToLower()));
                    }
                    else if (option == "phone")
                    {
                        user = _db.Users.FirstOrDefault(u => u.PhoneNumber.ToLower().Contains(search.ToLower()));
                    }
                    else if (option == "name")
                    {
                        user = _db.Users.FirstOrDefault(u => u.FirstName.ToLower().Contains(search.ToLower()) ||
                                                             u.LastName.ToLower().Contains(search.ToLower()));
                    }
                }

                if (user != null || orderHeaders.Count > 0)
                {
                    if (orderHeaders.Count == 0)
                    {
                        orderHeaders = _db.OrderHeader.Where(o => o.UserId == user.Id)
                            .OrderByDescending(o => o.PickupTime).ToList();
                    }

                    foreach (OrderHeader item in orderHeaders)
                    {
                        OrderDetailsViewModel order =
                            new OrderDetailsViewModel // Alternative to the one in ManageOrder.cshtml.cs
                            {
                                OrderHeader = item,
                                OrderDetails = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList()
                            };

                        OrderDetailsViewModel.Add(order);
                    }

                }

            }
            else
            {
                List<OrderHeader> orderHeaders = _db.OrderHeader
                    .Where(u => u.Status == StaticData.StatusReadyForPickup).OrderByDescending(u => u.PickupTime)
                    .ToList();
                foreach (OrderHeader item in orderHeaders)
                {
                    OrderDetailsViewModel order =
                        new OrderDetailsViewModel // Alternative to the one in ManageOrder.cshtml.cs
                        {
                            OrderHeader = item,
                            OrderDetails = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList()
                        };

                    OrderDetailsViewModel.Add(order);
                }
            }
        }
    }
}