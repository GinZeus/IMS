using IMS.Data;
using IMS.Models;

namespace IMS.CoreServices.Implementations
{
    public class BenefitService : IBenefitService
    {
        private readonly ApplicationDbContext _context;

        public BenefitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Benefit> GetBenefits()
        {
            return _context.Benefit;
        }
    }
}
