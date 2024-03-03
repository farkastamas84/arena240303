using Microsoft.AspNetCore.Mvc;

namespace ArenaSimulator.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status501NotImplemented)]
public class ApiControllerBase : ControllerBase
{
}