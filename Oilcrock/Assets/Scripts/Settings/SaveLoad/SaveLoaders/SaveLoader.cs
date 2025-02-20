
using UnityEngine;

namespace SaveUtils.SaveLoaders
{
    public abstract class SaveLoader
    {
        protected string UploadIfNotDefined(ref string value, string name, string defaultValue) =>
            value ??= PlayerPrefs.GetString(name, defaultValue);
        protected float UploadIfNotDefined(ref float? value, string name, float defaultValue) =>
            value ??= PlayerPrefs.GetFloat(name, defaultValue);
        protected int UploadIfNotDefined(ref int? value, string name, int defaultValue) =>
            value ??= PlayerPrefs.GetInt(name, defaultValue);


        protected void SetValue(ref string valueRef, string name, string value)
        {
            valueRef = value;
            PlayerPrefs.SetString(name, value);
        }
        protected void SetValue(ref float? valueRef, string name, float value)
        {
            valueRef = value;
            PlayerPrefs.SetFloat(name, value);
        }
        protected void SetValue(ref int? valueRef, string name, int value)
        {
            valueRef = value;
            PlayerPrefs.SetInt(name, value);
        }

        public abstract void ResetToDefault();
    }
}
