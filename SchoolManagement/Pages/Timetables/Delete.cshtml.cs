using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Timetables
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public DeleteModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Timetable Timetable { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables
                .Include(t => t.Class)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (timetable == null)
            {
                return NotFound();
            }
            else 
            {
                Timetable = timetable;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables.FindAsync(id);

            if (timetable != null)
            {
                Timetable = timetable;
                _context.Timetables.Remove(Timetable);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
