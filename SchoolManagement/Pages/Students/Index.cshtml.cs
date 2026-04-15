using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public IndexModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get; set; } = default!;

        // Các thuộc tính hỗ trợ phân trang và tìm kiếm
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            // Truy vấn cơ bản lấy tất cả Học sinh, kết hợp (Include) với bảng Lớp học và Người dùng liên quan
            var query = _context.Students
                .Include(s => s.Class)
                .Include(s => s.User)
                .AsQueryable();

            // Nếu người dùng nhập chuỗi tìm kiếm, lọc theo Tên học sinh hoặc Mã định danh
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.FullName.Contains(SearchString) || s.Address.Contains(SearchString));
            }

            // Tính tổng số lượng bản ghi sau khi lọc để phục vụ phân trang
            int totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            // Thực hiện Lấy dữ liệu với phân đoạn: Bỏ qua các bản ghi trang trước (Skip) và lấy số lượng của trang hiện tại (Take)
            Students = await query
                .OrderByDescending(s => s.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
