/* BeFit | fitNessMod
 * Purpose: Count and store calories burned while playing BeatSaber
 * Date: 12/14/18
 * Origin Contributor: Viscoci
 * Contributors: --
 * */
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace BeFitMod
{
    public class Plugin : IPlugin
    {
        public string Name => "BeFitMod";
        public static string alias = "fitNessMod";
        public static string modLog = "[" + alias + " | LOG] ";
        public string Version => "0.2.0"; //New Counting Algorithm
        bool enabled = true;
        public static bool safetyEnabled = false;
        public static Vector3 counterPosition = new Vector3(-4.25f, 0.5f, 7f);
        public bool legacyMode = ModPrefs.GetBool(alias, "legacyMode", false, true);
        MenuDisplay display;
        igcv02x calCounter;
        CalorieCounter legCalCounter;
        public void OnApplicationStart()
        {
            ModPrefs.SetString(alias, "version", "v " + Version.ToString());
            ModPrefs.SetInt(alias, "sessionCalories", 0);
            Console.WriteLine(modLog + "Current Date: " + DateTime.Now.ToString("dd.MM.yyyy"));
            Console.WriteLine(modLog + Name + " " +  Version);
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        } 
        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!enabled || safetyEnabled) return;  
            if (arg1.name == "GameCore" && !legacyMode) {  //Launch calories counter
                Console.WriteLine(modLog + "Scene Loaded succesfully");
                legCalCounter = null;
                calCounter = null;
                calCounter = new GameObject("inGameCalorieCounter").AddComponent<igcv02x>();
                Console.WriteLine(modLog + "Calorie counter loaded!");
            }
            else if(arg1.name == "GameCore" && legacyMode) //Launch Legacy Calorie Counter
            {
                Console.WriteLine(modLog + "Legacy Calorie Counter being loaded...");
                calCounter = null;
                legCalCounter = null;
                legCalCounter = new GameObject("legacyCalorieCounter").AddComponent<CalorieCounter>();
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
            if (arg0.name == "Menu" && !safetyEnabled) /// Settings Submenus 
            {
                SubMenus.Settings();
                SubMenus.playerProperties();
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
