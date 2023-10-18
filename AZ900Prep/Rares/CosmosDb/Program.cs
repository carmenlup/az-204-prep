using CosmosDb;

var dbManager = new DbManager();
//await dbManager.CreateDb();


await dbManager.CreateContainer("Orders", "/category");

await dbManager.AddItem("O1", "Laptop", 100);
//await dbManager.AddItem("O2", "PC", 100);
//await dbManager.AddItem("O3", "Mobile", 100); 
//await dbManager.AddItem("O4", "Fax", 100);

await dbManager.ReplaceItem();
await dbManager.ReadItem();
await dbManager.DeleteItem();
