using EventManager.BLL.Interfaces;
using EventManager.Domain.Entities;
using EventManager.WebAPI.DataTransferObjects;
using EventManager.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IMemberService _MemberService;
        TokenService _TokenService;

        public AuthController(IMemberService memberService, TokenService tokenService)
        {
            _MemberService = memberService;
            _TokenService = tokenService;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthTokenDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthLoginDTO loginDTO)
        {
            Member? member = _MemberService.Login(loginDTO.Identifier, loginDTO.Password);

            if (member is null)
            {
                ModelState.AddModelError("Message", "You have entered either the Username and/or Password incorrectly.");
                return ValidationProblem();
            }

            return Ok(new AuthTokenDTO()
            {
                Token = _TokenService.GenerateJwt(member)
            });
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] AuthRegisterDTO registerDTO)
        {
            Member? member = _MemberService.Register(new Member()
            {
                Pseudo = registerDTO.Pseudo,
                Email = registerDTO.Email,
                HashPwd = registerDTO.Password
            });

            if (member is null)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
