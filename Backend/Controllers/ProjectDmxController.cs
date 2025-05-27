using Backend.Services;
using Backend.Types.Fixture;
using Backend.Types.Project;
using Backend.Types.ProjectDmx;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQDmxController;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDmxController : ControllerBase
    {
        private readonly LumoraContext _context;
        private readonly Dictionary<string, FixtureTemplate> _templates;
        private readonly DmxEngine _engine;

        public ProjectDmxController(LumoraContext context, DmxEngine engine, IWebHostEnvironment env)
        {
            _context = context;
            _engine = engine;

            var templateList = FixtureTemplateLoader.LoadAll(
                Path.Combine(env.ContentRootPath, "Fixtures")
                );

            _templates = templateList.ToDictionary(t => t.Name);
        }

        [HttpPost("{projectId:int}/dmxAction")]
        public IActionResult DoDmxAction(int projectId, DmxActionType[] data)
        {
            try
            {
                var project = ProjectController.ConvertToOutputProjectType(GetSingleProject(projectId), _templates);

                foreach(var action in data)
                {
                    ExecuteAction(project, action);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        private void ExecuteAction(OutputProjectType project, DmxActionType data)
        {
            if(!project.Fixtures.Any(obj => obj.Id == data.FixtureId))
            {
                return;
            }

            var fixture = project.Fixtures.First(obj => obj.Id == data.FixtureId);
            var fixtureChannels = fixture.FixtureTemplate.Channels;

            if (!fixtureChannels.TryGetValue(data.Channel, out var channel))
            {
                return;
            }

            var targetDmxChannel = fixture.StartAddress + (channel - 1);

            switch(data.Action)
            {
                case "snap":
                    _engine.SetChannel(targetDmxChannel, data.TargetValue);
                    break;
                case "fade":
                    if(data.Duration != null)
                        _engine.FadeChannel(targetDmxChannel, data.TargetValue, data.Duration ?? 0);
                    break;
                case "animate":
                    if (data.StartValue != null && data.Duration != null && data.Cycles != null)
                        _engine.AnimateChannel(targetDmxChannel, data.StartValue ?? 0, data.TargetValue, data.Duration ?? 0, data.Cycles ?? 0);
                    break;
                default:
                    return;
            }
        }

        private Project GetSingleProject(int id)
        {
            return _context
                .Projects
                .Include(p => p.Fixtures)
                .FirstOrDefault(obj => obj.Id == id)
                ?? throw new KeyNotFoundException("Project does not exist");
        }
    }
}
