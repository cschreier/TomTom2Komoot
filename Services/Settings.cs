using System;
using System.IO;
using System.Text.Json;

namespace TomTom2Komoot.Services
{
    public class Settings
    {
        public Komoot Komoot { get; set; }
        public TomTom TomTom { get; set; }
        public DateTime LastSyncAt { get; set; }

        public void WriteLastSyncAt()
        {
            StreamWriter writer = new("./appsettings.json");
            string settings = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText("./appsettings.json", settings);
        }
    }

    public class Komoot
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TomTom
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
