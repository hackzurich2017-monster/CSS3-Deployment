using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackZurich.Utils
{
    static public class FoodCalculator
    {
        private const int proteinTarget = 15;
        private const int fatTarget = 30;
        private const int carbTarget = 55;

        static public string calculateWhatFoodINeed(int protein, int fat, int carb)
        {
            int total = protein + carb + fat;
            int pratio = proteinTarget - (protein * 100) / total;
            int fratio = fatTarget - (fat * 100) / total;
            int cratio = carbTarget - (carb * 100) / total;

            //Console.WriteLine(pratio + " " + fratio + " " + cratio + " " + total);

            if (pratio > fratio)
            {
                if (pratio > cratio)
                {
                    //I need Protein
                    return "Beans";
                }
                //I need Carb
                return "Bread";
            }
            else if (fratio > cratio)
            {
                //I need Fat
                return "Swiss Cheese";
            }
            //I need Carb
            return "Bread";
        }
    }
}