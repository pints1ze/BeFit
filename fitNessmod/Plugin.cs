/* BeFit | fitNessMod
 * Purpose: Count and store calories burned while playing BeatSaber
 * Date: 12/22/18
 * Origin Contributor: Viscoci
 * Contributors: --
 * */
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BeFitMod
{
    public class Plugin : IPlugin
    {
        public readonly Config mainConfig = new Config(Path.Combine(Environment.CurrentDirectory, "UserData/befit.cfg"));
        public static UserConfigs userConfigs;
        public static Plugin Instance { get; set; }
        public string Name => "BeFitMod";
        public static string alias = "fitNessMod";
        public static string modLog = "[" + alias + " | LOG] ";
        public string Version => "v0.2.3";
        public string _currentUser;
        bool enabled = true;
        public static bool safetyEnabled = false;
        public static Vector3 counterPosition = new Vector3(-4.5f, 0.5f, 7f);

        private static List<string> _userPaths;

        public static List<string> RetrieveUsers()
        {
            
            _userPaths = Directory.GetFiles(Path.Combine(Application.dataPath, "../UserData/BeFitUsers/"),
                "*.cfg", SearchOption.AllDirectories).ToList();
            Console.WriteLine("Found " + _userPaths.Count + " users");
            _userPaths.Insert(0, "DefaultUser"); 
            return _userPaths;
        }

        MenuDisplay display;
        Igcv02x calCounter;

        public void OnApplicationStart()
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "../UserData/BeFitUsers/"));
            userConfigs = new UserConfigs(ModPrefs.GetString(alias, "lastUserPath", Path.Combine(Application.dataPath, "../UserData/BeFitUsers/Player" + DateTime.Now.Minute + ".cfg"), true));
            Instance = this;

            enabled = Instance.mainConfig.BeFitPluginEnabled;
            ModPrefs.SetString(alias, "version", Version.ToString());


            Console.WriteLine(modLog + "Current Date: " + DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss tt"));
            Console.WriteLine(modLog + Name + " " +  Version);


            var users = RetrieveUsers();
            if(users.Count == 0) { Console.WriteLine("No user profile found!"); return; }

            Plugin.Instance.mainConfig.sessionCalories = 0;
            userConfigs.sessionCalories = 0;

            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }


        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            calCounter = null;
            display = null;
            mainConfig.Save();
            userConfigs.Save();
        }


        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!enabled || safetyEnabled) return;  
            if (arg1.name == "GameCore") {  //Launch calories counter
                Console.WriteLine(modLog + "Scene Loaded succesfully");
                calCounter = null;
                calCounter = new GameObject("inGameCalorieCounter").AddComponent<Igcv02x>();
                Console.WriteLine(modLog + "Calorie counter loaded!");
            }
            if(arg1.name == "Menu")
            {
                if (display != null) { return; }
                display = null;
                display = new GameObject("MenuDisplay").AddComponent<MenuDisplay>();
            }
            return;
        }


        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "TutorialEnvironment")
            {
                if (!enabled || safetyEnabled) return;
                safetyEnabled = true; //Disable mod
                return;
            }
            else { safetyEnabled = false; } //Enable Mod
            if (scene.name == "Menu" && !safetyEnabled) /// Settings Submenus 
            {
                SubMenus.Settings();
                SubMenus.PlayerProperties();
                if (!enabled || safetyEnabled) return;
                BeFitUI.OnLoad();
            }
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
            if (_userPaths == null) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RetrieveUsers();
                if (_userPaths.Count == 1) return;
                var oldIndex = _userPaths.IndexOf(_currentUser);
                if(oldIndex >= _userPaths.Count - 1)
                {
                    oldIndex = -1;
                }              
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}
