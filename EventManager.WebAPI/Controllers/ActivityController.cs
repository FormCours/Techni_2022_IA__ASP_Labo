using EventManager.BLL.Interfaces;
using EventManager.Domain.Entities;
using EventManager.WebAPI.DataTransferObjects;
using EventManager.WebAPI.DataTransferObjects.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        IActivityService _ActivityService;

        public ActivityController(IActivityService activityService)
        {
            _ActivityService = activityService;
        }


        // Récuperation de l'id de l'utilisateur via les informations du token
        private int CurrentUserId => User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                .Select(c => int.Parse(c.Value))
                                                .SingleOrDefault(-1);


        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEvent([FromRoute] int id)
        {
            Activity? activity = _ActivityService.GetActivity(id);

            if (activity is null)
            {
                return NotFound();
            }

            return Ok(activity.ToDTO());
        }

        [HttpGet("{id}/Image")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityImageDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEventImage([FromRoute] int id)
        {
            Activity? data = _ActivityService.GetActivity(id);
            if (data is null)
            {
                return NotFound();
            }

            string imageSrc = data.ImageSrc ?? "default_activity.png";
            string file = Path.Combine(Environment.CurrentDirectory, "Images", imageSrc);

            var contents = System.IO.File.ReadAllBytes(file);
            return Ok(new ActivityImageDTO()
            {
                Name = data.Name ?? "Default",
                Data = Convert.ToBase64String(contents)
            });
        }

        [HttpGet("NextActivities")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActivityImageDTO>))]
        public IActionResult GetNextActivities()
        {
            IEnumerable<ActivityDTO> activities = _ActivityService.GetFutureActivities()
                                                                  .Select(ActivityMapper.ToDTO);

            return Ok(activities);
        }

        [HttpGet("MyActivities")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActivityImageDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMyActivities()
        {
            // Récuperation des events où l'utilisateur est inscrit
            IEnumerable<ActivityDTO> activities = _ActivityService.GetMemberActivities(CurrentUserId)
                                                                  .Select(c => c.ToDTO());

            return Ok(activities);
        }


        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Create([FromBody] ActivityDataDTO dataDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Activity data = dataDTO.ToEntity(CurrentUserId);
            Activity activity = _ActivityService.CreateActivity(data);

            return CreatedAtAction(nameof(GetEvent), new { id = activity.Id }, activity.ToDTO());
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] ActivityDataDTO dataDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Activity? data = _ActivityService.GetActivity(id);
            if (data is null)
            {
                return NotFound();
            }
            if (data.CreatorId != CurrentUserId)
            {
                return Forbid();
            }

            // Update data
            data.Name = dataDTO.Name;
            data.Description = dataDTO.Description;
            data.StartDate = dataDTO.StartDate;
            data.EndDate = dataDTO.EndDate;
            data.MaxGuest = dataDTO.MaxGuest;

            ActivityDTO result = _ActivityService.UpdateActivity(data).ToDTO();
            return Ok(result);
        }

        [HttpPost("{id}/UploadImage")]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult UpdateImage([FromRoute] int id, [FromForm] ActivityImageDataDTO imgForm)
        {
            Activity? data = _ActivityService.GetActivity(id);
            if (data is null)
            {
                return NotFound();
            }
            if (data.CreatorId != CurrentUserId)
            {
                return Forbid();
            }

            // Définition du répertoire pour sauvegarder les images
            string directory = Path.Combine(Environment.CurrentDirectory, "Images");

            // Définition du nom de fichier 
            string now = DateTime.UtcNow.ToString("yyyyMMdd");
            string rng = Guid.NewGuid().ToString();
            string ext = Path.GetExtension(imgForm.ActivityImage.FileName);
            string filename = now + "-" + rng + ext;

            // Création du chemin d'acces au fichier
            string file = Path.Combine(directory, filename);

            // Ouverture d'un flux d'ecriture pour le fichier
            using (FileStream fs = new FileStream(file, FileMode.CreateNew))
            {
                // Sauvegarder l'image via le flux
                imgForm.ActivityImage.CopyTo(fs);
            }

            // Update Data
            data.ImageName = Path.GetFileNameWithoutExtension(imgForm.ActivityImage.FileName);
            data.ImageSrc = filename;

            _ActivityService.UpdateActivity(data);
            return NoContent();
        }

        [HttpPatch("{id}/Cancel")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult CancelActivity([FromRoute] int id)
        {
            Activity? data = _ActivityService.GetActivity(id);
            if (data is null)
            {
                return NotFound();
            }
            if (data.CreatorId != CurrentUserId)
            {
                return Forbid();
            }

            // Update data
            data.IsCancel = true;

            ActivityDTO result = _ActivityService.UpdateActivity(data).ToDTO();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Delete([FromRoute] int id)
        {
            Activity? data = _ActivityService.GetActivity(id);
            if (data is null)
            {
                return NotFound();
            }
            if (data.CreatorId != CurrentUserId)
            {
                return Forbid();
            }

            _ActivityService.DeleteActivity(id);
            return NoContent();
        }
    }
}
