using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using NLog;
using RestSharp;
using TomTom2Komoot.Models;
using TomTom2Komoot.Services;

namespace TomTom2Komoot
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Settings settings = new();
            try
            {
                logger.Info("app started");
                settings = ReadSettingsFile();

                KomootService komootService = new();
                TomTomService tomtomService = new();

                komootService.Login(settings.Komoot.Username, settings.Komoot.Password);
                tomtomService.Login(settings.TomTom.Username, settings.TomTom.Password);

                Workout[] cyclingWorkouts = tomtomService.GetWorkouts(TomTomWorkoutEnum.Cycling, settings.LastSyncAt.ToUniversalTime());
                Workout[] hikingWorkouts = tomtomService.GetWorkouts(TomTomWorkoutEnum.Hiking);

                Workout[] workouts = cyclingWorkouts.Concat(hikingWorkouts).ToArray();
                logger.Info($"{workouts.Length} new workouts found");

                foreach (Workout workout in workouts)
                {
                    try
                    {
                        byte[] activityBytes = tomtomService.DownloadActivityData(workout.Id);
                        komootService.Import(activityBytes);
                    }
                    catch (Exception ex)
                    {
                        settings.LastSyncAt = workout.StartDateTime.AddMinutes(-1).ToUniversalTime();
                        throw new Exception($"Error while uploading workout {workout.Id} to Komoot. {ex.Message}");
                    }
                }

                settings.LastSyncAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                settings.WriteLastSyncAt();
            }
        }

        private static Settings ReadSettingsFile()
        {
            try
            {
                StreamReader reader = new StreamReader("./appsettings.json");
                string settings = reader.ReadToEnd();
                return JsonSerializer.Deserialize<Settings>(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while reading the settings file. {ex.Message}");
            }
        }
    }
}
