using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using IllusionPlugin;
using TMPro;

/// <summary>
///  New Calorie Counting algorith that uses physical movements relative to weight and preset values.
///  Will be much mroe accurate than previous counting method.
///  
///  Needs Fixed:
///     - Left Hand Calorie COunts way to High
///     - Right Hand Calorie Counts way to High
///         - Find METS values for m/s may prove to be difficult for hand motions. 
///         - Tennis may be a viable comparison.
/// </summary>

namespace fitNessmod
{
    class hmdDebugging : MonoBehaviour
    {
        private StandardLevelSceneSetupDataSO lvlData;

        private int lifeCalories = ModPrefs.GetInt("fitNessMod", "lifeCalories", 0, true);
        private int dailyCalories = ModPrefs.GetInt("fitNessMod", "dailyCalories", 0, true);
        private int currentSessionCals = ModPrefs.GetInt("fitNessMod", "sessionCalories", 0, true);
        float roughFPS = 90f;
        float waitForSecondsTime = 0.25f;
        float playerWeight = ModPrefs.GetInt("fitNessMod", "weightLBS", 145, true);
        float weightKg;
        float totalCaloriesBurnt = 0;
        float[] headvelocityCoefficient = new float[5]{ 0.5f, 1.4f, 2, 2.4f, 3 };
        float[] METSVALShead = new float [5] { 8, 15f, 18, 20f, 22};
/// ////////////////////////////////////////////////
        float[] handvelocityCoefficient = new float[5] { 1.25f, 2.25f, 3.8f, 5f, 7.5f };
        float[] METSVALShands = new float[5] { 5f, 6.75f, 8f, 9.5f, 11f };
        /// /////////
        Vector3 HMDvelocity;
        Vector3 LHCvelocity;
        Vector3 RHCvelocity;
        GameObject LiveCount;
        public static TextMeshPro LiveCountText;
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        void Awake()
        {
            lvlData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().FirstOrDefault();
            Console.WriteLine("[fitNessMod | LOG] DEBUGGER!");
            weightKg = playerWeight * 0.4535924f; // Convert LBS to KG
            Plugin.safetyEnabled = false; //Debugging mode

            LiveCountText = this.gameObject.AddComponent<TextMeshPro>();
            LiveCountText.text = "0";
            LiveCountText.fontSize = 4;
            LiveCountText.color = Color.cyan;
            LiveCountText.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            LiveCountText.alignment = TextAlignmentOptions.Center;
            LiveCountText.rectTransform.position = Plugin.counterPosition + new Vector3(0, -0.4f, 0);

            LiveCount = new GameObject("Label");
            TextMeshPro label = LiveCount.AddComponent<TextMeshPro>();
            label.text = "Calories";
            label.fontSize = 3;
            label.color = Color.white;
            label.font = Resources.Load<TMP_FontAsset>("Beon SDF No-Glow");
            label.alignment = TextAlignmentOptions.Center;
            label.rectTransform.position = Plugin.counterPosition;
        }
        private void OnDestroy()
        {
            int calories = (int) totalCaloriesBurnt;
            headvelocityCoefficient = null;
            METSVALShead = null;
            handvelocityCoefficient = null;
            METSVALShands = null;
            MenuDisplay.countLGC.text = lvlData.difficultyBeatmap.level.songName;
            MenuDisplay.labelLG.text = "Last Played Song";
            MenuDisplay.lgcText.text = (calories).ToString();
            MenuDisplay.cscText.text = (currentSessionCals + calories).ToString();
            MenuDisplay.lcText.text = (lifeCalories + calories).ToString();
            MenuDisplay.dcText.text = (dailyCalories + calories).ToString();
            ModPrefs.SetInt("fitNessMod", "lifeCalories", lifeCalories + calories);
            ModPrefs.SetInt("fitNessMod", "dailyCalories", dailyCalories + calories);
            ModPrefs.SetInt("fitNessMod", "sessionCalories", currentSessionCals + calories);
            Console.WriteLine("[fitNessMod | LOG] Current Calories: " + ModPrefs.GetInt("fitNessMod", "sessionCalories", 0, true));
        }

