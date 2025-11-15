using RolePlayingGame.Application.Dtos.Response;
using RolePlayingGame.Application.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace RolePlayingGame.Api.Configuration
{
	[ExcludeFromCodeCoverage]
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (PlayerNotFoundApplicationException ex)
			{
				_logger.LogWarning("Player not found: {Message}", ex.Message);
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				context.Response.ContentType = "application/json";

				var response = new ErrorResponse { Message = ex.Message };
				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var response = new ErrorResponse { Message = "Internal server error" };
				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
		}
	}
}
