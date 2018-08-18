using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
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



        }
    }
}