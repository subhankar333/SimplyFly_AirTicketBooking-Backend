﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimplyFly_Project.Context;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    [DbContext(typeof(SimplyFlyDbContext))]
    [Migration("20240411153550_SD_update")]
    partial class SD_update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SimplyFly_Project.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AdminId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Airport", b =>
                {
                    b.Property<int>("AirportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AirportId"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AirportId");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"), 1L, 1);

                    b.Property<string>("BookingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BookingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.HasKey("BookingId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.CancelledBooking", b =>
                {
                    b.Property<int>("CancelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CancelId"), 1L, 1);

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("RefundStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CancelId");

                    b.HasIndex("BookingId");

                    b.ToTable("CancelledBookings");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CustomerId");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Flight", b =>
                {
                    b.Property<string>("FlightNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Airline")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("BasePrice")
                        .HasColumnType("float");

                    b.Property<int>("FlightOwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.HasKey("FlightNumber");

                    b.HasIndex("FlightOwnerId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.FlightOwner", b =>
                {
                    b.Property<int>("FlightOwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightOwnerId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FlightOwnerId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("FlightOwners");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Passenger", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerId"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassportNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PassengerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.PassengerBooking", b =>
                {
                    b.Property<int>("PassengerBookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerBookingId"), 1L, 1);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int");

                    b.Property<int?>("PassengerId")
                        .HasColumnType("int");

                    b.Property<string>("SeatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PassengerBookingId");

                    b.HasIndex("BookingId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("SeatNumber");

                    b.ToTable("PassengersBookings");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentDetailsId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.HasIndex("PaymentDetailsId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.PaymentDetails", b =>
                {
                    b.Property<int>("PaymentDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentDetailsId"), 1L, 1);

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpiryDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentDetailsId");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteId"), 1L, 1);

                    b.Property<int>("DestinationAirportId")
                        .HasColumnType("int");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<int>("SourceAirportId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("RouteId");

                    b.HasIndex("DestinationAirportId");

                    b.HasIndex("SourceAirportId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"), 1L, 1);

                    b.Property<DateTime>("Arrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Departure")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("ScheduleId");

                    b.HasIndex("FlightNumber");

                    b.HasIndex("RouteId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.SeatDetail", b =>
                {
                    b.Property<string>("SeatNo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SeatClass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SeatNo");

                    b.ToTable("SeatDetails");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Admin", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("SimplyFly_Project.Models.Admin", "Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Booking", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimplyFly_Project.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.CancelledBooking", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Customer", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("SimplyFly_Project.Models.Customer", "Username");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Flight", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.FlightOwner", "FlightOwner")
                        .WithMany("OwnedFlights")
                        .HasForeignKey("FlightOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FlightOwner");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.FlightOwner", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.User", "User")
                        .WithOne("FlightOwner")
                        .HasForeignKey("SimplyFly_Project.Models.FlightOwner", "Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Passenger", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.PassengerBooking", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.HasOne("SimplyFly_Project.Models.Passenger", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId");

                    b.HasOne("SimplyFly_Project.Models.SeatDetail", "SeatDetail")
                        .WithMany()
                        .HasForeignKey("SeatNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Passenger");

                    b.Navigation("SeatDetail");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Payment", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.PaymentDetails", "PaymentDetails")
                        .WithMany()
                        .HasForeignKey("PaymentDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentDetails");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Route", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Airport", "DestinationAirport")
                        .WithMany()
                        .HasForeignKey("DestinationAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SimplyFly_Project.Models.Airport", "SourceAirport")
                        .WithMany()
                        .HasForeignKey("SourceAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationAirport");

                    b.Navigation("SourceAirport");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Schedule", b =>
                {
                    b.HasOne("SimplyFly_Project.Models.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimplyFly_Project.Models.Route", "Route")
                        .WithMany("Schedules")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.FlightOwner", b =>
                {
                    b.Navigation("OwnedFlights");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.Route", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("SimplyFly_Project.Models.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Customer");

                    b.Navigation("FlightOwner");
                });
#pragma warning restore 612, 618
        }
    }
}
