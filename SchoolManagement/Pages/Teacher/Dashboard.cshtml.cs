using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages.Teacher
{
    [Authorize(Policy = "RequireTeacher")]
    public class DashboardModel : PageModel
    {
        private readonly SchoolManagementContext _context;

        public DashboardModel(SchoolManagementContext context)
        {
            _context = context;
        }

        public string TeacherFullName { get; set; } = string.Empty;
        public int MyStudentCount { get; set; }
        public int MyClassCount { get; set; }
        public int MyTimetableCount { get; set; }
        public List<Class> MyClasses { get; set; } = new();
        public List<Timetable> UpcomingTimetables { get; set; } = new();

        public async Task OnGetAsync()
        {
            var username = User.Identity?.Name ?? string.Empty;

            // Tìm Teacher tương ứng với tài khoản đang đăng nhập
            var teacher = await _context.Teachers
                .Include(t => t.Classes)
                .FirstOrDefaultAsync(t => t.User != null && t.User.Username == username);

            if (teacher != null)
            {
                TeacherFullName = teacher.FullName ?? username;

                MyClasses = await _context.Classes
                    .Where(c => c.HomeroomTeacherId == teacher.Id)
                    .ToListAsync();

                MyClassCount = MyClasses.Count;

                MyStudentCount = await _context.Students
                    .Where(s => MyClasses.Select(c => c.Id).Contains(s.ClassId))
                    .CountAsync();

                UpcomingTimetables = await _context.Timetables
                    .Include(t => t.Class)
                    .Include(t => t.Subject)
                    .Where(t => t.TeacherId == teacher.Id)
                    .OrderBy(t => t.DayOfWeek)
                    .ThenBy(t => t.Period)
                    .Take(10)
                    .ToListAsync();

                MyTimetableCount = UpcomingTimetables.Count;
            }
            else
            {
                TeacherFullName = username;
            }
        }
    }
}
