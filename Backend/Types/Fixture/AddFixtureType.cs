using Infrastructure.Entities;

namespace Backend.Types.Fixture
{
    public class AddFixtureType
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int StartAddress { get; set; }
        public string FixtureTemplate { get; set; }
        public static explicit operator Infrastructure.Entities.Fixture(AddFixtureType data)
        {
            return new Infrastructure.Entities.Fixture
            {
                ProjectId = data.ProjectId,
                Name = data.Name,
                StartAddress = data.StartAddress,
                FixtureTemplate = data.FixtureTemplate
            };
        }
    }
}
