using System;
using Mtg.Models;
using Mtg.Models.DTO;
using RestSharp;

namespace Mtg.Web.Utils
{
    public static class Network
    {
        private const string BaseUrl = "http://localhost:59467/api/";

        public static IRestResponse<Response<T>> MakePostRequest<T>(string path, GameRequest gameRequest) where T: new()
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(path, Method.POST) {RequestFormat = DataFormat.Json};
            gameRequest.RequestTime = DateTime.Now.ToUniversalTime();
            gameRequest.Token = Cryptography.GetSha1Hash(gameRequest);
            request.AddBody(gameRequest);
            return client.Execute<Response<T>>(request);
        }
    }
}