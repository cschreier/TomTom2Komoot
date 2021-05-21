using System;
using System.Text.Json;
using RestSharp;
using TomTom2Komoot.Models;

namespace TomTom2Komoot.Services
{
    public class KomootService
    {
        private RestClient _authClient;
        private RestClient _apiClient;
        private string _username;
        private string _password;
        private bool _isLoginSuccessful;


        public KomootService(Komoot komootSettings)
        {
            _authClient = new RestClient("https://account.komoot.com");
            _authClient.CookieContainer = new();

            _apiClient = new RestClient("https://www.komoot.de/api");
            _apiClient.CookieContainer = _authClient.CookieContainer;

            _username = komootSettings.Username;
            _password = komootSettings.Password;

            _isLoginSuccessful = false;
        }

        public void Login()
        {
            try
            {
                RestRequest firstLoginRequest = new("v1/signin");
                firstLoginRequest.AddJsonBody(new { email = _username, password = _password });
                IRestResponse firstLoginResponse = _authClient.Post(firstLoginRequest);

                RestRequest secondLoginRequest = new("actions/transfer?type=signin");
                _authClient.Get(secondLoginRequest);
                _isLoginSuccessful = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login error with Komoot. {ex.Message}");
            }
        }

        public void Import(byte[] data, string workoutName)
        {
            if (!_isLoginSuccessful) throw new UnauthorizedAccessException("Not authorized on Komoot");

            RestRequest importRequest = new("routing/import/files/?data_type=fit");
            importRequest.AddHeader("Content-Type", "application/octet-stream");
            importRequest.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
            IRestResponse importResponse = _apiClient.Post(importRequest);

            KomootImport komootImport = JsonSerializer.Deserialize<KomootImport>(importResponse.Content);
            KomootUpload komootUpload = new(komootImport);
            komootUpload.Name = workoutName ?? "Import";
            string serializedUploadModel = JsonSerializer.Serialize(komootUpload);

            RestRequest uploadRequest = new("v007/tours/?hl=de");
            uploadRequest.AddHeader("Accept", "application/hal+json,application/json");
            uploadRequest.AddParameter("application/hal+json", serializedUploadModel, ParameterType.RequestBody);
            uploadRequest.AddHeader("Content-Type", "application/hal+json");
            IRestResponse uploadResponse = _apiClient.Post(uploadRequest);
        }
    }
}
