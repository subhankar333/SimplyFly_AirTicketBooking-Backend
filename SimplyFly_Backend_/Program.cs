using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimplyFly_Project.Context;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;
using SimplyFly_Project.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimplyFly_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });



                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });



            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true; // Write indented JSON for better readability
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                          ValidateIssuer = false,
                          ValidateAudience = false
                      };
                  });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });


            //context 
            builder.Services.AddDbContext<SimplyFlyDbContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings:myconnection"]));

            #region Repostiroy injection

            builder.Services.AddScoped<IRepository<string, User>, UserRepository>(); 
            builder.Services.AddScoped<IRepository<int, Customer>, CustomerRepository>(); 
            builder.Services.AddScoped<IRepository<int, Admin>, AdminRepository>(); 
            builder.Services.AddScoped<IRepository<int, FlightOwner>, FlightOwnerRepository>();
            builder.Services.AddScoped<IRepository<int,Booking>, BookingRepository>();
            builder.Services.AddScoped<IRepository<string, Flight>, FlightRepository>();
            builder.Services.AddScoped<IRepository<int, PassengerBooking>, PassengerBookingRepository>();
            builder.Services.AddScoped<IRepository<int, Schedule>, ScheduleRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<ISeatDetailRepository, SeatDetailRepository>();
            builder.Services.AddScoped<IPassengerBookingRepository, PassengerBookingRepository>();
            builder.Services.AddScoped<IBookingRepository,BookingRepository>();
            builder.Services.AddScoped<IRepository<int,Passenger>, PassengerRepository>();   
            builder.Services.AddScoped<IRepository<int,Models.Route>, RouteRepository>();
            builder.Services.AddScoped<IRepository<int,Airport>, AirportRepository>();
            builder.Services.AddScoped<IRepository<string,SeatDetail>, SeatDetailRepository>();
            builder.Services.AddScoped<IRepository<int,CancelledBooking>, CancelBookingRepository>();

            #endregion


            #region Service injection 

            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFlightOwnerService, FlightOwnerService>();
            builder.Services.AddScoped<IBookingService,BookingService>();
            builder.Services.AddScoped<IFlightFlightOwnerService,FlightService>();
            builder.Services.AddScoped<IFlightCustomerService, ScheduleService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPassengerService, PassengerService>();
            builder.Services.AddScoped<IRouteFlightOwnerService,RouteService>();
            builder.Services.AddScoped<IScheduleFlightOwnerService,ScheduleService>();
            builder.Services.AddScoped<ISeatDetailService,SeatDetailService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            


            app.MapControllers();

            app.Run();
        }
    }
}
