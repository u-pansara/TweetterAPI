using Microsoft.AspNetCore.Mvc;
using TweetsAPI.Services;
using TweetsAPI.Model;

namespace TweetsAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetsRepository _TweetsRepository;

        public TweetsController(ITweetsRepository TweetsRepository)
        {
            _TweetsRepository = TweetsRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTweets()
        {
            var response = await _TweetsRepository.GetAllTweets();
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetAllTweetsforUser(string userName)
        {
            var response = await _TweetsRepository.GetAllTweetsforUser(userName);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("{useName}/add")]
        public async Task<IActionResult> Add(Tweets Tweets)
        {
            var response = await _TweetsRepository.Add(Tweets);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("{useName}/update/{id}")]
        public async Task<IActionResult> Update(int id, string TweetsDescription)
        {
            var response = await _TweetsRepository.Update(id, TweetsDescription);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{useName}/update/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _TweetsRepository.Delete(id);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("{useName}/like/{id}")]
        public async Task<IActionResult> LikeTweet(int id, bool flag, int userId)
        {
            var response = await _TweetsRepository.Like(id,flag, userId);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("{useName}/reply/{id}")]
        public async Task<IActionResult> TweetReply(int id, string reply, int userId)
        {
            var response = await _TweetsRepository.Reply(id, reply, userId);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
