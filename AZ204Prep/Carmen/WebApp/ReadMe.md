# Run app on local

1. DB Prerequisites:		
   Ensure you have installed local: 
	- SSMS - to connect to the  database
	- SQL Server - if you whant to run the  application with local DB
2. Secure configurations by using user secrets on local development:
	- In order to secure the configuration for an app, local user secrets can be used.
	- How to use user secrets from Visual Studio
	```	
	-> Right click on project -> Manage User Secrets
	
	**Investigate the changes:**
	
	By enabling Manage user secrets, it will do next changes
	
	- generate a secrets.json file under current user ~/AppData/Roaming/Microsoft/UserSecrets/<local_generated_id>
	- package Microsoft.Extensions.Configuration.UserSecrets will be added to the project Dependencies/Packages
	- <UserSecretsId>local_generated_id</UserSecretsId> is added to csproj file
	```
3. Add sensitive configuration to secrets.json on your local
			
	Add the next config to your secret.json file
->  For local db:
	```	
	{
		"ConnectionStrings": {
			"DbConnectionString": "Server=.\\SQLEXPRESS;Initial Catalog=replace-with-a-db-name-at-your-choice;Trusted_Connection=True;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
		}
	}
	```
	-> For Azure db

	```	
	{
		"ConnectionStrings": {
			"DbConnectionString": "Replace with the connecion string from azure db"
		}
	}
	```
-------------------------------------------------------------------

4. Build the application and run

		