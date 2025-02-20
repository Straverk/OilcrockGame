using System;
using UnityEngine;
using static GameSettings.Configs.AnnumsConfig;

namespace GameSettings.Configs.Annum
{
    [CreateAssetMenu(fileName = "SeasonConfig", menuName = "Setting/Seasons/Season Config", order = 1)]
    public class SeasonConfig : ScriptableObject
    {
        [SerializeField, Tooltip("The months of season")]
        public Months SeasonMonths;


        [Space, Header("Day shedule")]
        [Range(0, 24), Tooltip("Start time of the day")]
        public float DayStartTime = 6f;

        [Range(0, 24), Tooltip("End time of the day")]
        public float DayEndTime = 18f;


        [Space]
        [Range(0, 24), Tooltip("Start time of the night")]
        public float NightStartTime = 18f;

        [Range(0, 24), Tooltip("End time of the night")]
        public float NightEndTime = 6f;



        [Header("Sky Shaider Cubemap")]
        [SerializeField] public Material SkyboxShaderMaterial;



        [Header("Sun settings")]
        public float SunIntensity = 1f;
        public int SunTemperature = 5200;
        [Tooltip("Sun Intensity Multiply (time from 0 to 1)")] public AnimationCurve sunIntensityMultiplier = AnimationCurve.Constant(0, 1, 1);
        [Tooltip("Sun Temperature Multiply (time from 0 to 1)")] public AnimationCurve sunTemperatureMultiplier = AnimationCurve.Constant(0, 1, 1);



        [Header("Moon settings")]
        public float MoonIntensity = 1f;
        public float MoonTemperature = 12000f;
        [Tooltip("Moon Intensity Multiply (time from 0 to 1)")] public AnimationCurve moonIntensityMultiplier = AnimationCurve.Constant(0, 1, 1);
        [Tooltip("Moon Temperature Multiply (time from 0 to 1)")] public AnimationCurve moonTemperatureMultiplier = AnimationCurve.Constant(0, 1, 1);



        [Header("Stars settings")]
        public float StarsIntensity = 1f;



        private void OnValidate()
        {
            SkyboxShaderMaterial.SetFloat("DayFactor", 1);
        }

        public void SetDayFactor(float factor)
        {
            SkyboxShaderMaterial.SetFloat("DayFactor", factor);
        }
    }
}
