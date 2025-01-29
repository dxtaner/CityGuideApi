CityGuideApi
============

CityGuideApi is a .NET Core-based city guide API. This API includes features such as JWT authentication, SQL Server database connection, and Cloudinary integration.

Getting Started
---------------

Follow these steps to run this project locally.

### Requirements

*   .NET 5 or later
*   SQL Server
*   Visual Studio or Visual Studio Code
*   Postman or similar API client

### Installation

1.  **Clone the Project**

    git clone https://github.com/dxtaner/CityGuideApi
    cd CityGuideApi

3.  **Install Dependencies**

    dotnet restore

5.  **Configure Database Connection**

Update the \`DefaultConnection\` value in the \`appsettings.json\` file to match your SQL Server connection string.

7.  **Update Database**

    dotnet ef database update

9.  **Run the Project**

    dotnet run

Technologies Used
-----------------

*   ASP.NET Core
*   Entity Framework Core
*   JWT Authentication
*   AutoMapper
*   Cloudinary
*   Swagger UI

API Documentation
-----------------

You can test the API using Swagger UI. After running in the development environment, you can access it at:

    https://localhost:5001/swagger/index.html

Security
--------

This API uses JWT-based authentication. You must provide a valid JWT token to access protected endpoints.

Contributing
------------

To contribute, please fork the repository, make your improvements, and submit a pull request (PR).

License
-------

This project is licensed under the MIT license.
