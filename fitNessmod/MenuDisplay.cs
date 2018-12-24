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
        private int lifeCalories = Plugin.Instance.mainConfig.lifeCalories;
        private int dailyCalories = Plugin.Instance.mainConfig.dailyCalories;
        private int currentSessionCals = Plugin.Instance.mainConfig.sessionCalories;
        private string rdCals = ModPrefs.GetString(Plugin.alias, "date", "MM.dd.yyyy HH:mm:ss tt", true);
        private string version = ModPrefs.GetString(Plugin.alias, "version", "-.-.-", false);
        public static bool visibleLifeCalories = Plugin.Instance.mainConfig.lifeCaloriesDisplay;
        public static bool visibleCurrentCalories = Plugin.Instance.mainConfig.sessionCaloriesDisplay;
        public static bool visibleDailyCalories = Plugin.Instance.mainConfig.dailyCaloriesDisplay;
        public static bool visibleLastGameCalories = Plugin.Instance.mainConfig.lastGameCaloriesDisplay;

        private bool unitMetric = Plugin.Instance.mainConfig.metricUnits;
        GameObject countCSC;
        GameObject countLC;
        GameObject countDC;
        public static TextMeshPro CountLGC { get; set; } //Last Game
        public static TextMeshPro CscText { get; set; } //Current Session Calories count
        public static TextMeshPro LcText { get; set; } //Life Calories count
        public static TextMeshPro DcText { get; set; } //Daily calories count
        public static TextMeshPro LgcText { get; set; } //Last Game Calories Count
        public static TextMeshPro LabelLG { get; set; } //Last Game Label
        public static TextMeshPro Labelcsc { get; set; } //Current Session Label
        public static TextMeshPro LabelLC { get; set; } //Life Calories label
        public static TextMeshPro LabelDC { get; set; } //Daily Calories Label
        void Awake()
        {
            Init();
        }
        void Init()
        {

            string currentDate = DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss tt");
            DateTime currentDateTime;
            DateTime.TryParse(currentDate, out currentDateTime);
            DateTime lastDateCheck;
            DateTime.TryParse(rdCals, out lastDateCheck);
            
            if ((currentDateTime - lastDateCheck).TotalHours > 16)
            {
                dailyCalories = 0;
                ModPrefs.SetInt(Plugin.alias, "dailyCalories", dailyCalories);
                ModPrefs.SetString(Plugin.alias, "date", currentDate);
            }
            //Init Current Session Counter #
            /////////////////////////////////////////////////////////////////////////
            CscText = this.gameObject.AddComponent<TextMeshPro>();
            CscText.renderer.enabled = visibleCurrentCalories;
            CscText.text = Plugin.Instance.mainConfig.sessionCalories.ToString();
            CscText.fontSize = 2;
            CscText.color = Color.cyan;
            CscText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            CscText.alignment = TextAlignmentOptions.Center;
            CscText.rectTransform.position = menuPosition + new Vector3(-1, -0.2f, 0);
            CscText.rectTransform.rotation = slantBottom;
            //Init Current Sesion Counter Label
            /////////////////////////////////////////////////////////////////////////
            countCSC = new GameObject("countCSClabel");
            Labelcsc = countCSC.AddComponent<TextMeshPro>();
            Labelcsc.renderer.enabled = visibleCurrentCalories;
            Labelcsc.text = "Current Session Calories";
            Labelcsc.fontSize = 1;
            Labelcsc.color = Color.white;
            Labelcsc.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            Labelcsc.alignment = TextAlignmentOptions.Center;
            Labelcsc.rectTransform.position = menuPosition + new Vector3(-1f, 0, 0);
            Labelcsc.rectTransform.rotation = slantBottom;
            /////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////
            //Init Life Calories Counter #
            /////////////////////////////////////////////////////////////////////////
            LcText = new GameObject("lifeCalories").gameObject.AddComponent<TextMeshPro>();
            LcText.renderer.enabled = visibleLifeCalories;
            LcText.text = Plugin.Instance.mainConfig.lifeCalories.ToString();
            LcText.fontSize = 2;
            LcText.color = Color.cyan;
            LcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LcText.alignment = TextAlignmentOptions.Center;
            LcText.rectTransform.position = menuPosition + new Vector3(1, -0.2f, 0);
            LcText.rectTransform.rotation = slantBottom;
            //Init Life Calories Counter Label
            /////////////////////////////////////////////////////////////////////////
            countLC = new GameObject("countLClabel");
            LabelLC = countLC.AddComponent<TextMeshPro>();
            LabelLC.renderer.enabled = visibleLifeCalories;
            LabelLC.text = "All Calories";
            LabelLC.fontSize = 1;
            LabelLC.color = Color.white;
            LabelLC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LabelLC.alignment = TextAlignmentOptions.Center;
            LabelLC.rectTransform.position = menuPosition + new Vector3(1, 0, 0);
            LabelLC.rectTransform.rotation = slantBottom;
            /////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////
            //Init Daily Calories Counter #
            /////////////////////////////////////////////////////////////////////////  
            DcText = new GameObject("dailyCalories").gameObject.AddComponent<TextMeshPro>();
            DcText.renderer.enabled = visibleDailyCalories;
            DcText.text = Plugin.Instance.mainConfig.dailyCalories.ToString();
            DcText.fontSize = 2;
            DcText.color = Color.cyan;
            DcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            DcText.alignment = TextAlignmentOptions.Center;
            DcText.rectTransform.position = menuPosition + new Vector3(0, -0.2f, 0);
            DcText.rectTransform.rotation = slantBottom;
            //Init Daily Calories Counter Label
            /////////////////////////////////////////////////////////////////////////  
            countDC = new GameObject("countDClabel");
            LabelDC = countDC.AddComponent<TextMeshPro>();
            LabelDC.renderer.enabled = visibleDailyCalories;
            LabelDC.text = "Daily Calories";
            LabelDC.fontSize = 1;
            LabelDC.color = Color.white;
            LabelDC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LabelDC.alignment = TextAlignmentOptions.Center;
            LabelDC.rectTransform.position = menuPosition + new Vector3(0, 0, 0);
            LabelDC.rectTransform.rotation = slantBottom; 
            /////////////////////////////////////////////////////////////////////////
            //Init Last Game Calories Counter #
            /////////////////////////////////////////////////////////////////////////            
            LgcText = new GameObject("dailyCalories").gameObject.AddComponent<TextMeshPro>();
            LgcText.renderer.enabled = visibleLastGameCalories;
            LgcText.text = "";
            LgcText.fontSize = 2;
            LgcText.color = Color.cyan;
            LgcText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LgcText.alignment = TextAlignmentOptions.Center;
            LgcText.rectTransform.position = menuPosition + new Vector3(2.5f, -0.6f, 0f);
            LgcText.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            //Init Last Game Song name Label // Displays version number on launch
            /////////////////////////////////////////////////////////////////////////    
            CountLGC = new GameObject("countLGClabel").gameObject.AddComponent<TextMeshPro>();
            CountLGC.renderer.enabled = visibleLastGameCalories;
            CountLGC.text = "BeFit Mod " + version;
            CountLGC.fontSize = 1.5f;
            CountLGC.color = Color.white;
            CountLGC.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            CountLGC.alignment = TextAlignmentOptions.Center;
            CountLGC.rectTransform.position = menuPosition + new Vector3(2.5f, -0.4f, 0f);
            CountLGC.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            /////////////////////////////////////////////////////////////////////////
            //Init Last Game Calories Counter Label
            /////////////////////////////////////////////////////////////////////////  
            if(!Plugin.Instance.mainConfig.displayWeightOnLaunch) { return; }
            string weight;
            if(unitMetric)
            {
                weight = Plugin.Instance.mainConfig.weightKGS + "<size=60%>kgs";
            }
            else
            {
                weight = Plugin.Instance.mainConfig.weightLBS + "<size=60%>lbs";
            }
            LabelLG = new GameObject("countLGClabel").gameObject.AddComponent<TextMeshPro>();
            LabelLG.renderer.enabled = visibleLastGameCalories;
            LabelLG.text = "<size=100%>Current Weight Setting ~ " + weight;
            LabelLG.fontSize = 2f;
            LabelLG.color = Color.white;
            LabelLG.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LabelLG.alignment = TextAlignmentOptions.Center;
            LabelLG.rectTransform.position = menuPosition + new Vector3(2.5f, -0.2f, 0f);
            LabelLG.rectTransform.rotation = Quaternion.Euler(0, 60, 0);
            /////////////////////////////////////////////////////////////////////////
            
        }

        void OnDestroy()
        {
            //Console.WriteLine(Plugin.modLog + "Destroying menuDisplay...");
        }

    }
}
