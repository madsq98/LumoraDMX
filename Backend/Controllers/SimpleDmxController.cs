using Backend.Types.SimpleDmx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQDmxController;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleDmxController : ControllerBase
    {
        private readonly DmxEngine _engine;

        public SimpleDmxController(DmxEngine engine)
        {
            _engine = engine;
        }

        [HttpPost("setChannel")]
        public IActionResult SetChannel(SetChannelType data)
        {
            _engine.SetChannel(data.Channel, data.Value);
            return Ok();
        }

        [HttpPost("fadeChannel")]
        public IActionResult FadeChannel(FadeChannelType data)
        {
            _engine.FadeChannel(data.Channel, data.Value, data.Duration);
            return Ok();
        }

        [HttpPost("animateChannel")]
        public IActionResult AnimateChannel(AnimateChannelType data)
        {
            _engine.AnimateChannel(data.Channel, data.StartValue, data.TargetValue, data.Duration, data.Cycles);
            return Ok();
        }
    }
}
