using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using RestSharp;
using TomTom2Komoot.Models;

namespace TomTom2Komoot.Services
{
    public class TomTomService
    {
        private RestClient _client;
        private string _username;
        private string _password;
        private long _lastSyncedWorkoutId;
        private List<WorkoutType> _allWorkoutTypes;
        private bool _isLoginSuccessful;

        public TomTomService(TomTom tomtomSettings)
        {
            _client = new RestClient("https://mysports.tomtom.com/service/webapi/v2");
            _client.CookieContainer = new();

            _username = tomtomSettings.Username;
            _password = tomtomSettings.Password;
            _lastSyncedWorkoutId = tomtomSettings.LastSyncedWorkoutId;
            _allWorkoutTypes = tomtomSettings.WorkoutTypes;
            _isLoginSuccessful = false;
        }

        public void Login()
        {
            try
            {
                RestRequest loginRequest = new RestRequest("auth/user/login");
                loginRequest.AddJsonBody(new { email = _username, password = _password });
                IRestResponse response = _client.Post(loginRequest);
                _isLoginSuccessful = response.IsSuccessful;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login error with TomTom. {ex.Message}");
            }
        }

        public IEnumerable<Workout> GetWorkouts()
        {
            if (!_isLoginSuccessful)
                throw new UnauthorizedAccessException("Not authorized on TomTom");

            RestRequest allActivitiesRequest = new RestRequest("activity");
            IRestResponse response = _client.Get(allActivitiesRequest);
            IEnumerable<Workout> workouts = JsonSerializer.Deserialize<Models.TomTom>(response.Content)?.Workouts;

            if (workouts == null)
                throw new NullReferenceException($"Could not deserialize workouts from TomTom. {response.Content}");

            workouts = workouts.Where(c => 
                c.Id > _lastSyncedWorkoutId 
                && _allWorkoutTypes.Where(c => c.IsActive).Any(t => t.TypeId == c.ActivityTypeId)
            );

            return workouts.OrderBy(c => c.Id);
        }

        public byte[] DownloadActivityData(long activityId)
        {
            RestRequest downloadRequest = new RestRequest($"activity/{activityId}?dv=1.5&format=fit");
            return _client.DownloadData(downloadRequest);
        }
    }
}
