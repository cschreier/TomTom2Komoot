using System;
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
        private bool _isLoginSuccessful;

        public TomTomService()
        {
            _client = new RestClient("https://mysports.tomtom.com/service/webapi/v2");
            _client.CookieContainer = new();
            _isLoginSuccessful = false;
        }

        public void Login(string username, string password)
        {
            try
            {
                RestRequest loginRequest = new RestRequest("auth/user/login");
                loginRequest.AddJsonBody(new { email = username, password = password });
                IRestResponse response = _client.Post(loginRequest);
                _isLoginSuccessful = response.IsSuccessful;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login error with TomTom. {ex.Message}");
            }
        }

        public Workout[] GetWorkouts(TomTomWorkoutEnum? workoutFilter = null, DateTime? start = null)
        {
            if (!_isLoginSuccessful) throw new UnauthorizedAccessException("Not authorized on TomTom");

            RestRequest allActivitiesRequest = new RestRequest("activity?tracking=true&trackingv=1&includeWebGoals=false&limit=2147483647");
            IRestResponse response = _client.Get(allActivitiesRequest);
            Workout[] workouts = JsonSerializer.Deserialize<TomTomActivityModel>(response.Content).Workouts;

            if (workoutFilter != null)
                workouts = workouts.Where(c => c.ActivityTypeId == (int)workoutFilter).ToArray();
            if (start != null)
                workouts = workouts.Where(c => c.StartDateTime.ToUniversalTime() > start).ToArray();

            return workouts;
        }

        public byte[] DownloadActivityData(long activityId)
        {
            RestRequest downloadRequest = new RestRequest($"activity/{activityId}?dv=1.5&format=fit");
            return _client.DownloadData(downloadRequest);
        }
    }
}
