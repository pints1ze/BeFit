using System;
using VRUI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;

namespace BeFitMod
{
    class GenericFlowCoordinator<TCONT, TRIGHT> : FlowCoordinator where TCONT : VRUIViewController where TRIGHT : VRUIViewController
    {
        private TCONT _contentViewController;
        public TRIGHT _rightViewController;
        public Func<TCONT, string> OnContentCreated;

        BeFitUI ui;

        public MainFlowCoordinator mainFlowCoordinator;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                ui = BeFitUI._instance;
                _contentViewController = BeatSaberUI.CreateViewController<TCONT>();
                _rightViewController = BeatSaberUI.CreateViewController<TRIGHT>();
                title = OnContentCreated(_contentViewController);
            }
            if (activationType == FlowCoordinator.ActivationType.AddedToHierarchy)
            {
                ProvideInitialViewControllers(_contentViewController, null, _rightViewController);
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
