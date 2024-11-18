using GenAIChat.Application;
using GenAIChat.Presentation.API.Controllers.Request;
using GenAIChat.Presentation.API.Controllers.Response;
using Microsoft.AspNetCore.Mvc;

namespace GenAIChat.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Route("chat/{chatId}/[controller]")]
    public class PromptController(PromptApplication prompt) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendAsync(/*[FromRoute] string chatId,*/ [FromForm] PromptSendRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorResponse(ModelState));

            try
            {
                return Ok(await prompt.SendAsync(request.Prompt));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}
