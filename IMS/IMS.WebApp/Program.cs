using IMS.Data;
using Microsoft.EntityFrameworkCore;
using IMS.Models;
using IMS.CoreServices;
using IMS.CoreServices.Implementations;
using Microsoft.AspNetCore.Identity;
using IMS.HtmlEmail;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 7;
	options.Password.RequiredUniqueChars = 0;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	options.User.RequireUniqueEmail = true;
});

// Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	options.SlidingExpiration = true;
});

// Add logging
builder.Logging.AddFile();

// HostedService (Singleton)
builder.Services.AddHostedService<JobStatusService>();
builder.Services.AddHostedService<InterviewReminderService>();
builder.Services.AddHostedService<OfferReminderService>();

builder.Services.AddScoped<IInterviewScheduleService, InterviewScheduleService>();
builder.Services.AddScoped<IInterviewAssignService, InterviewAssignService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IRecoveryToken, RecoveryTokenService>();
builder.Services.AddScoped<IJobInterface, JobService>();
builder.Services.AddScoped<IJobBenefitsInterface, JobBenefitsService>();
builder.Services.AddScoped<IJobSkillsInterface, JobSkillsService>();
builder.Services.AddScoped<IJobLevelsInterface, JobLevelsService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IContractTypeService, ContractTypeService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IAcademicLevelService, AcademicLevelService>();
builder.Services.AddScoped<ICandidateSkillService, CandidateSkillService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapRazorPages();

app.Run();
