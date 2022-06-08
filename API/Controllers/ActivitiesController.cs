using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class ActivitiesController : BaseApiController
{

    [HttpGet]
    [Produces( "application/json" )]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param)
    { 
        return HandlePagedResult(await Mediator.Send(new AllActivitiesList.Query{Params = param}));
    }
 
    [HttpGet("{id}")]
    [Produces( "application/json" )]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityDTO>> GetActivity(Guid id)
    {
        return HandleResult(await Mediator.Send(new ActivitiesDetails.Query { Id = id }));
    }

    [HttpPost]
    [Produces( "application/json" )]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { Activity = activity }));
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id}")]
    [Produces( "application/json" )]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditActivity(Guid id ,Activity activity)
    {
        activity.Id = id;
        return HandleResult(await Mediator.Send(new EditActivity.Command{ Activity = activity }));
        
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id}")]
    [Produces( "application/json" )]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
    }

    [HttpPost("{id}/attend")]
    [ProducesResponseType(typeof(ActivityDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command{ Id = id }));
    }
}