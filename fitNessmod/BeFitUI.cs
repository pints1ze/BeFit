using System.Linq;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using VRUI;
using CustomUI.MenuButton;
using CustomUI.Utilities;

namespace BeFitMod
{
    class BeFitUI
    {
        private class BeFitStatsFlowCoordinator : GenericFlowCoordinator<BeFitStatsViewController, VRUIViewController, VRUIViewController> { }
        private FlowCoordinator _flowCoordinator = null;
        public BeFitUI()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        ~BeFitUI()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "Menu")
            {
                drawMenuButton();
                Console.WriteLine(Plugin.modLog + "Creating BeFit Stats Button");
            }
        }
        void Awake()
        {
            //create button
        }
        private void drawMenuButton()
        {
            MenuButtonUI.AddButton("BeFit Stats", delegate ()
            {
                var mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
                if(_flowCoordinator == null)
                {
                    var flowCoordinator = new GameObject("BeFitStatsViewController").AddComponent<BeFitStatsFlowCoordinator>();
                    flowCoordinator.OnContentCreated = (content) =>
                    {
                        content.onBackPressed = () =>
                        {
                            mainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { flowCoordinator, null, false });

                        };
                        return "Be Fit Values Set";
                    };

                }
            }
        }
        private void menuButtonClicked()
        {

        }

    }
}
