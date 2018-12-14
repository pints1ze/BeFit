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
            BoolViewController units = fitNessCalculating.AddBool("Metric Units? (Kgs, cm)");
            units.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "lbskgs", false, true); };
            units.SetValue += delegate (bool lork) { ModPrefs.SetBool(Plugin.alias, "lbskgs", lork); };
            bool lbsorkgs = ModPrefs.GetBool(Plugin.alias, "lbskgs", false, true);
            if (lbsorkgs) ////Converted to kgs                                                                                                
            {
                IntViewController weightKGS = fitNessCalculating.AddInt("Weight in Kilo Grams", 36, 363, 1);
                weightKGS.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "weightLBS", (int)(60 * 0.4535924), true); };
                weightKGS.SetValue += delegate (int kgs)
                {
                    ModPrefs.SetInt(Plugin.alias, "weightLBS", (int)(kgs * 2.2046f));
                };
            }/////// Freedom Units                 
            else
            {
                IntViewController weightLBS = fitNessCalculating.AddInt("Weight in lbs", 80, 800, 2);
                weightLBS.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "weightLBS", 132, true); };
                weightLBS.SetValue += delegate (int lbs) { ModPrefs.SetInt(Plugin.alias, "weightLBS", lbs); };

            }
        }
        public static void Settings()
        {
            
            SubMenu befitSettings = SettingsUI.CreateSubMenu("BeFit Settings");
            BoolViewController legacyMode = befitSettings.AddBool("Legacy Mode?");
            legacyMode.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "legacyMode", false, true); };
            legacyMode.SetValue += delegate (bool leg) { ModPrefs.SetBool(Plugin.alias, "legacyMode", leg); };
            IntViewController calCountAccuracy = befitSettings.AddInt("FPS Drop Reduction: ", 1, 45, 1);
            calCountAccuracy.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "caccVal", 30, true); }; 
            calCountAccuracy.SetValue += delegate (int acc) { ModPrefs.SetInt(Plugin.alias, "caccVal", acc);
            };
            BoolViewController viewDaily = befitSettings.AddBool("Show Daily Calories");
            viewDaily.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "dcv", true, true); }; 
            viewDaily.SetValue += delegate (bool dcv) {
                ModPrefs.SetBool(Plugin.alias, "dcv", dcv);
                MenuDisplay.visibleDailyCalories = dcv;
            };
            BoolViewController viewCurrent = befitSettings.AddBool("Show Current Session Calories");
            viewCurrent.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "csv", true, true); };
            viewCurrent.SetValue += delegate (bool csv) {
                ModPrefs.SetBool(Plugin.alias, "csv", csv);
                MenuDisplay.visibleCurrentCalories = csv;
            };
            BoolViewController viewLife = befitSettings.AddBool("Show All Calories");
            viewLife.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "lcv", false, true); };
            viewLife.SetValue += delegate (bool lcv) {
                ModPrefs.SetBool(Plugin.alias, "lcv", lcv);
                MenuDisplay.visibleLifeCalories = lcv;
            };
            BoolViewController viewLast = befitSettings.AddBool("Show Last Song Calories");
            viewLast.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "lgv", true, true); };
            viewLast.SetValue += delegate (bool lgv) {
                ModPrefs.SetBool(Plugin.alias, "lgv", lgv);
                MenuDisplay.visibleLastGameCalories = lgv;
            };
        }
    }
}
