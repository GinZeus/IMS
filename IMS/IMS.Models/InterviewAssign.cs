using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
	public class InterviewAssign
	{
		[Key]
		public int InterviewAssignId { get; set; }

		public int InterviewScheduleId {  get; set; }
		[ForeignKey("InterviewScheduleId")]
		public InterviewSchedule InterviewSchedule { get; set; }

		public string InterviewerId {  get; set; }
		[ForeignKey("InterviewerId")]
		public User User { get; set; }
	}
}
