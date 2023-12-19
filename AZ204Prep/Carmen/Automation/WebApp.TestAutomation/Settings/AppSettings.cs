using Newtonsoft.Json;
using System.IO;

public class AppSettings
{
    public string ConnectionString { get; set; }

    public static AppSettings LoadSettings()
    {
        string json = File.ReadAllText("appsettings.json");
        return JsonConvert.DeserializeObject<AppSettings>(json);
    }
}
