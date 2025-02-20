namespace SaveUtils.SaveLoaders
{
    public class SettingsSaveLoader : SaveLoader
    {
        private float? sensitivity = null;
        public float Sensitivity
        {
            get => UploadIfNotDefined(ref sensitivity, "sens", defaultValue: 5);
            set => SetValue(ref sensitivity, "sens", value);
        }

        public override void ResetToDefault()
        {
            sensitivity = null;
            _ = Sensitivity;
        }
    }
}
