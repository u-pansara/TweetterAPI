using Microsoft.AspNetCore.Mvc;
using TweetsAPI.Services;
using TweetsAPI.Model;

namespace TweetsAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/tweets")]
    public class LoginController : ControllerBase
    {
        private LoginRepository _loginRepository;
        
        public   UserCredential UserCredential { get; set; }
        public LoginController(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCredential userCredential)
        {
            var respose = await _loginRepository.Register(
                new Model.User { UserName = userCredential.UserName, Email = userCredential.Email }, userCredential.Password);

            if (respose == null)
            {
                return BadRequest(respose);
            }
            return Ok(respose);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredential userCredential)
        {
            var response = await _loginRepository.Login(userCredential.UserName, userCredential.Password);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("user/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _loginRepository.GetAllUsers();
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpGet("user/search/{username}")]
        public async Task<IActionResult> SearchByUserName(string userName)
        {
            var response = await _loginRepository.SearchByUserName(userName);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
