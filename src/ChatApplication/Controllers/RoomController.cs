namespace ChatApplication.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class RoomController : Controller
{
    public IActionResult Chat()
        => View();
}
