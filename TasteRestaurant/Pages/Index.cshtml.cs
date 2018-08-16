﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public IndexViewModel IndexVm { get; set; }

        public async Task OnGet()
        {
            IndexVm = new IndexViewModel()
            {
                MenuItems = await _db.MenuItem.Include(m => m.CategoryType).Include(m => m.FoodType).ToListAsync(),
                CategoryTypes =  _db.CategoryType.OrderBy(c => c.DisplayOrder)
            };
        }
    }
}
