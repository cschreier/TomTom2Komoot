using System;
using System.Text.Json.Serialization;

namespace TomTom2Komoot.Models
{
    public class TomTom
    {
        [JsonPropertyName("workouts")]
        public Workout[] Workouts { get; set; }
    }

    public class Workout
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("activity_type_id")]
        public int ActivityTypeId { get; set; }
        
        [JsonPropertyName("labels")]
        public Labels Labels { get; set; }
    }

    public class Labels
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
