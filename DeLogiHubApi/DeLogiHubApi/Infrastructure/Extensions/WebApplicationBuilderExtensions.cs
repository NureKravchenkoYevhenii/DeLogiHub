using DAL.DbContexts;
using Infrastructure.Configs;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeLogiHubApi.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder SetDefaultConfiguration(
		this WebApplicationBuilder builder)
	{
		builder.Host.ConfigureAutofac();

		var connectionModel = builder.Configuration.GetSection("Connection").Get<ConnectionModel>()!;

		var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? connectionModel.Host;
		var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? connectionModel.Database;
		var dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD") ?? connectionModel.Password;

		var connectionString = string.Format(connectionModel.ConnectionString, dbHost, dbName, dbPassword);

		builder.Services.AddDbContext<DeLogiHubDbContext>(options => options.UseSqlServer(connectionString));
		builder.Services.AddSingleton(connectionModel);

		var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>()!;
		authOptions.PrivateKeyString = builder.Configuration.GetSection("PrivateKeyString").Get<string>()!;

		builder.Services.AddSingleton(authOptions);

		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = authOptions.Issuer,
					ValidateAudience = true,
					ValidAudience = authOptions.Audience,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = authOptions.PublicKey,
					ClockSkew = TimeSpan.Zero
				};
			});

		builder.Services.AddMapper();
		builder.Services.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(options =>
		{
			options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
			options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
			{
				Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
				In = ParameterLocation.Header,
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey
			});

			options.OperationFilter<SecurityRequirementsOperationFilter>();
		});

		builder.Services.AddCors(option =>
		{
			option.AddPolicy(DeLogiHubConstants.ALLOW_ANY_ORIGINS, builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			});
		});

		return builder;
	}
}
