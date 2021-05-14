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
        private bool _isLoginSuccessful;

        public KomootService()
        {
            _authClient = new RestClient("https://account.komoot.com");
            _authClient.CookieContainer = new();

            _apiClient = new RestClient("https://www.komoot.de/api");
            _apiClient.CookieContainer = _authClient.CookieContainer;
            _isLoginSuccessful = false;
        }

        public void Login(string username, string password)
        {
            try
            {
                RestRequest firstLoginRequest = new("v1/signin");
                firstLoginRequest.AddJsonBody(new { email = username, password = password });
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

        public void Import(byte[] data)
        {
            if (!_isLoginSuccessful) throw new UnauthorizedAccessException("Not authorized on Komoot");

            RestRequest importRequest = new("routing/import/files/?data_type=fit");
            importRequest.AddHeader("Content-Type", "application/octet-stream");
            importRequest.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
            IRestResponse importResponse = _apiClient.Post(importRequest);

            KomootImportModel importModel = JsonSerializer.Deserialize<KomootImportModel>(importResponse.Content);
            KomootUploadModel uploadModel = new(importModel);
            string serializedUploadModel = JsonSerializer.Serialize(uploadModel);

            RestRequest uploadRequest = new("v007/tours/?hl=de");
            uploadRequest.AddHeader("Accept", "application/hal+json,application/json");
            uploadRequest.AddParameter("application/hal+json", serializedUploadModel, ParameterType.RequestBody);
            uploadRequest.AddHeader("Content-Type", "application/hal+json");
            IRestResponse uploadResponse = _apiClient.Post(uploadRequest);
        }
    }
}
