using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models
{
    public class JobLevel
    {
        [Key]
        public int JobLevelId { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }

        public int LevelId { get; set; }
        [ForeignKey("LevelId")]
        public Level Level { get; set; }
    }
}
