using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public DetailsModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);

            if (teacher is not null)
            {
                Teacher = teacher;

                return Page();
            }

            return NotFound();
        }
    }
}
