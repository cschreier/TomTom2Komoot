using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
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
                TomTomService tomtomService = new(settings.TomTom);

                komootService.Login();
                tomtomService.Login();

                IEnumerable<Workout> workouts = tomtomService.GetWorkouts();
                logger.Info($"{workouts.Count()} new workouts found");

                foreach (Workout workout in workouts)
                {
                    try
                    {
                        byte[] activityBytes = tomtomService.DownloadActivityData(workout.Id);
                        komootService.Import(activityBytes, workout.Labels?.Name);
                        settings.TomTom.LastSyncedWorkoutId = workout.Id;
                        logger.Info($"workout {workout.Id} synchronized");
                    }
                    catch (Exception ex)
                    {
                        settings.TomTom.LastSyncedWorkoutId = workout.Id;
                        throw new Exception($"Error while uploading workout {workout.Id} to Komoot. {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                settings.WriteLastSync();
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