        void Update()
        {
            InputTracking.GetNodeStates(nodeStates);
            if (!isRunning) {
                StartCoroutine("AverageVelocity");
            }
        }

        private void calCalc(float METS)
        {
            //=((($C2*3.5*$I$1)/200)/60)/90
            
            float calBurnedLastSecond = (float)(((METS * 3.5 * weightKg) / 200) / 60)*0.1f; //Calorie burned per second
            totalCaloriesBurnt += (float) calBurnedLastSecond;
            int displayText = (int) totalCaloriesBurnt;
            LiveCountText.text = displayText.ToString();
            //Console.WriteLine("[fitNessMod | LOG] Total Calories burned: " + totalCaloriesBurnt);
        }

        bool isRunning = false;
        IEnumerator AverageVelocity()
        {
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                isRunning = true;
                foreach (XRNodeState ns in nodeStates)
                {
                    ///////////////////////////////////////////////////////////////////////            Left Hand          ///////////////////////////////////////////            Left Hand
                    if (ns.nodeType == XRNode.LeftHand)
                    {

                        ns.TryGetVelocity(out LHCvelocity);
                        if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[0] && Math.Abs(LHCvelocity.x) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[1] && Math.Abs(LHCvelocity.x) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[2] && Math.Abs(LHCvelocity.x) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[3] && Math.Abs(LHCvelocity.x) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }
                        if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[0] && Math.Abs(LHCvelocity.y) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[1] && Math.Abs(LHCvelocity.y) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[2] && Math.Abs(LHCvelocity.y) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[3] && Math.Abs(LHCvelocity.y) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(LHCvelocity.y) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }
                        if (Math.Abs(LHCvelocity.z) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(LHCvelocity.z) >= handvelocityCoefficient[0] && Math.Abs(LHCvelocity.z) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(LHCvelocity.x) >= handvelocityCoefficient[1] && Math.Abs(LHCvelocity.z) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(LHCvelocity.z) >= handvelocityCoefficient[2] && Math.Abs(LHCvelocity.z) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(LHCvelocity.z) >= handvelocityCoefficient[3] && Math.Abs(LHCvelocity.z) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(LHCvelocity.z) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }

                    }
                    /////////////////////////////////////////////////////////////////////           Right Hand        ///////////////////////////////////////////            Right Hand
                    if (ns.nodeType == XRNode.RightHand)
                    {
                        ns.TryGetVelocity(out RHCvelocity);
                        if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[0] && Math.Abs(RHCvelocity.x) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[1] && Math.Abs(RHCvelocity.x) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[2] && Math.Abs(RHCvelocity.x) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[3] && Math.Abs(RHCvelocity.x) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }
                        if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[0] && Math.Abs(RHCvelocity.y) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[1] && Math.Abs(RHCvelocity.y) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[2] && Math.Abs(RHCvelocity.y) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[3] && Math.Abs(RHCvelocity.y) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(RHCvelocity.y) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }
                        if (Math.Abs(RHCvelocity.z) >= handvelocityCoefficient[0])
                        {
                            if (Math.Abs(RHCvelocity.z) >= handvelocityCoefficient[0] && Math.Abs(RHCvelocity.z) < handvelocityCoefficient[1])
                            {
                                calCalc(METSVALShands[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(RHCvelocity.x) >= handvelocityCoefficient[1] && Math.Abs(RHCvelocity.z) < handvelocityCoefficient[2])
                            {
                                calCalc(METSVALShands[1]);
                            }
                            else if (Math.Abs(RHCvelocity.z) >= handvelocityCoefficient[2] && Math.Abs(RHCvelocity.z) < handvelocityCoefficient[3])
                            {
                                calCalc(METSVALShands[2]);
                            }
                            else if (Math.Abs(RHCvelocity.z) >= handvelocityCoefficient[3] && Math.Abs(RHCvelocity.z) < handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[3]);
                            }
                            else if (Math.Abs(RHCvelocity.z) >= handvelocityCoefficient[4])
                            {
                                calCalc(METSVALShands[4]);
                            }

                        }
                    }
                    ///////////////////////////////////////////////////////////////////            Head           ///////////////////////////////////////////            Head
                    if (ns.nodeType == XRNode.Head)
                    {
                        ns.TryGetVelocity(out HMDvelocity);
                        if (Math.Abs(HMDvelocity.x) >= headvelocityCoefficient[0])
                        {
                            if (Math.Abs(HMDvelocity.x) >= headvelocityCoefficient[0] && Math.Abs(HMDvelocity.x) < headvelocityCoefficient[1])
                            {
                                calCalc(METSVALShead[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(HMDvelocity.x) >= headvelocityCoefficient[1] && Math.Abs(HMDvelocity.x) < headvelocityCoefficient[2])
                            {
                                calCalc(METSVALShead[1]);
                            }
                            else if (Math.Abs(HMDvelocity.x) >= headvelocityCoefficient[2] && Math.Abs(HMDvelocity.x) < headvelocityCoefficient[3])
                            {
                                calCalc(METSVALShead[2]);
                            }
                            else if (Math.Abs(HMDvelocity.x) >= headvelocityCoefficient[3] && Math.Abs(HMDvelocity.x) < headvelocityCoefficient[4])
                            {
                                calCalc(METSVALShead[3]);
                            }
                            else if (Math.Abs(HMDvelocity.x) >= 3)
                            {
                                calCalc(METSVALShead[4]);
                            }

                        }
                        if (Math.Abs(HMDvelocity.y) >= headvelocityCoefficient[0])
                        {
                            if (Math.Abs(HMDvelocity.y) >= headvelocityCoefficient[0] && Math.Abs(HMDvelocity.y) < headvelocityCoefficient[1])
                            {
                                calCalc(METSVALShead[0] + 0.5f); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(HMDvelocity.y) >= headvelocityCoefficient[1] && Math.Abs(HMDvelocity.y) < headvelocityCoefficient[2])
                            {
                                calCalc(METSVALShead[1] + 1f);
                            }
                            else if (Math.Abs(HMDvelocity.y) >= headvelocityCoefficient[2] && Math.Abs(HMDvelocity.y) < headvelocityCoefficient[3])
                            {
                                calCalc(METSVALShead[2] + 1.5f);
                            }
                            else if (Math.Abs(HMDvelocity.y) >= headvelocityCoefficient[3] && Math.Abs(HMDvelocity.y) < headvelocityCoefficient[4])
                            {
                                calCalc(METSVALShead[3] + 2f);
                            }
                            else if (Math.Abs(HMDvelocity.y) >= 3)
                            {
                                calCalc(METSVALShead[4] + 3f);
                            }

                        }
                        if (Math.Abs(HMDvelocity.z) >= headvelocityCoefficient[0])
                        {
                            if (Math.Abs(HMDvelocity.z) >= headvelocityCoefficient[0] && Math.Abs(HMDvelocity.z) < headvelocityCoefficient[1])
                            {
                                calCalc(METSVALShead[0]); //Equivalent to 2 to 3 mph for one hour
                            }
                            else if (Math.Abs(HMDvelocity.z) >= headvelocityCoefficient[1] && Math.Abs(HMDvelocity.z) < headvelocityCoefficient[2])
                            {
                                calCalc(METSVALShead[1]);
                            }
                            else if (Math.Abs(HMDvelocity.z) >= headvelocityCoefficient[2] && Math.Abs(HMDvelocity.z) < headvelocityCoefficient[3])
                            {
                                calCalc(METSVALShead[2]);
                            }
                            else if (Math.Abs(HMDvelocity.z) >= headvelocityCoefficient[3] && Math.Abs(HMDvelocity.z) < headvelocityCoefficient[4])
                            {
                                calCalc(METSVALShead[3]);
                            }
                            else if (Math.Abs(HMDvelocity.z) >= 3)
                            {
                                calCalc(METSVALShead[4]);
                            }

                        }
                    }
                }
                for (float duration = 0.1f; duration > 0; duration -= Time.fixedDeltaTime)
                {
                    yield return new WaitForFixedUpdate();
                }
                isRunning = false;
                
            }
        }





    }
}
