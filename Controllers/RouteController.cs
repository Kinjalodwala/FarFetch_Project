using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarFetch.Model;
using FarFetch.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarFetch.Controllers
{
    [Produces("application/json")]
    [Route("/api/")]
    public class RouteController : Controller
    {
        public RouteController(IDBOperation dbOps)
        {
            DBRecords = dbOps;
        }
        public IDBOperation DBRecords { get; set; }


        [Route("GetRoutes")]
        public IEnumerable<Routes> GetRoutes()
        {
            return DBRecords.GetRoutes();
        }
        
        [HttpPost]
        [Route("AddRoute")]
        public Routes AddRoute([FromBody]Routes Route)
        {
            return DBRecords.AddRoute(Route);
        }


        [HttpPut]
        [Route("UpdateRoute")]
        public Routes UpdateRoute([FromBody]Routes Route)
        {
            return DBRecords.UpdateRoute(Route);
        }

        [HttpDelete]
        [Route("DeleteRoute/{id}")]
        public void DeleteRoute(int id)
        {
            DBRecords.DeleteRoute(id);
        }

        [HttpGet]
        [Route("GetBestRoutes/{From}/{To}")]
        public IActionResult  GetBestRoutes(string From,string To)
        {
            string Message = string.Empty;
            string Message1 = string.Empty;
            string Message2 = string.Empty;
            AllPossibleRoute BestCheapestRoute = new AllPossibleRoute();
            AllPossibleRoute BestShortestRoute = new AllPossibleRoute();

            List<AllPossibleRoute> Result = DBRecords.GetBestRoutes(From, To);

            if (Result.Count == 0)
            {
                Message = "No route is available for the specified Destinations.";
                 var response = new { Message };
                return Ok(response);

            }
            else if(Result.Count==1)
            {
                Message = "Only 1 route is availbale";
                var response = new { Message,Result };
                return Ok(response);
            }
            else if (Result.Count >= 2)
            {
                Message1 = "Below is the cheapest route";
                int min = Result.Min(r => r.TotalCost);
                BestCheapestRoute = Result.First(r => r.TotalCost == min);

                Message2 = "Below is the quickest route";
                int min1 = Result.Min(r => r.TotalTime);
                BestShortestRoute = Result.First(r => r.TotalTime == min1);

                if (BestShortestRoute == BestCheapestRoute)
                {
                    Message = "Below are the possible Routes";
                    Message1 = "Below is the best shortest and cheapest route";
                    var response = new { Message1, BestCheapestRoute, Message, Result };
                    return Ok(response);
                }
                else
                {
                    Message = "Below are the possible Routes";
                    var response = new { Message1, BestCheapestRoute, Message2,BestShortestRoute, Message, Result };
                    return Ok(response);
                }

            }

            return Ok(Response);
           
        }
    }
}