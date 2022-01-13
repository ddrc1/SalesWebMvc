using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services {
    public class SalesRecordService {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context) {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDate(DateTime? minDate, DateTime? maxDate) {
            var result = _context.SalesRecord.Select(x => x);
            
            if (minDate.HasValue) {
                result = result.Where(x => x.Date >= minDate);
            }

            if (maxDate.HasValue) {
                result = result.Where(x => x.Date <= maxDate);
            }

            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).ToListAsync();
        }
        public async Task<IEnumerable<IGrouping<Department, SalesRecord>>> FindByDateGrouping(DateTime? minDate, DateTime? maxDate) {
            var result = _context.SalesRecord.Select(x => x);

            if (minDate.HasValue) {
                result = result.Where(x => x.Date >= minDate);
            }

            if (maxDate.HasValue) {
                result = result.Where(x => x.Date <= maxDate);
            }

            var data = await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).ToListAsync();

            return data.GroupBy(s => s.Seller.Department).ToList();
        }
    }
}
