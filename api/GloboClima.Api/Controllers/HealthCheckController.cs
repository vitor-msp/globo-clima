using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("healthcheck")]
public class HealthcheckController
{
    [HttpGet]
    public ActionResult<string> Healthcheck() => "health";
}