using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BoardCore.Services;
using BoardCore.Models;

namespace BoardCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private ILogger<UsersController> _logger;

        public UsersController(IUserService userService,ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger=logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody]ApiUsers model)
        {
            _logger.LogInformation("Authenticate user:{0} pass:{1}",model.Username,model.Password);

            var user =await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            _logger.LogInformation("Authenticate userid:{0}",user.UserId);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("~/api/users/getlist")]
        public IActionResult GetAll2()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}