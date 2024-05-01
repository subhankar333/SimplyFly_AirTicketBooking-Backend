using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RouteRepository : IRepository<int, Models.Route>
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<RouteRepository> _logger;

        public RouteRepository(SimplyFlyDbContext context, ILogger<RouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Models.Route> Add(Models.Route item)
        {
            _context.Add(item);
            _context.SaveChanges();

            _logger.LogInformation("Route added with RouteId " + item.RouteId);
            return item;
        }

        public async Task<Models.Route> Delete(int routeId)
        {
            var _route = await GetAsync(routeId);
            if(_route != null)
            {
                _context.Remove(_route);
                _context.SaveChanges();
                _logger.LogInformation("Route deleted with RouteId " + routeId);
                return _route;
            }

            throw new NoSuchRouteException();
        }

        public async Task<Models.Route> GetAsync(int routeId)
        {
            var routes = await GetAsync();
            var _route = routes.FirstOrDefault(r => r.RouteId == routeId);
            if (_route != null)
            {
                return _route;
            }

            throw new NoSuchRouteException();
        }

        public async Task<List<Models.Route>> GetAsync()
        {
            var routes = _context.Routes.Include(e => e.SourceAirport).Include(d => d.DestinationAirport).ToList();
            return routes;
        }

        public async Task<Models.Route> Update(Models.Route item)
        {
            var _route = await GetAsync(item.RouteId);
            if( _route != null )
            {
                _context.Entry<Models.Route>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Route updated with routeId" + item.RouteId);
                return _route;
            }

            throw new NoSuchRouteException();
        }
    }
}
