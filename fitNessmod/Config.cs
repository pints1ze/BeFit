using System;
using System.IO;
using UnityEngine;

namespace BeFitMod
{
    public class Config
    {
        public string FilePath { get; }
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
        public event Action<Config> ConfigChangedEvent;

        private readonly FileSystemWatcher _configWatcher;
        private bool _saving;

        public Config(string filePath)
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

            _configWatcher = new FileSystemWatcher(Environment.CurrentDirectory)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "befit.cfg",
                EnableRaisingEvents = true
            };
            _configWatcher.Changed += ConfigWatcherOnChanged;

            }
        ~Config()
        {
            _configWatcher.Changed -= ConfigWatcherOnChanged;

        }
        public void Save()
        {
            _saving = true;
            ConfigSerializer.SaveConfig(this, FilePath);
            Console.WriteLine(Plugin.modLog + "BeFit Config Saved");

        }
        public void Load()
        {
            ConfigSerializer.LoadConfig(this, FilePath);
            Console.WriteLine(Plugin.modLog + "BeFit Config Loaded");
        }

        private void ConfigWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if(_saving)
            {
                _saving = false;
                return;
            }

            Load();

            if(ConfigChangedEvent != null)
            {
                ConfigChangedEvent(this);

            }
        }
    }
}
