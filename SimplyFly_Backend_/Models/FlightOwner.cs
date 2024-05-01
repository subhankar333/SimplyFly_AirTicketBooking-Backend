using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class FlightOwner:IEquatable<FlightOwner>
    {
        [Key]
        public int FlightOwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
        public string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
        public List<Flight>? OwnedFlights { get; set; }

        public FlightOwner()
        {
            FlightOwnerId = 0;
        }
        public FlightOwner(int flighOwnerId, string name, string email, List<Flight> ownedFlights, string password)
        {
            FlightOwnerId = flighOwnerId;
            Name = name;
            Email = email;
            OwnedFlights = ownedFlights;

        }
        public FlightOwner(string name, string email, List<Flight> ownedFlights, string password)
        {
            Name = name;
            Email = email;
            OwnedFlights = ownedFlights;

        }


        public bool Equals(FlightOwner? other)
        {
            var Owner = other ?? new FlightOwner();
            return FlightOwnerId.Equals(Owner.FlightOwnerId);
        }
    }
}
