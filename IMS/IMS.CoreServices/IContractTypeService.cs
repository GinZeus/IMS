using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreServices
{
    public interface IContractTypeService
    {
        public IEnumerable<ContractType> GetContractTypes();
        public ContractType GetContractTypeById(int id);
    }
}
