using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Google.Cloud.Vision.V1;
using System.Collections;

//Need to download the sdk for it to work: 
// https://cloud.google.com/vision/docs/reference/libraries
// https://cloud.google.com/sdk/docs
// https://dl.google.com/dl/cloudsdk/channels/rapid/GoogleCloudSDKInstaller.exe
namespace HackZurich.Utilities
{
    public static class PictureLabelClient
    {
        public static ArrayList getDescriptionOfPicture(string picturePath = "C:\\HackatonTemp\\Apple.jpg", int numItemsToReturn = 3, double threshold = 0.5)
        {
            /*
            var image = Image.FromFile(picturePath);

            var client = ImageAnnotatorClient.Create();
            var response = client.DetectLabels(image);
            ArrayList allDesc = new ArrayList();
            foreach (var annotation in response)
            {
                if ((annotation.Description != null) && (annotation.Score > threshold))
                    allDesc.Add(annotation.Description);
            }
            int min = Math.Min(numItemsToReturn, allDesc.Count);
            allDesc = allDesc.GetRange(0, min);

            return allDesc;
            */

            return new ArrayList();
        }

    }
}