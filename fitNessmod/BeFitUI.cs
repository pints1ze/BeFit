using HMUI;
using IllusionPlugin;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRUI;
using CustomUI.MenuButton;
using CustomUI.Settings;
using UnityEngine.SceneManagement;
using CustomUI.BeatSaber;
using CustomUI.Utilities;

namespace BeFitMod
{
    class User
    {
        public string Name {
            get; set;
        }

        public string Path {
            get; set;
        }

        public string Weight {
            get; set;
        }

        public int WeeklyGoal {
            get; set;
        }

        public int DailyGoal
        {
            get; set;
        }

        public int LifeCalories
        {
            get; set;
        }

        public int DailyCalories
        {
            get; set;
        }

        public int SessionCalories
        {
            get; set;
        }
    }

    class BeFitUI : MonoBehaviour
    {
        private MainFlowCoordinator _mainFlowCoordinator;
        public static BeFitUI _instance;
        public class BeFitListFlowCoordinator : GenericFlowCoordinator<BeFitListViewController, BeFitStatsViewController> { };
        public BeFitListFlowCoordinator _beFitListFlowCoordinator;

        internal static void OnLoad()
        {
            if (_instance != null)
            {
                return;
            }
            new GameObject("BeFitUI").AddComponent<BeFitUI>();
        }

        private void Awake()
        {
            _instance = this;
            _mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            DontDestroyOnLoad(gameObject);
            CreateBeFitMenuButton();
            CreateSettingsUI();
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Menu") return;

            _mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();

            CreateBeFitMenuButton();
            CreateSettingsUI();
        }

        void CreateSettingsUI()
        {
            SubMenus.Settings();
            SubMenus.PlayerProperties();
        }

        private void CreateBeFitMenuButton()
        {
            Console.WriteLine("here");
            MenuButtonUI.AddButton(
                "BeFit",
                delegate ()
                {
                    if (_beFitListFlowCoordinator == null)
                    {
                        _beFitListFlowCoordinator = new GameObject("BeFitListFlowCoordinator").AddComponent<BeFitListFlowCoordinator>();
                        _beFitListFlowCoordinator.mainFlowCoordinator = _mainFlowCoordinator;
                        _beFitListFlowCoordinator.OnContentCreated = (content) =>
                        {
                            content.beFitListBackWasPressed = () =>
                            {
                                _mainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { _beFitListFlowCoordinator, null, false });
                            };
                            return "User Select";
                        };
                    }
                    _mainFlowCoordinator.InvokePrivateMethod("PresentFlowCoordinator", new object[] { _beFitListFlowCoordinator, null, false, false });
                });
            Console.WriteLine("Is the problem");
        }
    }
}
