using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Timetables
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Timetable Timetable { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove navigational and nested validation errors
            ModelState.Remove("Timetable.Class");
            ModelState.Remove("Timetable.Subject");
            ModelState.Remove("Timetable.Teacher");

            if (!ModelState.IsValid)
            {
                ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
                ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
                ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
                return Page();
            }

            _context.Timetables.Add(Timetable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
