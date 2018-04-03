using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SlackAPI;
using Newtonsoft.Json;
using RestSharp;
using WebApplication1.Models;
using System.Threading;
using System.IO;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCode()
        {
            Connection connection = Connection.Get();

            string queryCode = Request.QueryString.Get("code");
            string state = Request.QueryString.Get("state");
            if (!state.Equals(connection.ConnectionId))
            {
                return new EmptyResult();
            }

            RestClient client = new RestClient("https://slack.com/api/oauth.access");
            IRestRequest request = new RestRequest(Method.GET);
            request.AddQueryParameter("client_id", "18212299189.324327579253");
            request.AddQueryParameter("client_secret", "87fdadc663cd685230b5228ab79262cb");
            request.AddQueryParameter("code", queryCode);


            IRestResponse restResponse = client.Execute(request);
            SlackAuthorizeResponse response =
                JsonConvert.DeserializeObject<SlackAuthorizeResponse>(restResponse.Content);
            connection.Code = response.AccessToken;
            connection.Store();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SetConnection()
        {
            StreamReader reader = new StreamReader(Request.InputStream);
            string guid = reader.ReadToEnd().Replace("\"", "");
            Connection connection = Connection.Get();
            connection.ConnectionId = guid;
            connection.Code = "";
            connection.Store();
            return Json("");
        }

        [HttpPost]
        public ActionResult GetConnection()
        {
            StreamReader reader = new StreamReader(Request.InputStream);
            string guid = reader.ReadToEnd().Replace("\"", "");
            Connection connection = Connection.Get();
            if (connection.ConnectionId.Equals(guid))
            {
                string code = connection.Code;
                connection.Code = "";
                connection.Store();
                return Json(code);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public ActionResult PostVerification()
        {
            StreamReader reader = new StreamReader(Request.InputStream);
            string body = reader.ReadToEnd();
            VerificationEvent verify = JsonConvert.DeserializeObject<VerificationEvent>(body);
            return Json(verify.Challenge);
        }
    }
}
