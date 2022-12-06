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
    public class MemberController : ControllerBase
    {
        IMemberService _MemberService;

        public MemberController(IMemberService memberService)
        {
            _MemberService = memberService;
        }


        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult Get([FromRoute] int id)
        {
            Member? member = _MemberService.GetMember(id);

            if(member is null)
            {
                return NotFound();
            }

            return Ok(member.ToDTO());
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update([FromBody] MemberDataDTO dataDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Récuperation de l'id de l'utilisateur via les informations du token
            int id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                .Select(c => int.Parse(c.Value))
                                .Single();

            // Mise à jours des données de l'utilisateur
            Member data = dataDTO.ToEntity();
            data.Id = id;
            _MemberService.UpdateMember(data);

            return NoContent();
        } 
    }
}
