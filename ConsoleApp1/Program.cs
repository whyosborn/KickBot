using ConsoleApp1.API;
using RestSharp;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            String guid = Guid.NewGuid().ToString();
            RestClient homeClient = new RestClient("http://localhost:52085/Home/");
            IRestRequest homeRequest = new RestRequest("SetConnection", Method.POST);
            homeRequest.AddJsonBody(guid);
            IRestResponse response = homeClient.Execute(homeRequest);

            RestClient slackClient = new RestClient("https://slack.com/");
            IRestRequest slackRequest = new RestRequest("oauth/authorize", Method.GET);
            slackRequest.AddQueryParameter("client_id", "18212299189.324327579253");
            slackRequest.AddQueryParameter("scope", "channels:history, channels:read, groups:history, groups:read, im:history, im:read," +
                " links:read, mpim:history, mpim:read, pins:read, reactions:read, search:read, team:read, usergroups:read, users.profile:read, users:read, rtm:stream");
            slackRequest.AddQueryParameter("state", guid);
            slackRequest.AddQueryParameter("redirect_uri", "http://localhost:52085/Home/GetCode");
            response = slackClient.Execute(slackRequest);
            System.Diagnostics.Process.Start(response.ResponseUri.ToString());

            string accessToken = "";
            while(accessToken == null || accessToken.Length == 0)
            {
                homeRequest = new RestRequest("GetConnection", Method.POST);
                homeRequest.AddJsonBody(guid);
                accessToken = homeClient.Execute(homeRequest).Content.Replace("\"", "");
                Thread.Sleep(500);
            }

            SlackManager slack = new SlackManager(accessToken);
            Channel channel = null;
            foreach(Channel c in slack.GetChannelList().channels)
            {
                System.Diagnostics.Debug.WriteLine(c.name);
                if(c.name.Equals("random"))
                {
                    channel = c;
                    break;
                }
            }

            SlackStreamReader reader = new SlackStreamReader(accessToken);
            while (true)
            {
                reader.ReadNextEvent();
            }
        }
    }
}
