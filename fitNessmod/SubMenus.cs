using UnityEngine;
using CustomUI.Settings;
using IllusionPlugin;

namespace BeFitMod
{
    class SubMenus : MonoBehaviour
    {
        public static void playerProperties()
        {
            SubMenu fitNessCalculating = SettingsUI.CreateSubMenu("Fitness Properties");
            BoolViewController shhiWeight = fitNessCalculating.AddBool("Show weight at launch?");
            shhiWeight.GetValue += delegate {
                return Plugin.Instance.Config.displayWeightOnLaunch; };
            shhiWeight.SetValue += delegate (bool mswv) {
                Plugin.Instance.Config.displayWeightOnLaunch = mswv;
            };

            BoolViewController units = fitNessCalculating.AddBool("Metric Units? (Kgs, cm)");
            units.GetValue += delegate {
                return Plugin.Instance.Config.metricUnits; };
            units.SetValue += delegate (bool lork) {
                Plugin.Instance.Config.metricUnits = lork;
            };

            bool lbsorkgs = Plugin.Instance.Config.metricUnits;
            if (lbsorkgs) ////Converted to kgs                                                                                                
            {
                IntViewController weightKGS = fitNessCalculating.AddInt("Weight (kgs)", 36, 363, 1);
                weightKGS.GetValue += delegate {
                    return Plugin.Instance.Config.weightKGS; };
                weightKGS.SetValue += delegate (int kgs)
                {
                    Plugin.Instance.Config.weightKGS = kgs;
                };
            }/////// Freedom Units                 
            else
            {
                IntViewController weightLBS = fitNessCalculating.AddInt("Weight (lbs)", 80, 800, 2);
                weightLBS.GetValue += delegate {
                    return Plugin.Instance.Config.weightLBS; };
                weightLBS.SetValue += delegate (int lbs) 
                {
                    Plugin.Instance.Config.weightLBS = lbs;
                };
            }
        }
        public static void Settings()
        {
            
            SubMenu befitSettings = SettingsUI.CreateSubMenu("BeFit Settings");

            IntViewController calCountAccuracy = befitSettings.AddInt("FPS Drop Reduction: ", 1, 65, 1);
            calCountAccuracy.GetValue += delegate {
                return Plugin.Instance.Config.calorieCounterAccuracy;
            }; 
            calCountAccuracy.SetValue += delegate (int acc) {
                Plugin.Instance.Config.calorieCounterAccuracy = acc;
            };

            BoolViewController viewInGame = befitSettings.AddBool("Show Calories In Game");
            viewInGame.GetValue += delegate {
                return Plugin.Instance.Config.inGameCaloriesDisplay;
            };
            viewInGame.SetValue += delegate (bool dcig) {
                Plugin.Instance.Config.inGameCaloriesDisplay = dcig; };

            BoolViewController viewCurrent = befitSettings.AddBool("Show Current Session Calories");
            viewCurrent.GetValue += delegate {
                return Plugin.Instance.Config.sessionCaloriesDisplay; };
            viewCurrent.SetValue += delegate (bool csv) {
                Plugin.Instance.Config.sessionCaloriesDisplay = csv;
                MenuDisplay.visibleCurrentCalories = csv;
            };

            BoolViewController viewDaily = befitSettings.AddBool("Show Daily Calories");
            viewDaily.GetValue += delegate {
                return Plugin.Instance.Config.dailyCaloriesDisplay; };
            viewDaily.SetValue += delegate (bool dcv) {
                Plugin.Instance.Config.dailyCaloriesDisplay = dcv;
                MenuDisplay.visibleDailyCalories = dcv;
            };

            BoolViewController viewLife = befitSettings.AddBool("Show All Calories");
            viewLife.GetValue += delegate {
                return Plugin.Instance.Config.lifeCaloriesDisplay; };
            viewLife.SetValue += delegate (bool lcv) {
                Plugin.Instance.Config.lifeCaloriesDisplay = lcv;
                MenuDisplay.visibleLifeCalories = lcv;
            };
            BoolViewController viewLast = befitSettings.AddBool("Show Last Song Calories");
            viewLast.GetValue += delegate {
                return Plugin.Instance.Config.lastGameCaloriesDisplay;
            };
            viewLast.SetValue += delegate (bool lgv) {
                Plugin.Instance.Config.lastGameCaloriesDisplay = lgv;
                MenuDisplay.visibleLastGameCalories = lgv;
            };
        }
    }
}
