using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SchoolManagementContext _context;

        public IndexModel(SchoolManagementContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public string Username { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Username = User.Identity?.Name ?? "Admin";

            // Lấy thống kê nhanh từ Database
            TotalStudents = await _context.Students.CountAsync();
            TotalTeachers = await _context.Teachers.CountAsync();
            TotalClasses = await _context.Classes.CountAsync();
        }
    }
}
