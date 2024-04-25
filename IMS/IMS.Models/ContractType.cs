using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class ContractType
    {
        [Key]
        public int ContractTypeId { get; set; }
        [Required]
        public required string ContractTypeTitle { get; set; }
    }
}
