using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Service.Implementation;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EnterprisePortalWebAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DatabaseContext>(options =>
								 options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapProfiles>(),
															 AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(config =>
{
	config.DefaultApiVersion = new ApiVersion(1, 0);
	config.AssumeDefaultVersionWhenUnspecified = true;
	config.ReportApiVersions = true;
	config.ApiVersionReader = new UrlSegmentApiVersionReader();
});
var app = builder.Build();
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


