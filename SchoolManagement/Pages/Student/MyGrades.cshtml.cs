using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages.Student
{
    [Authorize]
    public class MyGradesModel : PageModel
    {
        private readonly SchoolManagementContext _context;
        public MyGradesModel(SchoolManagementContext context) => _context = context;

        public string StudentFullName { get; set; } = "";
        public List<Grade> Grades { get; set; } = new();

        public async Task OnGetAsync()
        {
            var username = User.Identity?.Name ?? string.Empty;
            var student = await _context.Students
                .Include(s => s.Grades).ThenInclude(g => g.Subject)
                .FirstOrDefaultAsync(s => s.User.Username == username);

            if (student != null)
            {
                StudentFullName = student.FullName;
                Grades = student.Grades
                    .OrderByDescending(g => g.Semester)
                    .ThenBy(g => g.Subject?.Name)
                    .ToList();
            }
        }
    }
}
