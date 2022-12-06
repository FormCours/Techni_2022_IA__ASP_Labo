using EventManager.BLL.Interfaces;
using EventManager.Domain.Entities;
using EventManager.WebAPI.DataTransferObjects;
using EventManager.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Login([FromBody] AuthLoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            Member? member = _MemberService.Login(loginDTO.Identifier, loginDTO.Password);

            if(member is null)
            {
                return Problem(
                    detail: "You have entered either the Username and/or Password incorrectly.",
                    statusCode: StatusCodes.Status400BadRequest
                ) ;
            }

            return Ok(new AuthTokenDTO()
            {
                Token = _TokenService.GenerateJwt(member)
            });
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] AuthRegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

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
