using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ConsoleApp1.API
{
    public class SlackManager
    {
        private string AccessToken;
        private SlackSocketClient Slack;
        private ManualResetEventSlim ClientReady;

        public SlackManager(string accessToken)
        {
            AccessToken = accessToken;
            Slack = new SlackSocketClient(accessToken);
            ClientReady = new ManualResetEventSlim(false);
        }

        public ChannelListResponse GetChannelList()
        {
            ChannelListResponse list = null;
            Connect();
            Slack.GetChannelList((listResponse) =>
            {
                list = listResponse;
                ClientReady.Set();
            });
            ClientReady.Wait();
            ClientReady.Reset();
            return list;
        }

        public ChannelMessageHistory GetChannelHistory(Channel channel)
        {
            ChannelMessageHistory msgHistory = null;
            Connect();
            Slack.GetChannelHistory((history) =>
            {
                msgHistory = history;
                ClientReady.Set();
            }, channel);
            ClientReady.Wait();
            ClientReady.Reset();
            return msgHistory;
        }

        public DirectMessageConversationListResponse GetDMList()
        {
            DirectMessageConversationListResponse list = null;
            Connect();
            Slack.GetDirectMessageList((listResponse) =>
            {
                list = listResponse;
                ClientReady.Set();
            });
            ClientReady.Wait();
            ClientReady.Reset();
            return list;
        }

        public MessageHistory GetDMHistory(DirectMessageConversation convo)
        {
            MessageHistory msgHistory = null;
            Connect();
            Slack.GetDirectMessageHistory((history) =>
            {
                msgHistory = history;
                ClientReady.Set();
            }, convo);
            ClientReady.Wait();
            ClientReady.Reset();
            return msgHistory;
        }

        public GroupListResponse GetGroupList()
        {
            GroupListResponse list = null;
            Connect();
            Slack.GetGroupsList((listResponse) =>
            {
                list = listResponse;
                ClientReady.Set();
            });
            ClientReady.Wait();
            ClientReady.Reset();
            return list;
        }

        public GroupMessageHistory GetGroupHistory(Channel group)
        {
            GroupMessageHistory msgHistory = null;
            Connect();
            Slack.GetGroupHistory((history) =>
            {
                msgHistory = history;
                ClientReady.Set();
            }, group);
            ClientReady.Wait();
            ClientReady.Reset();
            return msgHistory;
        }

        private void Connect()
        {
            Slack.Connect((connected) =>
            {
                ClientReady.Set();
            }, () =>
            {
                //nothing
            });
        }
        

    }
}