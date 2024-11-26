using GenAIChat.Application.Usecase;
using GenAIChat.Presentation.API.Controllers.Common;
using GenAIChat.Presentation.API.Controllers.Prompt.Request;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptController(PromptApplication prompt) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendAsync(/*[FromRoute] string chatId,*/ [FromBody] PromptSendRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorDto(ModelState));

            try
            {
                return Ok(await prompt.SendAsync(request.Prompt));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDto(ex));
            }
        }
    }
}
