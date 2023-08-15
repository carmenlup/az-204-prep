using AzureSecurity;

//var encryptionKeys = new EncryptionKeys();
//await encryptionKeys.RunCommands();


var secrets = new SecretsService();
await secrets.RunCommands();
