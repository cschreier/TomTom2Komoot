using System;
using System.Linq;

namespace TomTom2Komoot.Services
{
    public class UserSettings
    {
        public string KomootUser { get; }
        public string KomootPassword { get; }
        public string TomTomUser { get; }
        public string TomTomPassword { get; }
        public string[] SyncWorkoutTypes { get; }

        public UserSettings(string[] args)
        {

            KomootUser = args.FirstOrDefault(c => c.Split('=').First() == "ku")?.Split('=')?.Last();
            KomootPassword = args.FirstOrDefault(c => c.Split('=').First() == "kp")?.Split('=')?.Last();
            TomTomUser = args.FirstOrDefault(c => c.Split('=').First() == "tu")?.Split('=')?.Last();
            TomTomPassword = args.FirstOrDefault(c => c.Split('=').First() == "tp")?.Split('=')?.Last();
            SyncWorkoutTypes = args.FirstOrDefault(c => c.Split('=').First() == "w")?.Split('=')?.Last()?.Split(',');

            if (string.IsNullOrWhiteSpace(KomootUser))
                KomootUser = Environment.GetEnvironmentVariable("KOMOOT_USER");
            if (string.IsNullOrWhiteSpace(KomootPassword))
                KomootPassword = Environment.GetEnvironmentVariable("KOMOOT_PASSWORD");
            if (string.IsNullOrWhiteSpace(TomTomUser))
                TomTomUser = Environment.GetEnvironmentVariable("TOMTOM_USER");
            if (string.IsNullOrWhiteSpace(TomTomPassword))
                TomTomPassword = Environment.GetEnvironmentVariable("TOMTOM_PASSWORD");
            if (SyncWorkoutTypes == null || SyncWorkoutTypes.Length == 0)
                SyncWorkoutTypes = Environment.GetEnvironmentVariable("SYNC_WORKOUT_TYPES")?.Split(',');
        }
    }
}
