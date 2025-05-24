using System.Net;
using Amazon.DynamoDBv2.Model;
using GloboClima.Api.Exceptions;
using GloboClima.Api.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next) : ControllerBase
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleException(context, error);
        }
    }

    private static Task HandleException(HttpContext context, Exception error)
    {
        var code = HttpStatusCode.InternalServerError;
        var errorMessage = error.Message;
        if (error is DomainException) code = HttpStatusCode.UnprocessableEntity;
        if (error is UnauthorizeException) code = HttpStatusCode.Unauthorized;
        if (error is ConditionalCheckFailedException)
        {
            code = HttpStatusCode.Conflict;
            errorMessage = "Username already in use.";
        }
        var output = ErrorPresenter.GenerateJson(errorMessage);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(output);
    }
}