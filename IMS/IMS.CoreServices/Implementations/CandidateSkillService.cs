using IMS.Data;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class CandidateSkillService : ICandidateSkillService
	{
		private readonly ApplicationDbContext _context;

		public CandidateSkillService(ApplicationDbContext context)
		{
			_context = context;
		}

		public void AddCandidateSkill(CandidateSkill candidateSkill)
		{
			_context.CandidateSkills.Add(candidateSkill);
			_context.SaveChanges();
		}

		public void AddSkillsForCandidate(int candidateId, IEnumerable<int> skillId)
		{
			foreach (int id in skillId)
			{
				var candidateSkill = new CandidateSkill { CandidateId = candidateId, SkillId = id };
				_context.CandidateSkills.Add(candidateSkill);
			}

			_context.SaveChanges();
		}

        public async Task CreateCandidateSkill(CandidateSkill candidateSkill)
        {
            ArgumentNullException.ThrowIfNull(candidateSkill);

            try
            {
                _context.CandidateSkills.Add(candidateSkill);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task DeleteCandidateSkill(int candidateId)
        {
            try
            {
                var candidateSkills = await _context.CandidateSkills.Where(cs => cs.CandidateId == candidateId).ToListAsync();

                if (candidateSkills.Any())
                {
                    _context.CandidateSkills.RemoveRange(candidateSkills);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IEnumerable<int> GetSkillsByCandidateId(int candidateId)
        {
            var Skill = _context.CandidateSkills
            .Where(cs => cs.CandidateId == candidateId).Select(cs => cs.SkillId)
            .ToList();
            return Skill;
        }
    }
}
