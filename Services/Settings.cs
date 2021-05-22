using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TomTom2Komoot.Models;

namespace TomTom2Komoot.Services
{
    public class Settings
    {
        public TomTom TomTom { get; set; }

        public void WriteLastSync()
        {
            StreamWriter writer = new("./appsettings.json");
            string settings = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText("./appsettings.json", settings);
        }
    }

    public class TomTom
    {
        public List<WorkoutType> WorkoutTypes { get; set; }
        public long LastSyncedWorkoutId { get; set; }
    }

    public class WorkoutType
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
