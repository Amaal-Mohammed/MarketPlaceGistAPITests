using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
namespace MarketPlaceGistAPITests
{
    [TestClass]
    public class MarketPlaceGistAPITests
    {
        public String id;
       public String commentid;

        string fileName = "getid.txt";
        string fileCommentName = "getcommentid.txt";


        [TestMethod]
        public void addGist()
        {
            var client = new RestClient("https://api.github.com/gists");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Authorization", "Bearer 8f42e5dd0247a069941604a0bd01052aa16d5266");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{ \"description\": \"Hello World Examples tests\",\"files\": { \"hello_world.rb\": {\r\n      \"filename\": \"hello_world.rb\",\r\n      \"type\": \"application/x-ruby\",\r\n      \"language\": \"Ruby\",\r\n      \"raw_url\": \"https://gist.githubusercontent.com/octocat/6cad326836d38bd3a7ae/raw/db9c55113504e46fa076e7df3a04ce592e2e86d8/hello_world.rb\",\r\n      \"size\": 167,\r\n      \"truncated\": false,\r\n      \"content\": \"class HelloWorld\\n   def initialize(name)\\n      @name = name.capitalize\\n   end\\n   def sayHi\\n      puts \\\"Hello !\\\"\\n   end\\nend\\n\\nhello = HelloWorld.new(\\\"World\\\")\\nhello.sayHi\"\r\n    }}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var jObject = JObject.Parse(response.Content);
            id = jObject.GetValue("id").ToString();
            File.WriteAllText(fileName, id);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Assert.AreEqual(201, numericStatusCode); ;
        }

        [TestMethod]
        public void updateGist()
        {
            String id = File.ReadAllText(fileName);
            var client = new RestClient("https://api.github.com/gists/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Authorization", "Bearer 8f42e5dd0247a069941604a0bd01052aa16d5266");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{ \"description\": \"Hello World Examples testsggg\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Assert.AreEqual(200, numericStatusCode); ;
        }


        [TestMethod]
        public void addMyComment()
        {
            String id = File.ReadAllText(fileName);
            string url = "https://api.github.com/gists/" + id + "/" + "comments";
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Authorization", "Bearer 8f42e5dd0247a069941604a0bd01052aa16d5266");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"body\":\"my comment\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
     
            var jObject = JObject.Parse(response.Content);
            commentid = jObject.GetValue("id").ToString();
            File.WriteAllText(fileCommentName, commentid);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Assert.AreEqual(201, numericStatusCode); ;

        }

        [TestMethod]
        public void deleteMyComment()
        {
            String id = File.ReadAllText(fileName);
            String mycommentid= File.ReadAllText(fileCommentName);
            string url = "https://api.github.com/gists/" + id + "/" + "comments"+"/"+mycommentid;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Authorization", "Bearer 8f42e5dd0247a069941604a0bd01052aa16d5266");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Assert.AreEqual(204, numericStatusCode);

        }
    }
}
