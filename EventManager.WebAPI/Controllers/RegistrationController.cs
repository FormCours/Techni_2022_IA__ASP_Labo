using EventManager.WebAPI.DataTransferObjects.Mappers;
using EventManager.WebAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using EventManager.BLL.Interfaces;
using EventManager.Domain.Enums;

namespace EventManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegistrationController : ControllerBase
    {
        IActivityService _ActivityService;

        public RegistrationController(IActivityService activityService)
        {
            _ActivityService = activityService;
        }


        // Récuperation de l'id de l'utilisateur via les informations du token
        private int CurrentUserId => User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                .Select(c => int.Parse(c.Value))
                                                .SingleOrDefault(-1);

        [HttpGet("{activityId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MemberRegistrationDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRegistrations([FromRoute] int activityId)
        {
            Activity? data = _ActivityService.GetActivity(activityId);
            if (data is null)
            {
                return NotFound();
            }
            if (data.CreatorId != CurrentUserId)
            {
                return Problem(
                    detail: "Only the creator can get the registration list of the activity",
                    statusCode: StatusCodes.Status403Forbidden
                );
            }

            IEnumerable<MemberRegistrationDTO> registrations;
            registrations = _ActivityService.GetActivityRegistrations(activityId)
                                            .Select(RegistrationMapper.ToDTO);

            return Ok(registrations);
        }

        [HttpPost("Join/{activityId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult JoinActivity([FromRoute] int activityId, [FromBody] RegistrationGuestDTO dataDTO)
        {
            Activity? data = _ActivityService.GetActivity(activityId);
            if (data is null)
            {
                return Problem(
                    detail: "Activity not found",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            RegistrationResult reg = _ActivityService.RejoinActivity(activityId, CurrentUserId, (int)dataDTO.NbGuest);

            if(reg == RegistrationResult.TooManyGuest)
            {
                ModelState.AddModelError("NbGuest", "Too Many Guest");
                return ValidationProblem();
            }

            if (reg != RegistrationResult.Success)
            {
                string? message = (reg == RegistrationResult.AlreadyExists) 
                    ? "Your registration already exists"
                    : "Error on create the registration for the activity";

                return Problem(
                    detail: message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            return NoContent();
        }

        [HttpPost("Leave/{activityId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult LeaveActivity([FromRoute] int activityId)
        {
            Activity? data = _ActivityService.GetActivity(activityId);
            if (data is null)
            {
                return Problem(
                    detail: "Activity not found",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            RegistrationResult reg = _ActivityService.LeaveActivity(activityId, CurrentUserId);

            if (reg != RegistrationResult.Success)
            {
                return Problem(
                    detail: "Error on remove the registration for the activity",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            return NoContent();
        }
    }
}
