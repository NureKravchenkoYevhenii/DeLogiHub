using System.Text.Json.Serialization;
using System.Text.Json;
using Infrastructure.Exceptions;

namespace DeLogiHubApi.Infrastructure.Middleware;

public static class ErrorHandler
{
	private static readonly JsonSerializerOptions _jsonSerializerOptions =
		new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};

	public static IApplicationBuilder UseErrorHandler(
		this IApplicationBuilder applicationBuilder)
	{
		return applicationBuilder.Use(HandleError);
	}

	private static async Task HandleError(
		HttpContext httpContext,
		Func<Task> next)
	{
		try
		{
			await next();
		}
		catch (Exception ex)
		{
			string message;
			object data;

			switch (ex)
			{
				case UnauthorizedAccessException:
					httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
					message = ex.Message;
					data = ex.Data;
					break;
				case DeLogiHubException:
					httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
					message = ex.Message;
					data = ex.Data;
					break;
				default:
					httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
					message = ex.Message;
					data = ex.ToString();
					break;
			}

			var error = new
			{
				Message = message,
				Error = data
			};

			var errorJson = JsonSerializer.Serialize(
				error,
				_jsonSerializerOptions);

			await httpContext.Response.WriteAsync(errorJson);
		}
	}
}
