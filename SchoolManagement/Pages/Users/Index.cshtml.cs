using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Pages_Users
{
    public class IndexModel : PageModel
    {
        private readonly SchoolManagement.Models.SchoolManagementContext _context;

        public IndexModel(SchoolManagement.Models.SchoolManagementContext context)
        {
            _context = context;
        }

        public new IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
