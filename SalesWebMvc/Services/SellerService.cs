using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services {
    public class SellerService {

        private readonly SalesWebMvcContext _context;
        public SellerService(SalesWebMvcContext context) {
            _context = context;
        }

        public async Task<List<Seller>> FindAll() {
            return await _context.Seller.ToListAsync();
        }

        public async Task Insert(Seller seller) {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindById(int id) { 
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Remove(int id) {
            try {
                var obj = _context.Seller.Find(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            } catch (DbUpdateException ex) {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task Update(Seller seller) {
            if (!await _context.Seller.AnyAsync(x => x.Id == seller.Id)) {
                throw new NotFoundException("Id not found");
            }
            try {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException e) {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
