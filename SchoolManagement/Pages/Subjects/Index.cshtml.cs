using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Subjects
{
    public class IndexModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public IndexModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public IList<Subject> Subjects { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            var query = _context.Subjects.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.Name.Contains(SearchString));
            }

            int totalRecords = await query.CountAsync();
            TotalPages = totalRecords > 0 ? (int)Math.Ceiling(totalRecords / (double)PageSize) : 1;

            Subjects = await query
                .OrderByDescending(s => s.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
