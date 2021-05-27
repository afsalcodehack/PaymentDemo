using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.IRepositories;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo, ILogger<UsersController> logger)
            : base(logger)
        {
            _userRepo = userRepo;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            Guard.Against.Null(model, nameof(model));

            try
            {
                var response = _userRepo.Authenticate(model);

                if (response == null)
                {
                    return NoContent();
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                Logger.LogInformation(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepo.GetAll();
            var userID = this.User.Claims.First(x => x.Type == ClaimTypes.Role);
            return Ok(users);
        }
    }
}
