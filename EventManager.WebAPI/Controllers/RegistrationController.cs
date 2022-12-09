using EventManager.WebAPI.DataTransferObjects.Mappers;
using EventManager.WebAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using EventManager.BLL.Interfaces;

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
                return Forbid();
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
        public IActionResult JoinActivity([FromRoute] int activityId, [FromBody] ActivityGuestDTO dataDTO)
        {
            Activity? data = _ActivityService.GetActivity(activityId);
            if (data is null)
            {
                return NotFound();
            }

            bool isOk = _ActivityService.RejoinActivity(activityId, CurrentUserId, dataDTO.NbGuest);

            if (!isOk)
            {
                return BadRequest();
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
                return NotFound();
            }

            bool isOk = _ActivityService.LeaveActivity(activityId, CurrentUserId);

            if (!isOk)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
