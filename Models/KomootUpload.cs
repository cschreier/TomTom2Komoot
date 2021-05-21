using System;
using System.Text.Json.Serialization;

namespace TomTom2Komoot.Models
{
    public class KomootUpload
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonPropertyName("sport")]
        public string Sport { get; set; }
        [JsonPropertyName("start_point")]
        public Point StartPoint { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("_embedded")]
        public EmbeddedCoordinates EmbeddedCoordinates { get; set; }

        public KomootUpload(KomootImport importModel)
        {
            Status = "private";
            Type = "tour_recorded";
            Name = "Import";
            Sport = importModel.EmbeddedItems.Items[0].Sport;
            Source = importModel.EmbeddedItems.Items[0].Source;
            Date = importModel.EmbeddedItems.Items[0].Date;
            StartPoint = new(importModel.EmbeddedItems.Items[0].EmbeddedCoordinates.Coordinates.Items[0]);
            EmbeddedCoordinates = importModel.EmbeddedItems.Items[0].EmbeddedCoordinates;
        }
    }

    public class Point
    {
        [JsonPropertyName("alt")]
        public double Alt { get; set; }
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        public Point(CoordinatesItem importCoordinate)
        {
            Lat = importCoordinate.Lat;
            Lng = importCoordinate.Lng;
            Alt = importCoordinate.Alt;
        }
    }
}
