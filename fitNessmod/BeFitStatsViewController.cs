using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using VRUI;
using TMPro;
using CustomUI.BeatSaber;

namespace BeFitMod
{
    class BeFitStatsViewController : VRUIViewController
    {
        public static TextMeshProUGUI titleText { get; set; }
        public static TextMeshProUGUI userWeight { get; set; }
        public static TextMeshProUGUI curSessBurn { get; set; }
        public static TextMeshProUGUI dailyCalBurn { get; set; }
        public static TextMeshProUGUI lifeCalBurn { get; set; }
        public static TextMeshProUGUI dailyCalGoal { get; set; }
        public static TextMeshProUGUI weeklyCalGoal { get; set; }
        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation) FirstActivation();
        }
        private void FirstActivation()
        {
            RectTransform container = new GameObject("BeFitUserStatsContainer", typeof(RectTransform)).transform as RectTransform;
            container.SetParent(rectTransform, false);
            container.anchorMin = new Vector2(0.05f, 0.0f);
            container.anchorMax = new Vector2(0.95f, 1.0f);
            container.sizeDelta = new Vector2(0, 0);

            System.Action<RectTransform, float, float, float, float, float> relative_layout =
                (RectTransform rt, float x, float y, float w, float h, float pivotx) =>
                {
                    rt.anchorMin = new Vector2(x, y);
                    rt.anchorMax = new Vector2(x + w, y + h);
                    rt.pivot = new Vector2(pivotx, 1f);
                    rt.sizeDelta = Vector2.zero;
                    rt.anchoredPosition = Vector2.zero;
                };

            titleText = BeatSaberUI.CreateText(container, "User Stats for " + Plugin.userConfigs.name, Vector2.zero);
            titleText.fontSize = 7.0f;
            titleText.alignment = TextAlignmentOptions.Center;
            relative_layout(titleText.rectTransform, 0f, 0.85f, 1f, 0.166f, 0.5f);
            ///// Weight //////////////////////////////////////
            TextMeshProUGUI uwStat = BeatSaberUI.CreateText(container, "Weight ", Vector2.zero);///////////////////////////////////////////
            uwStat.fontSize = 5.0f;
            uwStat.alignment = TextAlignmentOptions.Left;
            relative_layout(uwStat.rectTransform, 0f, 0.7f, 1f, 0.166f, 0.5f);

            userWeight = BeatSaberUI.CreateText(container, (Plugin.userConfigs.metricUnits ?  Plugin.userConfigs.weightKGS : Plugin.userConfigs.weightLBS).ToString(), Vector2.zero);
            userWeight.fontSize = 5.0f;
            userWeight.alignment = TextAlignmentOptions.Right;
            relative_layout(userWeight.rectTransform, 0f, 0.7f, 1f, 0.166f, 0.5f);
            ///// Current Session Calories //////////////////////////////////////
            TextMeshProUGUI csbStat = BeatSaberUI.CreateText(container, "Current Session Calories ", Vector2.zero);///////////////////////////////////////////
            csbStat.fontSize = 5.0f;
            csbStat.alignment = TextAlignmentOptions.Left;
            relative_layout(csbStat.rectTransform, 0f, 0.6f, 1f, 0.166f, 0.5f);

            curSessBurn = BeatSaberUI.CreateText(container, Plugin.userConfigs.sessionCalories.ToString(), Vector2.zero);
            curSessBurn.fontSize = 5.0f;
            curSessBurn.alignment = TextAlignmentOptions.Right;
            relative_layout(curSessBurn.rectTransform, 0f, 0.6f, 1f, 0.166f, 0.5f);
            ///// Today's Daily Calories Count //////////////////////////////////////
            TextMeshProUGUI dcbStat = BeatSaberUI.CreateText(container, "Today's Daily Calories Count ", Vector2.zero);///////////////////////////////////////////
            dcbStat.fontSize = 5.0f;
            dcbStat.alignment = TextAlignmentOptions.Left;
            relative_layout(dcbStat.rectTransform, 0f, 0.5f, 1f, 0.166f, 0.5f);

            dailyCalBurn = BeatSaberUI.CreateText(container, Plugin.userConfigs.dailyCalories.ToString(), Vector2.zero);
            dailyCalBurn.fontSize = 5.0f;
            dailyCalBurn.alignment = TextAlignmentOptions.Right;
            relative_layout(dailyCalBurn.rectTransform, 0f, 0.5f, 1f, 0.166f, 0.5f);
            ///////////////////////////////////////////
            TextMeshProUGUI lcbStat = BeatSaberUI.CreateText(container, "All Time Calories", Vector2.zero);///////////////////////////////////////////
            lcbStat.fontSize = 5.0f;
            lcbStat.alignment = TextAlignmentOptions.Left;
            relative_layout(lcbStat.rectTransform, 0f, 0.4f, 1f, 0.166f, 0.5f);

            lifeCalBurn = BeatSaberUI.CreateText(container, Plugin.userConfigs.lifeCalories.ToString(), Vector2.zero);
            lifeCalBurn.fontSize = 5.0f;
            lifeCalBurn.alignment = TextAlignmentOptions.Right;
            relative_layout(lifeCalBurn.rectTransform, 0f, 0.4f, 1f, 0.166f, 0.5f);
            ///////////////////////////////////////////
            TextMeshProUGUI dcgStat = BeatSaberUI.CreateText(container, "Daily Calories Goal ", Vector2.zero);
            dcgStat.fontSize = 6.0f;
            dcgStat.alignment = TextAlignmentOptions.Left;
            relative_layout(dcgStat.rectTransform, 0f, 0.3f, 1f, 0.166f, 0.5f);

            dailyCalGoal = BeatSaberUI.CreateText(container, Plugin.userConfigs.dailyCalorieGoal.ToString(), Vector2.zero);
            dailyCalGoal.fontSize = 6.0f;
            dailyCalGoal.alignment = TextAlignmentOptions.Right;
            relative_layout(dailyCalGoal.rectTransform, 0f, 0.3f, 1f, 0.166f, 0.5f);
            ///////////////////////////////////////////
            TextMeshProUGUI wcgStat = BeatSaberUI.CreateText(container, "Weekyl Calories Goal " , Vector2.zero);
            wcgStat.fontSize = 6.0f;
            wcgStat.alignment = TextAlignmentOptions.Left;
            relative_layout(wcgStat.rectTransform, 0f, 0.2f, 1f, 0.166f, 0.5f);

            weeklyCalGoal = BeatSaberUI.CreateText(container, Plugin.userConfigs.weeklyCalorieGoal.ToString(), Vector2.zero);
            weeklyCalGoal.fontSize = 6.0f;
            weeklyCalGoal.alignment = TextAlignmentOptions.Right;
            relative_layout(weeklyCalGoal.rectTransform, 0f, 0.2f, 1f, 0.166f, 0.5f);

        }
    }
}
