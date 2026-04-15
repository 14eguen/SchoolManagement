using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Timetables
{
    public class EditModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public EditModel(SchoolManagement.Models.SchoolManagementContext context)
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

            var timetable = await _context.Timetables.FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }
            Timetable = timetable;
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", Timetable.ClassId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", Timetable.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", Timetable.TeacherId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Timetable.Class");
            ModelState.Remove("Timetable.Subject");
            ModelState.Remove("Timetable.Teacher");

            if (!ModelState.IsValid)
            {
                ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", Timetable.ClassId);
                ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", Timetable.SubjectId);
                ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", Timetable.TeacherId);
                return Page();
            }

            _context.Attach(Timetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(Timetable.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetables.Any(e => e.Id == id);
        }
    }
}
