using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;
using Route = SimplyFly_Project.Models.Route;

namespace SimplyFly_Project.Context
{
    [ExcludeFromCodeCoverage]
    public class SimplyFlyDbContext : DbContext
    {
        public SimplyFlyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<FlightOwner> FlightOwners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<SeatDetail> SeatDetails { get; set; }
        public DbSet<PassengerBooking> PassengersBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CancelledBooking> CancelledBookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and constraints
            modelBuilder.Entity<Route>()
                .HasOne(r => r.SourceAirport)
                .WithMany()
                .HasForeignKey(r => r.SourceAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.DestinationAirport)
                .WithMany()
                .HasForeignKey(r => r.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add additional configurations here as needed

            base.OnModelCreating(modelBuilder);
        }

    }
}
