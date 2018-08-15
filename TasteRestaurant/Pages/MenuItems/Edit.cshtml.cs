using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utilities;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.MenuItems
{
    [Authorize(Roles = StaticData.AdminEndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EditModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM = new MenuItemViewModel()
            {
                MenuItem = _db.MenuItem.Include(m => m.CategoryType)
                    .Include(m => m.FoodType)
                    .SingleOrDefault(m => m.Id == id),
                CategoryType = _db.CategoryType.ToList(),
                FoodType = _db.FoodType.ToList()
            };

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension));
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension),
                    FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                MenuItemVM.MenuItem.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }

            var menuItemFromDb = _db.MenuItem.Where(m => m.Id == MenuItemVM.MenuItem.Id).FirstOrDefault();
            if (MenuItemVM.MenuItem.Image != null)
            {
                menuItemFromDb.Image = MenuItemVM.MenuItem.Image;
            }
            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.FoodTypeId = MenuItemVM.MenuItem.FoodTypeId;
            menuItemFromDb.CategoryId= MenuItemVM.MenuItem.CategoryId;

            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");

        }

    }
}