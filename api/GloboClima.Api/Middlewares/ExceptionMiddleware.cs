using System.Net;
using Amazon.DynamoDBv2.Model;
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
        if (error is ConditionalCheckFailedException) code = HttpStatusCode.Conflict;
        var output = ErrorPresenter.GenerateJson(error.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(output);
    }
}