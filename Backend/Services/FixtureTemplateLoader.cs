using Backend.Types.Fixture;
using Infrastructure.Entities;
using System.Text.Json;

namespace Backend.Services
{
    public static class FixtureTemplateLoader
    {
        public static List<FixtureTemplate> LoadAll(string path)
        {
            var templates = new List<FixtureTemplate>();
            foreach (var file in Directory.GetFiles(path, "*.json"))
            {
                var json = File.ReadAllText(file);
                var template = JsonSerializer.Deserialize<FixtureTemplate>(json);
                if (template != null)
                    templates.Add(template);
            }
            return templates;
        }

        public static OutputFixtureType ConvertFixture(Dictionary<string, FixtureTemplate> fixtures, Fixture obj)
        {
            var fixtureTemplateName = obj.FixtureTemplate;

            if (fixtureTemplateName == null || !fixtures.TryGetValue(fixtureTemplateName, out FixtureTemplate template))
            {
                throw new Exception("Invalid fixture template.");
            }

            return new OutputFixtureType
            {
                Id = obj.Id,
                Name = obj.Name,
                StartAddress = obj.StartAddress,
                FixtureTemplate = template
            };
        }
    }
}
