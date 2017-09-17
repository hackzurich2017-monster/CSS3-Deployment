using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HackZurich.Utils
{
    public static class NutritiousClient
    {
        public static Dictionary<string, string> getNutritiousData(string food)
        {
            string query = food; //"butter";
            string api_Key = "iEcAu9qJ8zSTB051T8eaDLh517cDmLe1ezyeg5Gz";

            string url = @"https://api.nal.usda.gov/ndb/search/?format=json&q=" + query + " & sort=r&max=25&offset=0&ds=Standard%20Reference&api_key=" + api_Key;

            string json = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            Dictionary<string, string> descriptions = new Dictionary<string, string>();
            string name = "";
            bool sawANameBefore = false;
            string ndbno = "";
            foreach (string line in json.Split('\n'))
            {
                if (line.Contains("\"ndbno\"") && sawANameBefore)
                {
                    ndbno = line.Split('\"')[3];
                    descriptions.Add(name, ndbno);
                    sawANameBefore = false;
                }

                if (line.Contains("\"name\""))
                {
                    name = line.Split('\"')[3];
                    sawANameBefore = true;
                }
                else sawANameBefore = false;
            }

            foreach (var v in descriptions)
            {
                if (v.Key.ToLower().StartsWith(query))
                {
                    ndbno = v.Value;
                    break;
                }
            }
            url = "https://api.nal.usda.gov/ndb/reports/?ndbno=" + ndbno + "&type=b&format=json&api_key=" + api_Key;

            request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            ArrayList whiteListedNutriants = new ArrayList { "Protein", "Carbohydrate, by difference", "Energy" };
            sawANameBefore = false;

            Dictionary<string, string> nutriations = new Dictionary<string, string>();

            foreach (string line in json.Split('\n'))
            {
                if (line.Contains("\"value\"") && sawANameBefore)
                {
                    string value = line.Split('\"')[3];
                    nutriations.Add(name, value);
                    sawANameBefore = false;
                }

                string valueOfItem;
                if (line.Split('\"').Length >= 4)
                {
                    valueOfItem = line.Split('\"')[3];
                }
                else valueOfItem = "";

                if (line.Contains("\"name\"") && whiteListedNutriants.Contains(valueOfItem))
                {
                    name = line.Split('\"')[3];
                    sawANameBefore = true;
                }
            }

            return nutriations;
        }
    }
}