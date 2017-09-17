using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace HackZurich.Utils
{
    public static class WatsonClient
    {
        public static string getWatsonAnswerBasedOnIntent(string query = "where do banana grow")
        {
            string resultBasedOnIntent = "";

            Dictionary<string, string> watsonAnswer = getWatsonAnswerWithIntent(query);

            string watsonRes = "";
            string watsonIntent = "";
            string watsonEntity = "";

            foreach (var item in watsonAnswer)
            {
                if (item.Key.Equals("Response"))
                {
                    watsonRes = item.Value;
                }
                else if (item.Key.Equals("Intent"))
                {
                    watsonIntent = item.Value;
                }
                else if (item.Key.Equals("Entity"))
                {
                    watsonEntity = item.Value;
                }
            }

            switch (watsonIntent)
            {
                case "eatsomething":
                    resultBasedOnIntent = watsonRes;
                    break;
                case "origin":
                    resultBasedOnIntent = watsonRes;
                    break;
                default:
                    resultBasedOnIntent = watsonRes;
                    break;

            }
            return resultBasedOnIntent;
        }
        private static string sendSimpleRequestToWatson(string query = "where do banana grow")
        {
            string url = "https://gateway.watsonplatform.net/conversation/api/v1/workspaces/acceffd4-cce1-486f-ab0c-42455d31b86d/message?version=2017-05-26";
            var username = "4ac2790d-0711-442e-8962-9a2ea15ba76e";
            var password = "H1b1JZybELYj";
            var message = "{ \"input\":{ \"text\":\"" + query + "\"},\"context\":null}";

            byte[] data = Encoding.ASCII.GetBytes(message.ToString());

            WebRequest request = WebRequest.Create(url);
            request.Credentials = new System.Net.NetworkCredential(username, password);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            string responseContent = null;

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }
            return responseContent;
        }

        private static Dictionary<string, string> getWatsonAnswerWithIntent(string input = "where do banana grow")
        {
            string json = sendSimpleRequestToWatson(input);
            var answer = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[])
                },
                context = default(object)
            };

            answer = JsonConvert.DeserializeAnonymousType(json, answer);


            var output = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            var Intents = answer?.intents;
            string firstIntent = "";

            foreach (string line in Intents.ToString().Split('\n'))
            {
                if (line.Contains("intent"))
                {
                    string[] splittedLine = line.Split('\"');
                    if (splittedLine.Length > 4)
                    {
                        firstIntent = splittedLine[3];
                        break;
                    }
                }
            }
            /*
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add(output, firstIntent);
            return res; */

            string entityName = "";
            string entityValue = "";
            //bool sawEntity = false;
            foreach (string line in answer?.entities?.ToString().Split('\n'))
            {
                if (line.Contains("\"entity\""))
                {
   //                 sawEntity = true;
                    string[] splittedLine = line.Split('\"');
                    if (splittedLine.Length > 4)
                    {
                        entityName = splittedLine[3];
                    }
                }
                if (line.Contains("\"value\""))
                {
                    string[] splittedLine = line.Split('\"');
                    if (splittedLine.Length > 4)
                    {
                        entityValue = splittedLine[3];
                        break;
                    }
                }
            }

            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Response", output);
            res.Add("Intent", firstIntent);
            res.Add("Entity", entityValue);

            return res;
        }
    }
}