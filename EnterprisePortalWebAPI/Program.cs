using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Service.Implementation;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnterprisePortalWebAPI.Utility;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using EnterprisePortalWebAPI.Filters;
using EnterprisePortalWebAPI.Utility.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowMultipleOrigins", builder =>
	{
		builder
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.WithExposedHeaders("X-Pagination")
				.AllowAnyMethod();
	});
});
builder.Services.AddDbContext<DatabaseContext>(options =>
								 options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapProfiles>(),
															 AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBusinessService, BusinessService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IOneTimePasswordService, OneTimePasswordService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(x =>
{
	x.SwaggerDoc("v1", new()
	{
		Version = "v1",
		Title = "Enterprise Portal Web API",
		Description = "This service contains APIs for the enterprise portal under the directive for Hobink Global Service INC"
	});
	x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
		Name = "Authorization",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	x.AddSecurityRequirement(new OpenApiSecurityRequirement
						{
								{
										new OpenApiSecurityScheme
										{
												Reference = new OpenApiReference
												{
														Type = ReferenceType.SecurityScheme,
														Id = "Bearer"
												}
										},
										Array.Empty<string>()
								}
						});
});
builder.Services.AddApiVersioning(config =>
{
	config.DefaultApiVersion = new ApiVersion(1, 0);
	config.AssumeDefaultVersionWhenUnspecified = true;
	config.ReportApiVersions = true;
	config.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddTokenAuthentication(builder.Configuration);
var app = builder.Build();
app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();


