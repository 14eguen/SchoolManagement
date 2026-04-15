using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Classes
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public DetailsModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public Class Class { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classEntity = await _context.Classes.FirstOrDefaultAsync(m => m.Id == id);

            if (classEntity is not null)
            {
                Class = classEntity;

                return Page();
            }

            return NotFound();
        }
    }
}
