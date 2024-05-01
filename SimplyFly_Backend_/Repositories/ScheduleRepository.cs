using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ScheduleRepository : IRepository<int, Schedule>
    {

        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<ScheduleRepository> _logger;

        public ScheduleRepository(SimplyFlyDbContext context, ILogger<ScheduleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Schedule> Add(Schedule item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Schedule added with ScheduleId " + item.ScheduleId);
            return item;
        }

        public async Task<Schedule> Delete(int scheduleId)
        {
            var schedule = await GetAsync(scheduleId);
            if(schedule != null)
            {
                _context.Remove(schedule);
                _context.SaveChanges();
                _logger.LogInformation("Schedule deleted with ScheduleId " +  scheduleId);
                return schedule;
            }

            throw new NoSuchScheduleException();
        }
        public async Task<Schedule> GetAsync(int scheduleId)
        {
            var schedules = await GetAsync();
            var schedule = schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);

            if( schedule != null )
            {
                return schedule;
            }

            throw new NoSuchScheduleException();

        }

        public async Task<List<Schedule>> GetAsync()
        {
            //AsNoTracking() is used to indicate that the retrieved entities should not be tracked by the DbContext for changes
            var schedules = await _context.Schedules.AsNoTracking()
                             .Include(s => s.Route)
                             .Include(s => s.Flight)
                             .Include(s => s.Route.SourceAirport)
                             .Include(s => s.Route.DestinationAirport)
                             .ToListAsync();

            return schedules;
        }

        public async Task<Schedule> Update(Schedule item)
        {
            var schedule = await GetAsync(item.ScheduleId);
            if(schedule != null)
            {
                _context.Entry<Schedule>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Schedule updated with scheduleId" + item.ScheduleId);
                return schedule;
            }

            throw new NoSuchScheduleException();
        }
    }
}
