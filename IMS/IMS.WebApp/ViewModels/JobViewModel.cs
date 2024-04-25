using IMS.Models;

namespace IMS.WebApp.ViewModels
{
	public class JobViewModel
	{
		public required Job Job { get; set; }
		public required IEnumerable<Skill> Skills { get; set; }
		public required IEnumerable<Benefit> Benefits { get; set; }

		public required IEnumerable<Level> Levels { get; set; }
	}
}