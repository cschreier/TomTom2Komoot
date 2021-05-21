using System;
using System.Text.Json.Serialization;

namespace TomTom2Komoot.Models
{
    public class KomootImport
    {
        [JsonPropertyName("_embedded")]
        public EmbeddedItems EmbeddedItems { get; set; }
    }

    public class EmbeddedItems {
        [JsonPropertyName("items")]
        public Item[] Items { get; set; }
    }

    public class Item {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonPropertyName("sport")]
        public string Sport { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("_embedded")]
        public EmbeddedCoordinates EmbeddedCoordinates { get; set; }

    }
    public class EmbeddedCoordinates{
        [JsonPropertyName("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
    public class Coordinates {
        [JsonPropertyName("items")]
        public CoordinatesItem[] Items { get; set; }
    }

    public class CoordinatesItem {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        [JsonPropertyName("lng")]
        public double Lng { get; set; }
        [JsonPropertyName("alt")]
        public double Alt { get; set; }
        [JsonPropertyName("t")]
        public long Time { get; set; }
    }
}
