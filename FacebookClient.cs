﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;


namespace PiratesName
{
     public class FacebookClient
    {
        private static FacebookClient instance;
        private string accessToken;
 
        private static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        private String appId = "179238955563951";
        private String clientSecret = "4976ec1d3a2261229d638de92376b7d9";
        private String scope = "publish_stream";
 
        public FacebookClient()
        {
            try
            {
                accessToken = (string)appSettings["accessToken"];
            }
            catch (KeyNotFoundException e)
            {
                accessToken = "";
            }
        }
 
        public static FacebookClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new FacebookClient();
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        public string AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                accessToken = value;
                if (accessToken.Equals(""))
                    appSettings.Remove("accessToken");
                else
                    appSettings.Add("accessToken", accessToken);
            }
        }

        public virtual String GetLoginUrl()
        {
            return "https://m.facebook.com/dialog/oauth?client_id=" + appId + "&redirect_uri=https://www.facebook.com/connect/login_success.html&scope=" + scope + "&display=touch";
        }

        public virtual String GetAccessTokenRequestUrl(string code)
        {
            return "https://graph.facebook.com/oauth/access_token?client_id=" + appId + "&redirect_uri=https://www.facebook.com/connect/login_success.html&client_secret=" + clientSecret + "&code=" + code;
        }

        public virtual String GetAccessTokenExchangeUrl(string accessToken)
        {
            return "https://graph.facebook.com/oauth/access_token?client_id=" + appId + "&client_secret=" + clientSecret + "&grant_type=fb_exchange_token&fb_exchange_token=" + accessToken;
        }

        public void PostMessageOnWall(string message, UploadStringCompletedEventHandler handler)
        {
            WebClient client = new WebClient();
            client.UploadStringCompleted += handler;
            client.UploadStringAsync(new Uri("https://graph.facebook.com/me/feed"), "POST", "message=" + HttpUtility.UrlEncode(message) + "&access_token=" + FacebookClient.Instance.AccessToken);
        }

        public void ExchangeAccessToken(UploadStringCompletedEventHandler handler)
        {
            WebClient client = new WebClient();
            client.UploadStringCompleted += handler;
            client.UploadStringAsync(new Uri(GetAccessTokenExchangeUrl(FacebookClient.Instance.AccessToken)), "POST", "");
        }

    }
}
