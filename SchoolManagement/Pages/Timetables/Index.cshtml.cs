using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Timetables
{
    public class IndexModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public IndexModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public IList<Timetable> Timetables { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;

            var query = _context.Timetables
                .Include(t => t.Class)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(t => t.Class.Name.Contains(SearchString) 
                                      || t.Subject.Name.Contains(SearchString) 
                                      || t.Teacher.FullName.Contains(SearchString));
            }

            int totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Timetables = await query
                .OrderByDescending(t => t.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
