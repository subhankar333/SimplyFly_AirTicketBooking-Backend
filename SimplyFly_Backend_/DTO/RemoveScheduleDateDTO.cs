using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class RemoveScheduleDateDTO
    {
        public DateTime DateOfSchedule { get; set; }
        public int AirportId { get; set; }
    }
}
