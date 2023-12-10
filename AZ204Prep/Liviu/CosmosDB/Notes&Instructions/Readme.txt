1. Set the Sources/CosmosDB project as Startup project
2. Go to Azure portal and create a CosmosDB account. I'm using Serverless for Capacity Mode, otherwise with the default 1000 RU we cannot create 3 containers with the minimum 400 RU each (3X400 = 1200 which is greated than Account's limit).
3. Go to CosmosDB account -> Keys and copy the URI + primary Key from there and set the DbUtilities.connectionUri / connectionKey properties
4. Run the project