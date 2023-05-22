namespace ChatApplication.Controllers.v1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase {}
