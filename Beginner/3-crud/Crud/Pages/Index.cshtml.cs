using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext db, ILogger<IndexModel> logger) {
            _db = db;
            _logger = logger;
        }
        public IList<Customer> Customers { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Customers = await _db.Customers.AsNoTracking().ToListAsync();
            _logger.LogCritical("Attento!", Customers);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                _db.Remove(customer);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }


    }
}
