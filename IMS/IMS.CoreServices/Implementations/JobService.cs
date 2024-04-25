using ClosedXML.Excel;
using IMS.Data;
using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class JobService : IJobInterface
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IJobSkillsInterface _jsService;
		private readonly IJobBenefitsInterface _jbService;
		private readonly IJobLevelsInterface _jlService;

		public JobService(ApplicationDbContext dbContext, IJobSkillsInterface jsService, IJobBenefitsInterface jbService, IJobLevelsInterface jlService)
		{
			_dbContext = dbContext;
			_jsService = jsService;
			_jbService = jbService;
			_jlService = jlService;
		}

		public async Task<int> CreateJob(Job job)
		{
			ArgumentNullException.ThrowIfNull(job);

			try
			{
				_dbContext.Job.Add(job);
				await _dbContext.SaveChangesAsync();
				return job.JobId;
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task CreateJobs(IEnumerable<Job> jobs)
		{
			ArgumentNullException.ThrowIfNull(jobs);

			await _dbContext.Job.AddRangeAsync(jobs);
			await _dbContext.SaveChangesAsync();
		}

		public IEnumerable<Job> GetAllJobs()
		{
			var jobs = _dbContext.Job
				.Include(j => j.JobBenefits)
					.ThenInclude(jb => jb.Benefit)
				.Include(j => j.JobLevels)
					.ThenInclude(jl => jl.Level)
				.Include(j => j.JobSkills)
					.ThenInclude(js => js.Skill)
				.Where(j => j.IsDeleted == false);
			return jobs;
		}

		public IEnumerable<Job> GetJobsByStatus(JobStatus? status)
		{
			var jobs = GetAllJobs();
			var jobsByStatus = jobs.Where(j => j.Status == status).ToList();
			return jobsByStatus;
		}

		public Job GetJobById(int jobId)
		{
			var job = _dbContext.Job
		.Include(j => j.JobBenefits)
			.ThenInclude(jb => jb.Benefit)
		.Include(j => j.JobLevels)
			.ThenInclude(jl => jl.Level)
		.Include(j => j.JobSkills)
			.ThenInclude(js => js.Skill)
		.FirstOrDefault(j => j.JobId == jobId);

			return job;
		}

		public async Task UpdateJob(Job job)
		{
			if (job == null)
				throw new ArgumentNullException(nameof(job));

			try
			{
				_dbContext.Entry(job).State = EntityState.Modified;
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!JobExists(job.JobId))
				{
					throw new Exception($"Job with ID {job.JobId} not found.");
				}
				else
				{
					throw;
				}
			}
		}
		private bool JobExists(int id)
		{
			return _dbContext.Job.Any(e => e.JobId == id);
		}

		public async Task<IEnumerable<Job>> GetUnclosedJobs()
		{
			try
			{
				return await _dbContext.Job.Where(j => j.Status != JobStatus.Closed).ToListAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		/// <summary>
		/// Get 
		/// </summary>
		/// <param name="worksheet"></param>
		/// <param name="rowNumber"></param>
		/// <returns></returns>
		private Job GetJobInExcelRow(IXLWorksheet worksheet, int rowNumber)
		{
			// acess the row
			IXLRow row = worksheet.Row(rowNumber);

			string jobTitle = row.Cell(((int)JobColumn.JobTitle)).GetValue<string>();
			DateTime startDate = row.Cell(((int)JobColumn.StartDate)).GetValue<DateTime>();
			DateTime endDate = row.Cell(((int)JobColumn.EndDate)).GetValue<DateTime>();
			int salaryFrom = row.Cell(((int)JobColumn.SalaryFrom)).GetValue<int>();
			int salaryTo = row.Cell(((int)JobColumn.SalaryTo)).GetValue<int>();
			string workingAddress = row.Cell(((int)JobColumn.WorkingAddress)).GetValue<string>();
			string description = row.Cell(((int)JobColumn.Description)).GetValue<string>();

			// create job instance
			Job job = new Job
			{
				Title = jobTitle,
				StartDate = startDate,
				EndDate = endDate,
				SalaryFrom = salaryFrom,
				SalaryTo = salaryTo,
				WorkingAddress = workingAddress,
				Description = description,
				// default all jobs imported from Excel status will be Open
				Status = JobStatus.Open
			};

			return job;
		}

		private IEnumerable<Skill> GetJobSkillsInExcelRow(IXLWorksheet worksheet, int rowNumber)
		{
			// acess the row
			IXLRow row = worksheet.Row(rowNumber);

			// read required skills in CSV format
			string csvSkill = row.Cell(((int)JobColumn.RequiredSkills)).GetValue<string>();
			List<string> skillNames = ReadCSV(csvSkill);

			// Get Skill object
			List<Skill> skills = _dbContext.Skill.Where(s => skillNames.Contains(s.SkillName)).ToList();
			return skills;
		}

		private IEnumerable<Benefit> GetJobBenefitsInExcelRow(IXLWorksheet worksheet, int rowNumber)
		{
			// acess the row
			IXLRow row = worksheet.Row(rowNumber);

			// read required skills in CSV format
			string csvSkill = row.Cell(((int)JobColumn.Benefits)).GetValue<string>();
			List<string> benefitNames = ReadCSV(csvSkill);

			// Get Skill object
			List<Benefit> benefits = _dbContext.Benefit.Where(b => benefitNames.Contains(b.BenefitName)).ToList();
			return benefits;
		}

		private IEnumerable<Level> GetJobLevelsInExcelRow(IXLWorksheet worksheet, int rowNumber)
		{
			// acess the row
			IXLRow row = worksheet.Row(rowNumber);

			// read required skills in CSV format
			string csvSkill = row.Cell(((int)JobColumn.Level)).GetValue<string>();
			List<string> levelNames = ReadCSV(csvSkill);

			// Get Skill object
			List<Level> levels = _dbContext.Levels.Where(l => levelNames.Contains(l.LevelName)).ToList();
			return levels;
		}

		private List<string> ReadCSV(string csvText)
		{
			List<string> lines = csvText.Split(',').Select(x => x.Trim()).ToList();
			return lines;
		}

		public async Task ImportJob(Stream stream, string? lastUpdateBy = null)
		{
			// Create workbook
			XLWorkbook wb = new XLWorkbook(stream);

			// Select the first worksheet
			IXLWorksheet worksheet = wb.Worksheet(1);

			// Loop through all the rows
			int rowCount = worksheet.LastRowUsed().RowNumber();
			for (int i = 2; i <= rowCount; i++)
			{
				// Insert Job
				Job job = GetJobInExcelRow(worksheet, i);
				job.CreatedOn = DateTime.Now;
				job.LastUpdatedBy = lastUpdateBy;
				job.LastUpdatedOn = DateTime.Now;
				await CreateJob(job);

				IEnumerable<Skill> requiredSkills = GetJobSkillsInExcelRow(worksheet, i);
				IEnumerable<Benefit> benefits = GetJobBenefitsInExcelRow(worksheet, i);
				IEnumerable<Level> levels = GetJobLevelsInExcelRow(worksheet, i);

				// Insert JobSkill
				foreach (Skill skill in requiredSkills)
				{
					JobSkill js = new JobSkill
					{
						SkillId = skill.SkillId,
						JobId = job.JobId
					};

					await _jsService.CreateJobSkill(js);
				}

				// Insert JobBenefit
				foreach (Benefit benefit in benefits)
				{
					JobBenefit jb = new JobBenefit
					{
						BenefitId = benefit.BenefitId,
						JobId = job.JobId
					};

					await _jbService.CreateJobBenefit(jb);
				}

				// Insert Level
				foreach (Level level in levels)
				{
					JobLevel jl = new JobLevel
					{
						LevelId = level.LevelId,
						JobId = job.JobId
					};

					await _jlService.CreateJobLevel(jl);
				}
			}
		}

		public async Task<int> DeleteJob(Job job)
		{
			ArgumentNullException.ThrowIfNull(job);

			try
			{
				job.IsDeleted = true;
				await _dbContext.SaveChangesAsync();
				return job.JobId;
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
