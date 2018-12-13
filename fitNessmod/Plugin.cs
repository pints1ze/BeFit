/* BeFit | fitNessMod
 * Purpose: Count and store calories burned while playing BeatSaber
 * Date: 12/7/18
 * O: Viscoci
 * C: --
 * */
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using CustomUI.Settings;

namespace BeFitMod
{
    public class Plugin : IPlugin
    {
        public string Name => "BeFitMod";
        public static string alias = "fitNessMod";
        public string Version => "0.2.0"; //New Counting Algorithm
        bool enabled = true;
        public static bool safetyEnabled = false;
        private readonly string[] env = { "DefaultEnvironment", "BigMirrorEnvironment", "TriangleEnvironment", "NiceEnvironment" };
        private int lifeCalories = ModPrefs.GetInt(Plugin.alias, "lifeCalories", 0, true);
        private int dailyCalories = ModPrefs.GetInt(Plugin.alias, "dailyCalories", 0, true);
        private string rdCals = ModPrefs.GetString(Plugin.alias, "date", "dd.MM.yyyy", true);
        public static Vector3 counterPosition = new Vector3(-4.25f, 0.5f, 7f);
        MenuDisplay display;
        igcv02x calCounter;
        public void OnApplicationStart()
        {
            ModPrefs.SetString("fitNessMod", "version", "v " + Version.ToString());
            ModPrefs.SetInt(Plugin.alias, "sessionCalories", 0);
            Console.WriteLine(Plugin.alias + " LOG| Current Session Cals set to 0!");
            Console.WriteLine(Plugin.alias + " LOG| Daily Calories loaded: " + dailyCalories);
            Console.WriteLine(Plugin.alias + " LOG| Life of Mod Calories : " + lifeCalories);
            Console.WriteLine(Plugin.alias + " LOG| Current Date: " + DateTime.Now.ToString("dd.MM.yyyy"));
            Console.WriteLine(Plugin.alias + " LOG| Last Burn Date: " + rdCals);
            Console.WriteLine(Plugin.alias + " LOG| " + Name + Version);
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!enabled || safetyEnabled) return;  
            if (arg1.name == "GameCore") {  //Launch calories counter
                Console.WriteLine(Plugin.alias + " LOG|  Scene Loaded succesfully");
                calCounter = null;
                calCounter = new GameObject("hmdDebugging").AddComponent<igcv02x>();
                Console.WriteLine(Plugin.alias + " LOG|  calorie counter loaded!");
            }
            if(arg1.name == "Menu")
            {
                if (display != null) { return; }
                display = null;
                display = new GameObject("MenuDisplay").AddComponent<MenuDisplay>();
            }


            return;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "TutorialEnvironment")
            {
                safetyEnabled = true; //Disable mod
                return;
            }
            else { safetyEnabled = false; } //Enable Mod
            if (arg0.name == "Menu" && !safetyEnabled)
            {
                SubMenu befitSettings = SettingsUI.CreateSubMenu("BeFit Menu"); //////// Submenu Created //////////
                SubMenu befitStats = SettingsUI.CreateSubMenu("Fitness Statistics"); //////// Statistics Page Created ////////

                IntViewController cbAllTime = befitStats.AddInt("Calories Burned All Time", 0, Int32.MaxValue, 0);
                cbAllTime.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "lifeCalories", 0, true); };
                
                IntViewController cbCurSession = befitStats.AddInt("Calories Burned All Time", 0, Int32.MaxValue, 0);
                cbCurSession.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "sessionCalories", 0, true); };
                IntViewController cbDate = befitStats.AddInt("Calories Burned All Time", 0, Int32.MaxValue, 0);
                cbDate.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "dailyCalories", 0, true); };



                bool lbsorkgs = ModPrefs.GetBool(Plugin.alias, "lbskgs", false, true);
                if (lbsorkgs)
                {
                    IntViewController weightKGS = befitSettings.AddInt("Weight in Kilo Grams", 36, 363, 1);
                    weightKGS.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "weightLBS", (int) (60 * 2.2046f), true);
                    };
                    weightKGS.SetValue += delegate (int kgs) { ModPrefs.SetInt(Plugin.alias, "weightLBS", (int) (kgs * 2.2046f)); };

                    IntViewController allTimeLBSburned = befitStats.AddInt("Kilo Grams burned ", 0, Int32.MaxValue, 0);
                    allTimeLBSburned.GetValue += delegate { return (int)(((ModPrefs.GetInt(Plugin.alias, "lifeCalories", 0, true)) * 2.204623f) /7700); };
                }
                else
                {
                    IntViewController weightLBS = befitSettings.AddInt("Weight in lbs", 80, 800, 2);
                    weightLBS.GetValue += delegate { return ModPrefs.GetInt(Plugin.alias, "weightLBS", 132, true); };
                    weightLBS.SetValue += delegate (int lbs) { ModPrefs.SetInt(Plugin.alias, "weightLBS", lbs); };

                    IntViewController allTimeLBSburned = befitStats.AddInt("Calories Burned All Time", 0, Int32.MaxValue, 0);
                    allTimeLBSburned.GetValue += delegate { return (ModPrefs.GetInt(Plugin.alias, "lifeCalories", 0, true)) / 3500; };
                }
                ////Daily
                BoolViewController viewDaily = befitSettings.AddBool("Show Daily Calories Burned");
                viewDaily.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "dcv", true, true); };
                viewDaily.SetValue += delegate (bool dcv) { ModPrefs.SetBool(Plugin.alias, "dcv", dcv);
                    MenuDisplay.visibleDailyCalories = dcv;
                };
                ////Current
                BoolViewController viewCurrent = befitSettings.AddBool("Show Current Session Calories Burned");
                viewCurrent.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "csv", true, true); };
                viewCurrent.SetValue += delegate (bool csv) { ModPrefs.SetBool(Plugin.alias, "csv", csv);
                    MenuDisplay.visibleCurrentCalories = csv;
                };
                ////All
                BoolViewController viewLife = befitSettings.AddBool("Show All Calories Burned");
                viewLife.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "lcv", false, true); };
                viewLife.SetValue += delegate (bool lcv) { ModPrefs.SetBool(Plugin.alias, "lcv", lcv);
                    MenuDisplay.visibleLifeCalories = lcv;
                };
                ////Last Song
                BoolViewController viewLast = befitSettings.AddBool("Show Last Song Calories Burned");
                viewLast.GetValue += delegate { return ModPrefs.GetBool(Plugin.alias, "lgv", true, true); };
                viewLast.SetValue += delegate (bool lgv) { ModPrefs.SetBool(Plugin.alias, "lgv", lgv);
                    MenuDisplay.visibleLastGameCalories = lgv; 
                };
            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            calCounter = null;
            display = null;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
