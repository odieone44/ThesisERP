
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThesisERP.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
}

