using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSecurity
{
    public class AccessToken
    {
        public async Task RunCommands()
        {
            string resource = "http://storage.azure.com";
            // get the url from documentation https://learn.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/how-to-use-vm-token
            string metadataUri = "http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=" + resource;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Metadata", "true");
            
            HttpResponseMessage response = await client.GetAsync(metadataUri);
            var content = await response.Content.ReadAsStringAsync();
            // returns a json containing the access_token

            Console.WriteLine(content);


        }
    }
}
