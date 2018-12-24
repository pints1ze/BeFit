using UnityEngine;
using CustomUI.Settings;

namespace BeFitMod
{
    class SubMenus : MonoBehaviour
    {
        public static void PlayerProperties()
        {
            SubMenu fitNessCalculating = SettingsUI.CreateSubMenu("Fitness Properties");
            BoolViewController shhiWeight = fitNessCalculating.AddBool("Show weight at launch?");
            shhiWeight.GetValue += delegate {
                return Plugin.Instance.mainConfig.displayWeightOnLaunch; };
            shhiWeight.SetValue += delegate (bool mswv) {
                Plugin.Instance.mainConfig.displayWeightOnLaunch = mswv;
            };

            BoolViewController units = fitNessCalculating.AddBool("Metric Units? (Kgs, cm)");
            units.GetValue += delegate {
                return Plugin.Instance.mainConfig.metricUnits; };
            units.SetValue += delegate (bool lork) {
                Plugin.Instance.mainConfig.metricUnits = lork;
            };

            bool lbsorkgs = Plugin.Instance.mainConfig.metricUnits;
            if (lbsorkgs) ////Converted to kgs                                                                                                
            {
                IntViewController weightKGS = fitNessCalculating.AddInt("Weight (kgs)", 36, 363, 1);
                weightKGS.GetValue += delegate {
                    return Plugin.Instance.mainConfig.weightKGS; };
                weightKGS.SetValue += delegate (int kgs)
                {
                    Plugin.Instance.mainConfig.weightKGS = kgs;
                };
            }/////// Freedom Units                 
            else
            {
                IntViewController weightLBS = fitNessCalculating.AddInt("Weight (lbs)", 80, 800, 2);
                weightLBS.GetValue += delegate {
                    return Plugin.Instance.mainConfig.weightLBS; };
                weightLBS.SetValue += delegate (int lbs) 
                {
                    Plugin.Instance.mainConfig.weightLBS = lbs;
                };
            }
        }
        public static void Settings()
        {
            
            SubMenu befitSettings = SettingsUI.CreateSubMenu("BeFit Settings");
            BoolViewController pluginEnabled = befitSettings.AddBool("Plugin Enabled?");
            pluginEnabled.GetValue += delegate {
                return Plugin.Instance.mainConfig.BeFitPluginEnabled;
            };
            pluginEnabled.SetValue += delegate (bool plug) {
                Plugin.Instance.mainConfig.BeFitPluginEnabled = plug;
            };
            IntViewController calCountAccuracy = befitSettings.AddInt("FPS Drop Reduction ", 1, 65, 1);
            calCountAccuracy.GetValue += delegate {
                return Plugin.Instance.mainConfig.calorieCounterAccuracy;
            }; 
            calCountAccuracy.SetValue += delegate (int acc) {
                Plugin.Instance.mainConfig.calorieCounterAccuracy = acc;
            };

            BoolViewController viewInGame = befitSettings.AddBool("Show Calories In Game");
            viewInGame.GetValue += delegate {
                return Plugin.Instance.mainConfig.inGameCaloriesDisplay;
            };
            viewInGame.SetValue += delegate (bool dcig) {
                Plugin.Instance.mainConfig.inGameCaloriesDisplay = dcig; };

            BoolViewController viewCurrent = befitSettings.AddBool("Show Current Session Calories");
            viewCurrent.GetValue += delegate {
                return Plugin.Instance.mainConfig.sessionCaloriesDisplay; };
            viewCurrent.SetValue += delegate (bool csv) {
                Plugin.Instance.mainConfig.sessionCaloriesDisplay = csv;
                MenuDisplay.visibleCurrentCalories = csv;
            };

            BoolViewController viewDaily = befitSettings.AddBool("Show Daily Calories");
            viewDaily.GetValue += delegate {
                return Plugin.Instance.mainConfig.dailyCaloriesDisplay; };
            viewDaily.SetValue += delegate (bool dcv) {
                Plugin.Instance.mainConfig.dailyCaloriesDisplay = dcv;
                MenuDisplay.visibleDailyCalories = dcv;
            };

            BoolViewController viewLife = befitSettings.AddBool("Show All Calories");
            viewLife.GetValue += delegate {
                return Plugin.Instance.mainConfig.lifeCaloriesDisplay; };
            viewLife.SetValue += delegate (bool lcv) {
                Plugin.Instance.mainConfig.lifeCaloriesDisplay = lcv;
                MenuDisplay.visibleLifeCalories = lcv;
            };
            BoolViewController viewLast = befitSettings.AddBool("Show Last Song Calories");
            viewLast.GetValue += delegate {
                return Plugin.Instance.mainConfig.lastGameCaloriesDisplay;
            };
            viewLast.SetValue += delegate (bool lgv) {
                Plugin.Instance.mainConfig.lastGameCaloriesDisplay = lgv;
                MenuDisplay.visibleLastGameCalories = lgv;
            };
        }
    }
}
