using System.Reflection.Metadata.Ecma335;
using Backend.Services;
using Backend.Types.Fixture;
using Backend.Types.Project;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly LumoraContext _context;
        private readonly Dictionary<string, FixtureTemplate> _templates;

        public ProjectController(LumoraContext context, IWebHostEnvironment env)
        {
            _context = context;

            var templateList = FixtureTemplateLoader.LoadAll(
                Path.Combine(env.ContentRootPath, "Fixtures")
                );

            _templates = templateList.ToDictionary(t => t.Name);
        }

        [HttpPost]
        public IActionResult CreateProject(CreateProjectType data)
        {
            try
            {
                var toSave = (Project)data;
                toSave.Fixtures = new List<Fixture>();

                var newEntity = _context.Projects.Add(toSave).Entity;
                _context.SaveChanges();

                return Ok(ConvertToOutputProjectType(newEntity, _templates));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("fixture")]
        public IActionResult AddFixture(AddFixtureType data)
        {
            var relevantProject = _context.Projects
                .Include(p => p.Fixtures)
                .FirstOrDefault(obj => obj.Id == data.ProjectId);
            if (relevantProject == null)
                return NotFound("Project not found");

            relevantProject.Fixtures.Add((Fixture)data);
            relevantProject.Updated = DateTime.Now;
            _context.SaveChanges();

            return Ok(ConvertToOutputProjectType(relevantProject, _templates));
        }

        [HttpGet]
        public IActionResult ListProjects()
        {
            try
            {
                var entities = _context.Projects
                    .Include(p => p.Fixtures)
                    .ToList();
                return Ok(entities.Select(obj => ConvertToOutputProjectType(obj, _templates)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public static OutputProjectType ConvertToOutputProjectType(Project obj, Dictionary<string, FixtureTemplate> fixtures)
        {
            var toReturn = new OutputProjectType
            {
                Id = obj.Id,
                Title = obj.Title,
                Description = obj.Description,
                Author = obj.Author,
                Created = obj.Created,
                Updated = obj.Updated,
                Fixtures = obj.Fixtures.Select(f => FixtureTemplateLoader.ConvertFixture(fixtures, f)).ToList()
            };

            return toReturn;
        }
    }
}
