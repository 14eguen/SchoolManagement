using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages.Student
{
    [Authorize]
    public class MyTimetableModel : PageModel
    {
        private readonly SchoolManagementContext _context;
        public MyTimetableModel(SchoolManagementContext context) => _context = context;

        public string ClassName { get; set; } = "";
        public List<Timetable> Timetables { get; set; } = new();

        public async Task OnGetAsync()
        {
            var username = User.Identity?.Name ?? string.Empty;
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.User.Username == username);

            if (student?.ClassId != null)
            {
                ClassName = student.Class?.Name ?? "";
                Timetables = await _context.Timetables
                    .Include(t => t.Subject)
                    .Include(t => t.Teacher)
                    .Where(t => t.ClassId == student.ClassId)
                    .OrderBy(t => t.DayOfWeek).ThenBy(t => t.Period)
                    .ToListAsync();
            }
        }
    }
}
