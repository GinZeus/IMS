using IMS.Models;

namespace IMS.CoreServices
{
    public interface IBenefitService
    {
        IEnumerable<Benefit> GetBenefits();
    }
}
