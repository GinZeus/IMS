using IMS.Models;
using IMS.Utilities.Constants;

namespace IMS.CoreServices
{
    public interface ICandidateService
    {
        public IEnumerable<Candidate> GetCandidatesNotBannedAndDeleted();
        public void UpdateCandidateStatus(int candidateId, CandidateStatus newStatus);
        IEnumerable<Candidate> GetAvailableCandidates();
        Candidate CreateCandidate(Candidate candidate);
        byte[] GetCandidateCv(int candidateId);
        Candidate? GetCandidate(int candidateId);
        IEnumerable<Skill> GetCandidateSkill(int candidateId);
        Task<int> UpdateCandidate(Candidate candidateToUpdate);
        Task<int> CountAllCandidates();
        Task<int> DeleteCandidate(Candidate candidate);
    }
}
