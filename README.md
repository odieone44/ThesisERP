# Thesis ERP

The back-end of my thesis project for University of Pireaus Computer Science BSc.  

A simple web api that provides basic order/inventory management, built with .NET 6 and using Swagger-UI/OpenApi 3.0 to expose api documentation page. 

This project is still under development.  

Many parts of the application are not yet fully functional or do not adhere to known best practices and patterns.  
The goal is to study and transition this solution towards a Clean/Onion Architecture, following an Event-Driven Design. 

# Running the app locally using Visual Studio on Windows

To run the application locally, you need to have the .NET 6 SDK installed.  

1. After cloning/downloading this repo, edit the appsettings.json file with your SQL connection string, and change any other configuration values to your liking.  

2. Open a Package Manager Console on the ThesisERP.Infrastracture project and run the following commands to create/update the database:  
  i. `Add-Migration InitialMigration -OutputDir Data/Migrations`  
  ii. `Update-Database`

3. Open a powershell window and run `setx THESIS_JWT_KEY "[your key]" /M`.  
  This will set a system environment variable. Use a strong key for this variable as it is used for signing and authenticating JWTs. 

4. Set `ThesisERP.Api` as the startup project, and run the app from Visual Studio. 

Upon running, the application will automatically seed the database with an admin user based on the provided credentials in the appsettings file, and some test entities.  

# Tests

Not implemented yet.
