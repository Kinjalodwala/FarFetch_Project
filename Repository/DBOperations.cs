using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FarFetch.Configuration;
using FarFetch.Model;
using Microsoft.Extensions.Options;

namespace FarFetch.Repository
{
    public class DBOperations : IDBOperation
    {
        private readonly ConnectionStrings connectionStrings;


        public DBOperations(IOptions<ConnectionStrings> options)
        {
            connectionStrings = options.Value;
        }

        Points IDBOperation.AddPoints(string PointName)
        {
            Points Point = new Points();
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("AddPoints", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Point", PointName));
                SqlParameter output = new SqlParameter("outID", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                con.Open();
                cmd.ExecuteNonQuery();

                Point.PointID = (int)output.Value;
                Point.Point = PointName;

            }

            return Point;
        }

        Points IDBOperation.UpdatePoint(Points Point)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("UpdatePoint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Point", Point.Point));
                cmd.Parameters.Add(new SqlParameter("PointID", Point.PointID));
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Point;
        }

        void IDBOperation.DeletePoint(int PointID)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("DeletePoint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("PointID", PointID));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        List<Points> IDBOperation.GetPoints()
        {
            List<Points> PointList = GetAllPoints();
            return PointList;
        }

        public List<Points> GetAllPoints()
        {
            List<Points> PointList = new List<Points>();
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("GetAllPoints", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Points Point = new Points();

                    Point.PointID = Convert.ToInt32(rdr["PointID"]);
                    Point.Point = rdr["Point"].ToString();

                    PointList.Add(Point);
                }
                con.Close();
            }
            return PointList;
        }

        // Routes operations

        List<Routes> IDBOperation.GetRoutes()
        {
            List<Routes> RouteList = GetAllRoutes();
            return RouteList;
        }

        public List<Routes> GetAllRoutes()
        {
            List<Routes> RouteList = new List<Routes>();
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("GetAllRoutes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Routes Route = new Routes();

                    Route.RouteID = Convert.ToInt32(rdr["RouteID"]);
                    Route.PointFrom = rdr["PointFrom"].ToString();
                    Route.PointTo = rdr["PointTo"].ToString();
                    Route.Time = Convert.ToInt32(rdr["Time"]);
                    Route.Cost = Convert.ToInt32(rdr["Cost"]);
                    RouteList.Add(Route);
                }
                con.Close();
            }
            return RouteList;
        }

        Routes IDBOperation.AddRoute(Routes Route)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("AddRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("PointFrom", Route.PointFrom));
                cmd.Parameters.Add(new SqlParameter("PointTo", Route.PointTo));
                cmd.Parameters.Add(new SqlParameter("Time", Route.Time));
                cmd.Parameters.Add(new SqlParameter("Cost", Route.Cost));

                SqlParameter output = new SqlParameter("outID", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                con.Open();
                cmd.ExecuteNonQuery();

                Route.RouteID = (int)output.Value;

            }

            return Route;
        }

        Routes IDBOperation.UpdateRoute(Routes Route)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("UpdateRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("RouteID", Route.RouteID));
                cmd.Parameters.Add(new SqlParameter("PointFrom", Route.PointFrom));
                cmd.Parameters.Add(new SqlParameter("PointTo", Route.PointTo));
                cmd.Parameters.Add(new SqlParameter("Time", Route.Time));
                cmd.Parameters.Add(new SqlParameter("Cost", Route.Cost));

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Route;
        }

        void IDBOperation.DeleteRoute(int RouteID)
        {
            using (SqlConnection con = new SqlConnection(connectionStrings.DbConnection))
            {
                SqlCommand cmd = new SqlCommand("DeleteRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("RouteID", RouteID));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



        public List<AllPossibleRoute> GetBestRoutes(string From, string To)
        {
            List<Routes> RouteList = GetAllRoutes();

            string Origin = From;

            List<Routes> MaxRouteList = new List<Routes>();

            foreach (Routes mr in RouteList)
            {
                if (mr.PointFrom == From)
                {
                    MaxRouteList.Add(mr);
                }
            }

            int count = 0;
            List<AllPossibleRoute> AllPossibleRoutesList = new List<AllPossibleRoute>();
            do
            {
                var ReturnedTuples = GetNumberOfRoutes(RouteList, AllPossibleRoutesList, From, To);
                count++;
                AllPossibleRoutesList = ReturnedTuples.Item1;
                RouteList = ReturnedTuples.Item2;

            } while (count < MaxRouteList.Count);
            return AllPossibleRoutesList;

        }

        (List<AllPossibleRoute>, List<Routes>) GetNumberOfRoutes(List<Routes> RouteList, List<AllPossibleRoute> AllPossibleRoutesList, string From, string To)
        {

            string Origin = From;
            List<Routes> AvailableRoutes = new List<Routes>();
            List<Routes> NewRouteList = new List<Routes>();
            List<Routes> MaxRouteList = new List<Routes>();

            foreach (Routes r in RouteList)
            {
                NewRouteList.Add(r);
                if (r.PointFrom == Origin)
                {
                    AvailableRoutes.Add(r);
                    if (Origin == From)
                    {
                        NewRouteList.Remove(r);
                    }

                    if (r.PointTo == To)
                    {
                        AllPossibleRoute possibleRoute = new AllPossibleRoute();
                        if (possibleRoute.RouteNames == null)
                            possibleRoute.RouteNames = From;
                        foreach (Routes ar in AvailableRoutes)
                        {
                            possibleRoute.RouteNames += "-" + ar.PointTo;
                            possibleRoute.TotalTime += ar.Time;
                            possibleRoute.TotalCost += ar.Cost;
                        }
                        AllPossibleRoutesList.Add(possibleRoute);
                        Origin = From;
                        AvailableRoutes = new List<Routes>();
                    }
                    else
                    {
                        Origin = r.PointTo;
                    }

                }
            }
            return (AllPossibleRoutesList, NewRouteList);
        }

    }
}
