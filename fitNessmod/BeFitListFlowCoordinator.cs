using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using VRUI;

namespace BeFitMod
{
    class BeFitListFlowCoordinator : FlowCoordinator
    {
        BeFitUI ui;

        public BeFitListViewController _beFitListViewController;
        public MainFlowCoordinator mainFlowCoordinator;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BeFit Users";
                ui = BeFitUI._instance;
            }
            if(_beFitListViewController == null)
            {
                _beFitListViewController = BeatSaberUI.CreateViewController<BeFitListViewController>();
                _beFitListViewController.beFitListBackWasPressed += Dismiss;
            }
            if(activationType == FlowCoordinator.ActivationType.AddedToHierarchy)
            {
                ProvideInitialViewControllers(_beFitListViewController, null, null);
            }
        }

        void Dismiss()
        {
            (mainFlowCoordinator as FlowCoordinator).InvokePrivateMethod("DismissFlowCoordinator", new object[] { this, null, false });
        }

        protected override void DidDeactivate(DeactivationType type)
        {

        }
    }
}
