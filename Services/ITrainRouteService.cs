using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
namespace BMCIT.Services
{
    public interface ITrainRouteService
    {
        //Routes
        IEnumerable<Routes> GetAllRoutes { get; }
        Response AddRoute(Routes routeData);
        Response GetRouteById(string Id);
        Response UpdateRoute(Routes routeData);
        Response DeleteOneRouteById(string Id);
    }
}