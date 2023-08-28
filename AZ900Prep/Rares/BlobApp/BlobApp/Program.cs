using BlobApp;

//await new Blob().HandleBlobs();
var table = new TableStorage();
table.AddEntities();
table.UpdateEntity("Mobile", "O1", 69);
table.QueryEntity("Mobile");

table.GetEntityAndDeleteThem();
