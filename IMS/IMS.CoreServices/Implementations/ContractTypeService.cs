using IMS.Data;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreServices.Implementations
{
    public class ContractTypeService : IContractTypeService
    {
        private readonly ApplicationDbContext _dbContext;

        public ContractTypeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        
        public IEnumerable<ContractType> GetContractTypes()
        {
            return _dbContext.ContractTypes;
        }

		public ContractType GetContractTypeById(int contractTypeId)
		{
			ContractType contractType = _dbContext.ContractTypes.Where(c => c.ContractTypeId == contractTypeId).FirstOrDefault();
			return contractType;
		}
	}
}
