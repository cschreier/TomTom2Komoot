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
            AppSettings appsettings = new();
            UserSettings userSettings = new(args);

            try
            {
                logger.Info("app started");
                appsettings = ReadAppSettingsFile();

                KomootService komootService = new(userSettings);
                TomTomService tomtomService = new(userSettings, appsettings.TomTom);

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
                        appsettings.TomTom.LastSyncedWorkoutId = workout.Id;
                        logger.Info($"workout {workout.Id} synchronized");
                    }
                    catch (Exception ex)
                    {
                        appsettings.TomTom.LastSyncedWorkoutId = workout.Id;
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
                appsettings.WriteLastSync();
            }
        }

        private static AppSettings ReadAppSettingsFile()
        {
            try
            {
                StreamReader reader = new StreamReader("./appsettings.json");
                string settings = reader.ReadToEnd();
                return JsonSerializer.Deserialize<AppSettings>(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while reading the settings file. {ex.Message}");
            }
        }
    }
}
