using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SLC_Classview_CSharp.Global
{
    public class Utility
    {
        /// <summary>
        /// Get data from the service
        /// </summary>
        /// <param name="apiEndPoint">API endpoint string to the data</param>
        /// <returns></returns>
        public static string GetJsonData(string apiEndPoint)
        {
            string tokenid = HttpContext.Current.Session["access_token"].ToString();
            var result = Global.Utility.GetData(apiEndPoint, tokenid);
            return result;
        }

        /// <summary>
        /// Given the related links from the API result set, we get the related API call base on the "rel" property
        /// </summary>
        /// <param name="strLinks">the API result set links</param>
        /// <param name="methodName">the "rel" part of the result set links</param>
        /// <returns></returns>
        public static string GetRelatedApiEndPoint(string strLinks, string methodName)
        {
            var links = JArray.Parse(strLinks);
            for (var i = 0; i < links.Count; i++)
            {
                if (links[i]["rel"].ToString() == methodName)
                    return links[i]["href"].ToString();
            }
            return "";
        }

        /// <summary>
        /// Format JSON string data into JArray
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JArray GetArrayData(string endPoint, string token)
        {
            JArray response = JArray.Parse(GetData(endPoint, token));
            return response;
        }

        /// <summary>
        /// Call the the API to get raw data
        /// </summary>
        /// <param name="endPoint">API endpoint string to the data</param>
        /// <param name="token">the current authorization token</param>
        /// <returns></returns>
        public static string GetData(string endPoint, string token)
        {
            WebClient restClient = new WebClient();

            string bearerToken = string.Format("bearer {0}", token);
            restClient.Headers.Add("Authorization", bearerToken);
            restClient.Headers.Add("Content-Type", "application/vnd.slc+json");
            restClient.Headers.Add("Accept", "application/vnd.slc+json");

            return restClient.DownloadString(endPoint);
        }

        /// <summary>
        /// Call the the API to upload data
        /// </summary>
        /// <param name="endPoint">API endpoint string to the data</param>
        /// <param name="token">the current authorization token</param>
        /// <returns></returns>
        public static string PostData(string endPoint, string token, string param)
        {
            WebClient restClient = new WebClient();

            string bearerToken = string.Format("bearer {0}", token);
            restClient.Headers.Add("Authorization", bearerToken);
            restClient.Headers.Add("Content-Type", "application/vnd.slc+json");

            return restClient.UploadString(endPoint, "POST", param);
        }
        /// <summary>
        /// Call the the API to post data
        /// </summary>
        /// <param name="endPoint">API endpoint string to the data</param>
        /// <param name="token">the current authorization token</param>
        /// <returns></returns>
        public static string PutData(string endPoint, string token, string param)
        {
            WebClient restClient = new WebClient();

            string bearerToken = string.Format("bearer {0}", token);
            restClient.Headers.Add("Authorization", bearerToken);
            restClient.Headers.Add("Content-Type", "application/vnd.slc+json");
            restClient.Headers.Add("Accept", "application/vnd.slc+json");

            var result =  restClient.UploadString(endPoint, "PUT", param);
            return result;
        }



    }
}