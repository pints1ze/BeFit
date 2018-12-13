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
using CustomUI.MenuButton;
using CustomUI.Settings;
using CustomUI.Utilities;
using CustomUI.GameplaySettings;

namespace fitNessmod
{
    public class Plugin : IPlugin
    {
        public string Name => "fitNessMod";
        public string Version => "0.2.0"; //Patch 1 Fixed Tutorial Crash
        bool enabled = true;
        public static bool safetyEnabled = false;
        private readonly string[] env = { "DefaultEnvironment", "BigMirrorEnvironment", "TriangleEnvironment", "NiceEnvironment" };
        private int lifeCalories = ModPrefs.GetInt("fitNessMod", "lifeCalories", 0, true);
        private int dailyCalories = ModPrefs.GetInt("fitNessMod", "dailyCalories", 0, true);
        private string rdCals = ModPrefs.GetString("fitNessMod", "date", "dd.MM.yyyy", true);
        public static Vector3 counterPosition = new Vector3(-4.25f, 0.5f, 7f);
        MenuDisplay display;
        hmdDebugging headmovement;
        public void OnApplicationStart()
        {
            ModPrefs.SetString("fitNessMod", "version", "v " + Version.ToString());
            ModPrefs.SetInt("fitNessMod", "sessionCalories", 0);
            Console.WriteLine("[fitNessMod | LOG] Current Session Cals set to 0!");
            Console.WriteLine("[fitNessMod | LOG] Daily Calories loaded: " + dailyCalories);
            Console.WriteLine("[fitNessMod | LOG] Life of Mod Calories : " + lifeCalories);
            Console.WriteLine("[fitNessMod | LOG] Current Date: " + DateTime.Now.ToString("dd.MM.yyyy"));
            Console.WriteLine("[fitNessMod | LOG] Last Burn Date: " + rdCals);
            Console.WriteLine("[fitNessMod | LOG] Loaded!");
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            //calCount = null;
            //calCount = new GameObject("CalorieCounter").AddComponent<CalorieCounter>();

            if (!enabled || safetyEnabled) return;
            
            if (arg1.name == "GameCore") {  //Launch calories counter
                Console.WriteLine("[fitNessMod | LOG] Scene Loaded succesfully");
                headmovement = null;
                headmovement = new GameObject("hmdDebugging").AddComponent<hmdDebugging>();
                Console.WriteLine("[fitNessMod | LOG] calorie counter loaded!");
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
            if (arg0.name == "Menu") //On menu == display. HealthWarning used for layout setup. Will be removed later.
            {
                

                SubMenu settingsSubmenu = SettingsUI.CreateSubMenu("BeFit Menu");

                bool lbsorkgs = ModPrefs.GetBool("fitNessMod", "lbskgs", false, true);
                if (lbsorkgs)
                {
                    IntViewController weightKGS = settingsSubmenu.AddInt("Weight in Kilo Grams", 36, 363, 1);
                    weightKGS.GetValue += delegate { return ModPrefs.GetInt("fitNessMod", "weightLBS", (int) (60 * 2.2046f), true);
                    };
                    weightKGS.SetValue += delegate (int kgs) { ModPrefs.SetInt("fitNessMod", "weightLBS", (int) (kgs * 2.2046f)); };
                }
                else
                {
                    IntViewController weightLBS = settingsSubmenu.AddInt("Weight in lbs", 80, 800, 2);
                    weightLBS.GetValue += delegate { return ModPrefs.GetInt("fitNessMod", "weightLBS", 132, true); };
                    weightLBS.SetValue += delegate (int lbs) { ModPrefs.SetInt("fitNessMod", "weightLBS", lbs); };
                }


                
                BoolViewController viewDaily = settingsSubmenu.AddBool("Show Daily Calories Burned");
                viewDaily.GetValue += delegate { return ModPrefs.GetBool("fitNessModd", "dcv", true, true); };
                viewDaily.SetValue += delegate (bool dcv) { ModPrefs.SetBool("fitNessModd", "dcv", dcv);
                    MenuDisplay.visibleDailyCalories = dcv;
                };

                BoolViewController viewCurrent = settingsSubmenu.AddBool("Show Current Session Calories Burned");
                viewCurrent.GetValue += delegate { return ModPrefs.GetBool("fitNessModd", "csv", true, true); };
                viewCurrent.SetValue += delegate (bool csv) { ModPrefs.SetBool("fitNessModd", "csv", csv);
                    MenuDisplay.visibleCurrentCalories = csv;
                };

                BoolViewController viewLife = settingsSubmenu.AddBool("Show All Calories Burned");
                viewLife.GetValue += delegate { return ModPrefs.GetBool("fitNessModd", "lcv", true, true); };
                viewLife.SetValue += delegate (bool lcv) { ModPrefs.SetBool("fitNessModd", "lcv", lcv);
                    MenuDisplay.visibleLifeCalories = lcv;
                };

                BoolViewController viewLast = settingsSubmenu.AddBool("Show Last Song Calories Burned");
                viewLast.GetValue += delegate { return ModPrefs.GetBool("fitNessModd", "lgv", true, true); };
                viewLast.SetValue += delegate (bool lgv) { ModPrefs.SetBool("fitNessModd", "lgv", lgv);
                    MenuDisplay.visibleLastGameCalories = lgv; 
                };
            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            headmovement = null;
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
