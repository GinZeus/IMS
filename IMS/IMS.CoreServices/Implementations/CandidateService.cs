using IMS.Data;
using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class CandidateService : ICandidateService
	{
		private readonly ApplicationDbContext _dbContext;

		public CandidateService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<Candidate> GetCandidatesNotBannedAndDeleted()
		{
			return _dbContext.Candidates.Where(c => c.Status != CandidateStatus.Banned && c.IsDeleted == false);
		}

		public void UpdateCandidateStatus(int candidateId, CandidateStatus newStatus)
		{
			var candidateToUpdate = _dbContext.Candidates.FirstOrDefault(c => c.CandidateId == candidateId);
			if (candidateToUpdate != null)
			{
				candidateToUpdate.Status = newStatus;
				_dbContext.SaveChanges();
			}
		}

		public Candidate CreateCandidate(Candidate candidate)
		{
			ArgumentNullException.ThrowIfNull(candidate, nameof(candidate));

			_dbContext.Candidates.Add(candidate);
			_dbContext.SaveChanges();

			return candidate;
		}

		public byte[] GetCandidateCv(int candidateId)
		{
			Candidate? candidate = _dbContext.Candidates.Where(c => c.CandidateId == candidateId).FirstOrDefault();
			MemoryStream m = new MemoryStream(candidate.CVAttachment);
			return m.ToArray();
		}

		public IEnumerable<Candidate> GetAvailableCandidates()
		{
			return _dbContext.Candidates.Include(c => c.Position).Include(c => c.Recruiter).Where(c => c.IsDeleted == false);
		}


		public Candidate? GetCandidate(int candidateId)
		{
			Candidate? candidate = _dbContext.Candidates.Include(c => c.Position).Include(c => c.Recruiter).Where(c => (c.CandidateId == candidateId && c.IsDeleted == false)).FirstOrDefault();
			return candidate;
		}

		public IEnumerable<Skill> GetCandidateSkill(int candidateId)
		{
			return _dbContext.CandidateSkills.Where(cs => cs.CandidateId == candidateId).Include(cs => cs.Skill).Select(cs => cs.Skill);
		}

        public async Task<int> UpdateCandidate(Candidate candidate)
        {
            ArgumentNullException.ThrowIfNull(candidate);

            try
            {
                _dbContext.Candidates.Update(candidate);
                await _dbContext.SaveChangesAsync();
                return candidate.CandidateId;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public async Task<int> CountAllCandidates()
        {
            return await _dbContext.Candidates.CountAsync();
		}
        public async Task<int> DeleteCandidate(Candidate candidate)
        {
            ArgumentNullException.ThrowIfNull(candidate);

            try
            {
				candidate.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
                return candidate.CandidateId;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
