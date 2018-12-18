using IllusionPlugin;
using System;
using UnityEngine;
using TMPro;

namespace BeFitMod
{
    class MenuDisplay : MonoBehaviour
    {
        public static Vector3 menuPosition = new Vector3(0f, 0.3f, 1.25f);
        public static Quaternion slantBottom = Quaternion.Euler(35, 0, 0);
        private int lifeCalories = Plugin.Instance.Config.lifeCalories;
        private int dailyCalories = Plugin.Instance.Config.dailyCalories;
        private int currentSessionCals = Plugin.Instance.Config.sessionCalories;
        private string rdCals = ModPrefs.GetString(Plugin.alias, "date", "dd.MM.yyyy", true);
        private string version = ModPrefs.GetString(Plugin.alias, "version", "-.-.-", false);
        public static bool visibleLifeCalories = Plugin.Instance.Config.lifeCaloriesDisplay;
        public static bool visibleCurrentCalories = Plugin.Instance.Config.sessionCaloriesDisplay;
        public static bool visibleDailyCalories = Plugin.Instance.Config.dailyCaloriesDisplay;
        public static bool visibleLastGameCalories = Plugin.Instance.Config.lastGameCaloriesDisplay;

        private bool unitMetric = Plugin.Instance.Config.metricUnits;
        GameObject countCSC;
        GameObject countLC;
        GameObject countDC;
        public static TextMeshPro countLGC { get; set; } //Last Game
        public static TextMeshPro cscText { get; set; } //Current Session Calories count
        public static TextMeshPro lcText { get; set; } //Life Calories count
        public static TextMeshPro dcText { get; set; } //Daily calories count
        public static TextMeshPro lgcText { get; set; } //Last Game Calories Count
        public static TextMeshPro labelLG { get; set; } //Last Game Label
        public static TextMeshPro labelcsc { get; set; } //Current Session Label
        public static TextMeshPro labelLC { get; set; } //Life Calories label
        public static TextMeshPro labelDC { get; set; } //Daily Calories Label
        void Awake()
        {
            Init();
        }
        void Init()
        {
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            if (rdCals != currentDate)
            {
                dailyCalories = 0;
                ModPrefs.SetInt(Plugin.alias, "dailyCalories", dailyCalories);
                ModPrefs.SetString(Plugin.alias, "date", currentDate);
            }
            //Init Current Session Counter #
            /////////////////////////////////////////////////////////////////////////
            cscText = this.gameObject.AddComponent<TextMeshPro>();
            cscText.renderer.enabled = visibleCurrentCalories;
            cscText.text = Plugin.Instance.Config.sessionCalories.ToString();
            cscText.fontSize = 2;
            cscText.color = Color.cyan;
            cscText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            cscText.alignment = TextAlignmentOptions.Center;
            cscText.rectTransform.position = menuPosition + new Vector3(-1, -0.2f, 0);
            cscText.rectTransform.rotation = slantBottom;
            //Init Current Sesion Counter Label
            /////////////////////////////////////////////////////////////////////////
            countCSC = new GameObject("countCSClabel");
            labelcsc = countCSC.AddComponent<TextMeshPro>();
            labelcsc.renderer.enabled = visibleCurrentCalories;
            labelcsc.text = "Current Session Calories";
            labelcsc.fontSize = 1;
            labelcsc.color = Color.white;
            labelcsc.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            labelcsc.alignment = TextAlignmentOptions.Center;
            labelcsc.rectTransform.position = menuPosition + new Vector3(-1f, 0, 0);
            labelcsc.rectTransform.rotation = slantBottom;
            /////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////
            //Init Life Calories Counter #
            /////////////////////////////////////////////////////////////////////////
            lcText = new GameObject("lifeCalories").gameObject.AddComponent<TextMeshPro>();
            lcText.renderer.enabled = visibleLifeCalories;
            lcText.text = Plugin.Instance.Config.lifeCalories.ToString();
            lcText.fontSize = 2;
            lcText.color = Color.cyan;
            lcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            lcText.alignment = TextAlignmentOptions.Center;
            lcText.rectTransform.position = menuPosition + new Vector3(1, -0.2f, 0);
            lcText.rectTransform.rotation = slantBottom;
            //Init Life Calories Counter Label
            /////////////////////////////////////////////////////////////////////////
            countLC = new GameObject("countLClabel");
            labelLC = countLC.AddComponent<TextMeshPro>();
            labelLC.renderer.enabled = visibleLifeCalories;
            labelLC.text = "All Calories";
            labelLC.fontSize = 1;
            labelLC.color = Color.white;
            labelLC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            labelLC.alignment = TextAlignmentOptions.Center;
            labelLC.rectTransform.position = menuPosition + new Vector3(1, 0, 0);
            labelLC.rectTransform.rotation = slantBottom;
            /////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////
            //Init Daily Calories Counter #
            /////////////////////////////////////////////////////////////////////////  
            dcText = new GameObject("dailyCalories").gameObject.AddComponent<TextMeshPro>();
            dcText.renderer.enabled = visibleDailyCalories;
            dcText.text = Plugin.Instance.Config.dailyCalories.ToString();
            dcText.fontSize = 2;
            dcText.color = Color.cyan;
            dcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            dcText.alignment = TextAlignmentOptions.Center;
            dcText.rectTransform.position = menuPosition + new Vector3(0, -0.2f, 0);
            dcText.rectTransform.rotation = slantBottom;
            //Init Daily Calories Counter Label
            /////////////////////////////////////////////////////////////////////////  
            countDC = new GameObject("countDClabel");
            labelDC = countDC.AddComponent<TextMeshPro>();
            labelDC.renderer.enabled = visibleDailyCalories;
            labelDC.text = "Daily Calories";
            labelDC.fontSize = 1;
            labelDC.color = Color.white;
            labelDC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            labelDC.alignment = TextAlignmentOptions.Center;
            labelDC.rectTransform.position = menuPosition + new Vector3(0, 0, 0);
            labelDC.rectTransform.rotation = slantBottom; 
            /////////////////////////////////////////////////////////////////////////
            //Init Last Game Calories Counter #
            /////////////////////////////////////////////////////////////////////////            
            lgcText = new GameObject("dailyCalories").gameObject.AddComponent<TextMeshPro>();
            lgcText.renderer.enabled = visibleLastGameCalories;
            lgcText.text = "";
            lgcText.fontSize = 2;
            lgcText.color = Color.cyan;
            lgcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            lgcText.alignment = TextAlignmentOptions.Center;
            lgcText.rectTransform.position = menuPosition + new Vector3(2.5f, -0.6f, 0f);
            lgcText.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            //Init Last Game Song name Label // Displays version number on launch
            /////////////////////////////////////////////////////////////////////////    
            countLGC = new GameObject("countLGClabel").gameObject.AddComponent<TextMeshPro>();
            countLGC.renderer.enabled = visibleLastGameCalories;
            countLGC.text = "BeFit Mod " + version;
            countLGC.fontSize = 1.5f;
            countLGC.color = Color.white;
            countLGC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            countLGC.alignment = TextAlignmentOptions.Center;
            countLGC.rectTransform.position = menuPosition + new Vector3(2.5f, -0.4f, 0f);
            countLGC.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            /////////////////////////////////////////////////////////////////////////
            //Init Last Game Calories Counter Label
            /////////////////////////////////////////////////////////////////////////  
            if(!Plugin.Instance.Config.displayWeightOnLaunch) { return; }
            string weight;
            if(unitMetric)
            {
                weight = Plugin.Instance.Config.weightKGS + "<size=60%>kgs";
            }
            else
            {
                weight = Plugin.Instance.Config.weightLBS + "<size=60%>lbs";
            }
            labelLG = new GameObject("countLGClabel").gameObject.AddComponent<TextMeshPro>();
            labelLG.renderer.enabled = visibleLastGameCalories;
            labelLG.text = "<size=100%>Current Weight Setting ~ " + weight; //I'm not set on the name
            labelLG.fontSize = 2f;
            labelLG.color = Color.white;
            labelLG.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            labelLG.alignment = TextAlignmentOptions.Center;
            labelLG.rectTransform.position = menuPosition + new Vector3(2.5f, -0.2f, 0f);
            labelLG.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            /////////////////////////////////////////////////////////////////////////
            
        }

        void OnDestroy()
        {
            //Console.WriteLine(Plugin.modLog + "Destroying menuDisplay...");
        }

    }
}
