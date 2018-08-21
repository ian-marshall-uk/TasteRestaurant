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
    public class OrderConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public OrderConfirmationModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public void OnGet(int id)
        {
            //Get user id of the currently logged in user
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = _db.OrderHeader.FirstOrDefault(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = _db.OrderDetail.Where(o => o.OrderId == id).ToList()
            };


        }
    }
}