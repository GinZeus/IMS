using IMS.Models;

namespace IMS.CoreServices
{
	public interface ICandidateSkillService
	{
		void AddCandidateSkill(CandidateSkill candidateSkill);
		void AddSkillsForCandidate(int candidateId, IEnumerable<int> skillId);
        Task CreateCandidateSkill(CandidateSkill candidateSkill);
        Task DeleteCandidateSkill(int candidateId);
        IEnumerable<int> GetSkillsByCandidateId(int candidateId);
	}
}
