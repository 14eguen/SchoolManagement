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
    public class IndexModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public IndexModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public IList<Class> Classes { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            // Truy vấn lấy danh sách Lớp học và Giáo viên chủ nhiệm
            var query = _context.Classes
                .Include(c => c.HomeroomTeacher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(c => c.Name.Contains(SearchString));
            }

            // Tính số trang
            int totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            // Lấy dữ liệu theo phân trang
            Classes = await query
                .OrderByDescending(c => c.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
