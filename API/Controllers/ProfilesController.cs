using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        return HandleResult(await Mediator.Send(new AllProfilesList.Query()));
    }

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

    [HttpGet("{username}/activities")]
    public async Task<IActionResult> GetUserActivities(string username, string predicate)
    {
        return HandleResult(
            await Mediator.Send(new ListActivities.Query { Username = username, Predicate = predicate }));
    }
}