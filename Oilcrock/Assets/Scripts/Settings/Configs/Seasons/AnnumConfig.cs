using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameSettings.Configs.Annum;

namespace GameSettings.Configs
{
    [CreateAssetMenu(fileName = "Annums Config", menuName = "Setting/Seasons/Annums Config", order = 1)]
    public class AnnumsConfig : ScriptableObject
    {
        [Flags]
        public enum Months
        {
            January = 1,
            February = 1 << 1,
            March = 1 << 2,
            April = 1 << 3,
            May = 1 << 4,
            June = 1 << 5,
            July = 1 << 6,
            August = 1 << 7,
            September = 1 << 8,
            October = 1 << 9,
            November = 1 << 10,
            December = 1 << 11,

            Summer = June | July | August,
            Autumn = September | October | November,
            Winter = December | January | February,
            Spring = March | April | May,

            //All = Summer | August | Winter | Spring
        }

        [Serializable]
        public struct Date
        {
            public int Year;
            public int Month;
            public int Day;

            public int Hour;
        }


        private const int MinuteInDay = 1440;



        [SerializeField, Tooltip("Day Length in minutes")] public float DayMinutesLength;

        [Space]
        [SerializeField] public Date DefaultDate;

        [Space]
        [SerializeField] private List<SeasonConfig> seasonsConfigs;



        public float DayScaler => (int) (MinuteInDay / DayMinutesLength);



        private void OnValidate()
        {
            if (DayMinutesLength < 0)
            {
                Debug.LogWarning("The Day Minutes Length Value cannot be less than 0!");
                return;
            }
            if (DayMinutesLength == 0)
            {
                Debug.LogWarning("The Day Minutes Length Value cannot be 0!");
                return;
            }


            seasonsConfigs.Sort((x, y) => x.SeasonMonths.CompareTo(y.SeasonMonths));

            foreach (var season in Enum.GetValues(typeof(Months)).OfType<Months>())
            {
                var cfg = seasonsConfigs.FindAll(x => (x.SeasonMonths & season) != 0);
                if (cfg.Count == 0)
                {
                    Debug.LogWarning($"No \"{season}\" Season Config!");
                    break;
                }
                else if (cfg.Count > 1)
                {
                    Debug.LogWarning($"\"{season}\" Season more than one!");
                    break;
                }
            }
        }

        public SeasonConfig GetSeasonConfig(DateTime date) =>
            seasonsConfigs.Find(x => (x.SeasonMonths & (Months)(1 << (date.Month - 1))) != 0);
    }
}
