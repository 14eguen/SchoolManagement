using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Classes
{
    public class CreateModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public CreateModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["HomeroomTeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Class Class { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Classes.Add(Class);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
