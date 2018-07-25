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
    public class PointsController : Controller
    {
        
        public PointsController(IDBOperation dbOps)
        {
            DBRecords = dbOps;
        }
        public IDBOperation DBRecords { get; set; }

        
        [Route("GetPoints")]
        public List<Points> GetPoints()
        {
            return DBRecords.GetPoints();
        }
        [HttpPost]
        [Route("AddPoints/{PointName}")]
        public Points AddPoints(string PointName)
        {
            return DBRecords.AddPoints(PointName);
        }


        [HttpPut]
        [Route("UpdatePoint")]
        public Points UpdatePoint([FromBody]Points Point)
        {
            return DBRecords.UpdatePoint(Point);
        }

        [HttpDelete]
        [Route("DeletePoint/{id}")]
        public void DeletePoint(int id)
        {
            DBRecords.DeletePoint(id);
        }
    }
}
