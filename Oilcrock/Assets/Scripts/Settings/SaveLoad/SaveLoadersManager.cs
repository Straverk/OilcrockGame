using SaveUtils.SaveLoaders;

namespace SaveUtils
{
    public class SaveLoadersManager
    {
        public SettingsSaveLoader SettingsSaveLoader { get; private set; }
        public SeasonSaveLoader SeasonSaveLoader { get; private set; }

        public void Init()
        {
            SettingsSaveLoader ??= new();
            SeasonSaveLoader ??= new();
        }
    }
}
