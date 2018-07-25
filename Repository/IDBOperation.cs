using FarFetch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarFetch.Repository
{
    public interface IDBOperation
    {
        List<Points> GetPoints();
        Points AddPoints(string Point);
        Points UpdatePoint(Points Point);
        void DeletePoint(int PointID);

        List<Routes> GetRoutes();
        Routes AddRoute(Routes Route);
        Routes UpdateRoute(Routes Route);
        void DeleteRoute(int RouteID);
        List<AllPossibleRoute> GetBestRoutes(string From, string To);

    }
}
