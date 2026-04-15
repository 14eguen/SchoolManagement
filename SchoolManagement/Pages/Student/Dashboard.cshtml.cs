using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages.Student
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly SchoolManagementContext _context;

        public DashboardModel(SchoolManagementContext context)
        {
            _context = context;
        }

        public string StudentFullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = "Chưa xếp lớp";
        public int GradeCount { get; set; }
        public int TimetableCount { get; set; }
        public List<Timetable> MyTimetables { get; set; } = new();
        public List<Grade> MyGrades { get; set; } = new();

        public async Task OnGetAsync()
        {
            var username = User.Identity?.Name ?? string.Empty;

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Grades).ThenInclude(g => g.Subject)
                .FirstOrDefaultAsync(s => s.User.Username == username);

            if (student != null)
            {
                StudentFullName = student.FullName;
                ClassName = student.Class?.Name ?? "Chưa xếp lớp";

                MyGrades = student.Grades
                    .OrderByDescending(g => g.Semester)
                    .Take(6)
                    .ToList();
                GradeCount = student.Grades.Count;

                // Lấy TKB theo lớp của học sinh
                if (student.ClassId != null)
                {
                    MyTimetables = await _context.Timetables
                        .Include(t => t.Subject)
                        .Include(t => t.Teacher)
                        .Where(t => t.ClassId == student.ClassId)
                        .OrderBy(t => t.DayOfWeek)
                        .ThenBy(t => t.Period)
                        .ToListAsync();
                    TimetableCount = MyTimetables.Count;
                }
            }
            else
            {
                StudentFullName = username;
            }
        }
    }
}
