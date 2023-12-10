using StorageAccountApp;

// Uncomment to run

//var tableService = new TableService();
//await tableService.RunTableServiceCommandsAsync();

//var blobSerice = new BloblService();
//await blobSerice.RunBlobServiceCommandsAsync();

var grantAccessWithAppObj = new GrantAccessWithApplicationObjects();
await grantAccessWithAppObj.RunBlobCommandsUsingAppObjectsAuthentication();

