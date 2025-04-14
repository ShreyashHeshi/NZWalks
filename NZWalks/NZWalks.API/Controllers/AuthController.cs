using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.CQRS.Command;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepositary tokenRepositary;
        private readonly IMediator mediator;
        private readonly RoleManager<IdentityRole> roleManger;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepositary tokenRepositary, IMediator mediator, RoleManager<IdentityRole> roleManger)
        {
            this.userManager = userManager;
            this.tokenRepositary = tokenRepositary;
            this.mediator = mediator;
            this.roleManger = roleManger;
        }


        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var registerUser = await mediator.Send(new RegisterAuthCommand(registerRequestDto));
            if (registerUser == null)
            {
                return BadRequest(new { message = "User registration failed. Please try again." });
            }

            return Ok(new { message = "User was registered successfully. Please login." });
            //return Ok(registerUser);


            /*var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                // add roles to this user
                if(registerRequestDto.Roles!=null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser,registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("user was registered please login");
                    }
                }
            }

            return BadRequest("Something went wrong");*/
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginUser = await mediator.Send(new LoginAuthCommand(loginRequestDto));
            if(loginUser== null)
            {
                return BadRequest("username and password not match");
            }
            return Ok(loginUser);





            /*var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null)
                    {
                        // Create Token
                        var jwtToken = tokenRepositary.CreateJWTToken(user,roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }


                    return Ok();
                }
            }

            return BadRequest("Username or Password incorrect");*/
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            try
            {
                var result = await mediator.Send(new RefreshTokenCommand
                {
                    AccessToken = refreshTokenRequest.AccessToken,
                    RefreshToken = refreshTokenRequest.RefreshToken
                });

                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while refreshing token" });
            }
        }





        // Add this to your AuthController
        [HttpGet]
        [Route("Roles")]
        public IActionResult GetAvailableRoles()
        {
            // You might want to get these from a database or configuration
            var availableRoles = roleManger.Roles.Select(r => r.Name).ToList();
            return Ok(availableRoles);
        }

    }
}
