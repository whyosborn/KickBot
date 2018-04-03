using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.API
{
    class SlackStreamReader
    {
        private const string SlackUrl = "https://slack.com/api/rtm.connect";

        private StreamReader Reader;
        private string AccessToken;

        public SlackStreamReader(string accessToken)
        {
            AccessToken = accessToken;
            Reader = CreateStreamReader();
        }

        private StreamReader CreateStreamReader()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SlackUrl);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            return new StreamReader(stream);
        }

        public void ReadNextEvent()
        {
            Task task = Read();
            task.Wait();
        }

        private async Task Read()
        {
            string line = await Reader.ReadLineAsync();
            Console.Out.WriteLine(line);
        }
    }
}
