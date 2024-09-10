# Introduction
This is a registry API project containing RESTful API that built with clean code architecture that is used for interacting registry data.

# Getting Started
Clone the repo.

# Build and Test
Open the solution in visual studio, build the project and run. It will navigate you to swagger page.

Alternatively, if you're not interested in debugging, you can just use the dotnet CLI from the project directory (where .csproj is).
`dotnet build`
`dotnet run`

# General Information

If you want to make use of the requests, you must configure your own `appsettings.Development.json` file under Web layer and set your connectiong string. The `ASPNETCORE_ENVIRONMENT` environment variable will determine which appsettings files are loaded during startup. For development, you can go to the project properties to change this value.
- Valid values are: Development, Production

Errors are currently be funneled `UnhandledExceptionFilter.cs`. In other words, if you are troubleshooting HTTP 500s, you'll want to put a breakpoint in the filter, or consult the logs.
We're using Serilog to log to log files located in the `Logs` folder in the project root. You'll find error information there, or in the console window.

# Authentication & Authorization
Registry API uses basic authentication. Except for the create user endpoint. It uses a base64 encoded API key passed in the `Authorization` header. It will compare this value to the value of the API key in the appropriate `appsettings.Environment.json` file.

Ex. `Authorization: bXlhcGlrZXk=`

POST https://registryapi.company.com/api/user HTTP/1.1
Content-Type: application/json
Authorization: bXlhcGlrZXk=
{
	"username": "[USERNAME]",
	"password": "[PASSWORD]",
	"customerid": [CUSTOMER ID]
}

All other endpoints will use basic auth, which is the username and password separated by a `:` base64 encoded and preceeded with the word `Basic` in the Authorization header.

Ex. `Authorization: Basic ZGlja2J1dHQ6ZGlja2J1dHRzcGFzc3dvcmQ=`

The `BasicAuthenticationHandler` will be the place to go for authentication troubleshooting.

GET https://registryapi.company.com/api/customer/supportcommunity HTTP/1.1
Content-Type: application/json
Authorization: Basic Zm9vdGVkckB5YWhvby5jb206aW50ZXJtc29m

# Docker
There's a Dockerfile. Not using it for now. It can be ignored.