using DeLogiHubApi.Infrastructure.Extensions;
using DeLogiHubApi.Infrastructure.Middleware;
using Infrastructure.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.SetDefaultConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseCors(DeLogiHubConstants.ALLOW_ANY_ORIGINS);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
