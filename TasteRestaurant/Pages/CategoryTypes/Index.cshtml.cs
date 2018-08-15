using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utilities;

namespace TasteRestaurant.Pages.CategoryTypes
{
    [Authorize(Policy = StaticData.AdminEndUser)]
    public class IndexModel : PageModel
    {
        public readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<CategoryType> CategoryType { get; set; }

        public async Task OnGet()
        {
            CategoryType = await _db.CategoryType.OrderBy(c=>c.DisplayOrder).ToListAsync();
        }
    }
}