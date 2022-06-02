using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await Mediator.Send(new ProfileDetails.Query { Username = username }));
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile(EditProfile.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}