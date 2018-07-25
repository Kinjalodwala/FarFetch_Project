
Source code URL : https://github.com/Kinjalodwala/FarFetch_Project

Api URLs For testing in Postman Client.

For Points

1. http://localhost:64467/api/GetPoints
2. http://localhost:64467/api/AddPoints/DestnationA
3. http://localhost:64467/api/UpdatePoint
   Select Body / row / Json format / enter records to be updated
4. http://localhost:64467/api/DeletePoint/{id}
   {id}= put id of the records that you want to delete

For Routes

1. http://localhost:64467/api/GetRoutes
2. http://localhost:64467/api/AddRoute
   Select Body / row / Json format / enter records to be added
3. http://localhost:64467/api/UpdateRoute
   Select Body / row / Json format / enter records to be updated
4. http://localhost:64467/api/DeleteRoute/{id}
   {id}= put id of the records that you want to delete

To get best routes

http://localhost:64467/api/GetBestRoutes/DestinationA/DestinationB
http://localhost:64467/api/GetBestRoutes/DestinationA/DestinationC
http://localhost:64467/api/GetBestRoutes/DestinationA/DestinationI
http://localhost:64467/api/GetBestRoutes/DestinationF/DestinationB


Above link will show the possible cheapest, shortest and best route according to data.

The tools as below :

1. ASP.NET Core C#
2. MVC
3. Restful Web API
4. SQL Server (There is FarFetch.bak file given in project folder for Database backups.)





