using System;
using System.IO;

namespace BeFitMod
{
    public class UserConfigs
    {
        public string FilePath { get; }
        public bool BeFitPluginEnabled = true;
        public string name = "Player1";
        public int lifeCalories = 0;
        public int dailyCalories = 0;
        public int sessionCalories = 0;
        public int calorieCounterAccuracy = 45;
        public int dailyCalorieGoal = 300;
        public int weeklyCalorieGoal = 3500;
        public int weightKGS = 60;
        public int weightLBS = 132;
        public bool lifeCaloriesDisplay = false;
        public bool dailyCaloriesDisplay = true;
        public bool sessionCaloriesDisplay = true;
        public bool lastGameCaloriesDisplay = true;
        public bool inGameCaloriesDisplay = true;
        public bool displayWeightOnLaunch = true;
        public bool metricUnits = false;


        public event Action<UserConfigs> UserConfigsChangedEvent;


        private readonly FileSystemWatcher _userConfigsWatcher;
        private bool _saving;

        public UserConfigs(string filePath)
        {
            FilePath = filePath;
            if (File.Exists(FilePath))
            {
                Load();
                var text = File.ReadAllText(FilePath);
                Save();
            }
            else
            {
                Save();
            }

            _userConfigsWatcher = new FileSystemWatcher(Environment.CurrentDirectory)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = name + ".cfg",
                EnableRaisingEvents = true
            };
            _userConfigsWatcher.Changed += UserConfigsWatcherOnChanged;

            }
        ~UserConfigs()
        {
            _userConfigsWatcher.Changed -= UserConfigsWatcherOnChanged;

        }
        public void Save()
        {
            _saving = true;
            ConfigSerializer.SaveConfig(this, FilePath);
            Console.WriteLine(Plugin.modLog + "BeFit UserConfigs Saved");

        }
        public void Load()
        {
            ConfigSerializer.LoadConfig(this, FilePath);
            Console.WriteLine(Plugin.modLog + "BeFit UserConfigs Loaded");
        }

        private void UserConfigsWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if(_saving)
            {
                _saving = false;
                return;
            }

            Load();

            if(UserConfigsChangedEvent != null)
            {
                UserConfigsChangedEvent(this);

            }
        }
    }
}
