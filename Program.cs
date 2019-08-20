using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace ConsoleApiCall
{
    class Program
    {
        static void Main(string[] args)
        {
            //1
            var client = new RestClient("https://api.nytimes.com/svc/topstories/v2");
            //2
            var request = new RestRequest("home.json?api-key=Av6bD6g74cKacVrQ2fuNGh8MQS9fAIQF", Method.GET);
            //3
            var response = new RestResponse();

            //4
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Article> articleList = JsonConvert.DeserializeObject<List<Article>>(jsonResponse["results"].ToString());
            foreach (Article article in articleList)
            {
                Console.WriteLine("Section: {0}", article.Section);
                Console.WriteLine("Title: {0}", article.Title);
                Console.WriteLine("Abstract: {0}", article.Abstract);
                Console.WriteLine("Url: {0}", article.Url);
                Console.WriteLine("Byline: {0}", article.Byline);
            }
            Console.ReadLine();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }

    public class Article
    {
        public string Section { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Url { get; set; }
        public string Byline { get; set; }
    }
}
