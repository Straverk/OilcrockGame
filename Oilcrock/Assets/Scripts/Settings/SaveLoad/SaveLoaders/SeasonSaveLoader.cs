using System;
using UnityEngine;

namespace SaveUtils.SaveLoaders
{
    public class SeasonSaveLoader : SaveLoader
    {
        public const int DefaultDate = 1088;


        private DateTime? dateTime = null;
        public DateTime DateTime
        {
            get => UploadIfNotDefined(ref dateTime, "dt", defaultValue: new());
            set => SetValue(ref dateTime, "dt", value);
        }

        public override void ResetToDefault()
        {
            dateTime = new();
            _ = DateTime;
        }

        public static uint DateTimeToInt(DateTime dt)
        {
            return /* Year 1-6  */ (uint)Math.Clamp(dt.Year - 2025, 0, 63) |
                /* Month   7-10 */ (uint)dt.Month << 6 |
                /* Day    11-15 */ (uint)dt.Day << 10 |
                /* Hour   16-20 */ (uint)dt.Hour << 15 |
                /* Minute 21-26 */ (uint)dt.Minute << 20 |
                /* Second 27-32 */ (uint)dt.Second << 26;
        }


        public static DateTime IntToDateTime(uint time)
        {
            return new DateTime(
                (int)((time & 0b111111) + 2025),
                (int)((time >> 6) & 0b1111),
                (int)((time >> 10) & 0b11111),
                (int)((time >> 15) & 0b11111),
                (int)((time >> 20) & 0b111111),
                (int)(time >> 26));
        }

        protected DateTime UploadIfNotDefined(ref DateTime? value, string name, DateTime defaultValue)
        {
            value ??= IntToDateTime((uint)PlayerPrefs.GetInt(name, DefaultDate));

            return value.Value;
        }
        protected void SetValue(ref DateTime? valueRef, string name, DateTime value)
        {
            valueRef = value;

            PlayerPrefs.SetInt(name, (int)DateTimeToInt(value));
        }
    }
}
