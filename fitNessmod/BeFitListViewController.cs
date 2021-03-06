﻿using HMUI;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using VRUI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.IO;
namespace BeFitMod
{
    class BeFitListViewController : VRUIViewController, TableView.IDataSource
    {
        private int selected;
        BeFitUI ui;
        public Button _pageUpButton;
        public Button _pageDownButton;
        public Button _backButton;
        public Button _newUserButton;
        public TextMeshProUGUI _versionNumber;

        public TableView _beFitTableView;
        LevelListTableCell _songListTableCellInstance;

        private List<User> _user = new List<User>();
        public Action beFitListBackWasPressed;
        public Action beFitListNewUserPressed;



        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            try
            {
                LoadUsers(firstActivation);
                if (firstActivation)
                {
                    for(int i=0; i <_user.Count; i++)
                        if(_user[i].Path == Plugin.Instance._currentUser)
                        {
                            selected = i;
                        }
                    ui = BeFitUI._instance;
                    _songListTableCellInstance = Resources.FindObjectsOfTypeAll<LevelListTableCell>().First(x => (x.name == "LevelListTableCell"));

                    RectTransform container = new GameObject("BeFitListContainer", typeof(RectTransform)).transform as RectTransform;
                    container.SetParent(rectTransform, false);
                    container.sizeDelta = new Vector2(60f, 0f);

                    _beFitTableView = new GameObject("BeFitListTableView").AddComponent<TableView>();
                    _beFitTableView.gameObject.AddComponent<RectMask2D>();
                    _beFitTableView.transform.SetParent(container, false);

                    (_beFitTableView.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
                    (_beFitTableView.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
                    (_beFitTableView.transform as RectTransform).sizeDelta = new Vector2(0f, 60f);
                    (_beFitTableView.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);

                    _beFitTableView.SetPrivateField("_preallocatedCells", new TableView.CellsGroup[0]);
                    _beFitTableView.SetPrivateField("_isInitialized", false);
                    _beFitTableView.dataSource = this;

                    _beFitTableView.didSelectRowEvent += _BeFitTableView_DidSelectRowEvent;

                    _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), container, false);
                    (_pageUpButton.transform as RectTransform).anchoredPosition = new Vector2(0f, 30f);
                    _pageUpButton.interactable = true;
                    _pageUpButton.onClick.AddListener(delegate ()
                    {
                        _beFitTableView.PageScrollUp();
                    });

                    _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), container, false);
                    (_pageDownButton.transform as RectTransform).anchoredPosition = new Vector2(0f, -30f);
                    _pageDownButton.interactable = true;
                    _pageDownButton.onClick.AddListener(delegate ()
                    {
                        _beFitTableView.PageScrollDown();
                    });

                    _newUserButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PlayButton")), container, false);
                    (_newUserButton.transform as RectTransform).anchoredPosition = new Vector2(40f, -30f);
                    BeatSaberUIExtensions.SetButtonText(_newUserButton, "New User");
                    _newUserButton.interactable = true;
                    _newUserButton.onClick.AddListener(delegate ()
                    {
                        UserConfigs newUser = new UserConfigs(Path.Combine(Application.dataPath, "../UserData/BeFitUsers/Player" + DateTime.Now.Millisecond + ".cfg"));
                        newUser.Save();
                        UnLoadUsers();
                        LoadUsers(firstActivation);
                        RefreshScreen();
                        if (beFitListNewUserPressed != null) beFitListNewUserPressed();
                    });

                    _versionNumber = Instantiate(Resources.FindObjectsOfTypeAll<TextMeshProUGUI>().First(x => (x.name == "Text")), rectTransform, false);

                    (_versionNumber.transform as RectTransform).anchoredPosition = new Vector2(-10f, 10f);
                    (_versionNumber.transform as RectTransform).anchorMax = new Vector2(1f, 0f);
                    (_versionNumber.transform as RectTransform).anchorMin = new Vector2(1f, 0f);

                    string versionNumber = Plugin.Instance.Version;
                    _versionNumber.text = versionNumber;
                    _versionNumber.fontSize = 5;
                    _versionNumber.color = Color.white;

                    if (_backButton == null)
                    {
                        _backButton = BeatSaberUI.CreateBackButton(rectTransform as RectTransform);

                        _backButton.onClick.AddListener(delegate ()
                        {
                            if (beFitListBackWasPressed != null) beFitListBackWasPressed();
                            UnLoadUsers();
                            
                        });
                    }
                }

                _beFitTableView.SelectRow(selected);
                _beFitTableView.ScrollToRow(selected, true);

                PreviewCurrent();
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION IN DidActivate: " + e);
            }
        }
        private void PreviewCurrent()
        {
            if(selected != 0)
            {
                GenerateStats(selected);
            }
            else
            {
                Console.WriteLine("Preview");
                GenerateStatsOriginal();
            }
        }

        protected override void DidDeactivate(DeactivationType type)
        {
            base.DidDeactivate(type);
        }

        public void RefreshScreen()
        {
            _beFitTableView.ReloadData();
        }

        private void _BeFitTableView_DidSelectRowEvent(TableView sender, int row)
        {
            Plugin.Instance._currentUser = this._user[row].Path;
            selected = row;
            if(row == 0)
            {
                GenerateStatsOriginal();
                return;
            }
            GenerateStats(row);
        }

        public void GenerateStatsOriginal()
        {
            Console.WriteLine("NOthing happens");
        }

        public void GenerateStats(int index)
        {
            Plugin.Instance._currentUser = _user[index].Path;
            selected = index;
            Console.WriteLine($"Selected user {_user[index].Name} weighing {_user[index].Weight}");

            if(_user[index] != null)
            {
                try
                {
                    BeFitStatsViewController.titleText.text =  _user[index].Name.ToString();
                    BeFitStatsViewController.userWeight.text = _user[index].Weight.ToString();
                    BeFitStatsViewController.curSessBurn.text =  _user[index].SessionCalories.ToString();
                    BeFitStatsViewController.dailyCalBurn.text = _user[index].DailyCalories.ToString();
                    BeFitStatsViewController.lifeCalBurn.text =  _user[index].LifeCalories.ToString();
                    BeFitStatsViewController.dailyCalGoal.text = _user[index].DailyGoal.ToString();
                    BeFitStatsViewController.weeklyCalGoal.text =  _user[index].WeeklyGoal.ToString();
                    Console.WriteLine("Index: " + index);
                }
                catch(NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public float RowHeight()
        {
            return 10f;
        }

        public int NumberOfRows()
        {
            return _user.Count;
        }

        public TableCell CellForRow(int row)
        {
            LevelListTableCell _tableCell = Instantiate(_songListTableCellInstance);
            
            
            var BeFit = _user.ElementAtOrDefault(row);
            _tableCell.songName = BeFit.Name;
            _tableCell.author = BeFit.Weight.ToString();
            _tableCell.coverImage = Sprite.Create(Texture2D.blackTexture, new Rect(), Vector2.zero);
            _tableCell.reuseIdentifier = "BeFitListCell";
            Console.WriteLine("I'm too tired for this shit");

            
            return _tableCell;
        }
        public void UnLoadUsers()
        {
            Console.WriteLine("Unloading Users");
            for (int i = 0; i < _user.Count; i++)
            {
                _user.Remove(_user[i]);
            }
        }

        public void LoadUsers(bool FirstRun)
        {
            Console.WriteLine("Loading users!");
            if (FirstRun)
            {
                foreach (string use in Plugin.RetrieveUsers())
                {
                    if(use == null)
                    {
                        return;
                    }
                    UserConfigs tempUserCfgs = new UserConfigs(use);
                    User tempuse = new User();
                    tempuse.Name = tempUserCfgs.name;
                    tempuse.Path = tempUserCfgs.FilePath;
                    if (tempUserCfgs.metricUnits) tempuse.Weight = tempUserCfgs.weightKGS + "<size=60%>kgs";
                    else { tempuse.Weight = tempUserCfgs.weightLBS + "<size=60%>lbs"; }
                    tempuse.WeeklyGoal = tempUserCfgs.weeklyCalorieGoal;
                    tempuse.DailyGoal = tempUserCfgs.dailyCalorieGoal;
                    tempuse.LifeCalories = tempUserCfgs.lifeCalories;
                    tempuse.DailyCalories = tempUserCfgs.dailyCalories;
                    tempuse.SessionCalories = tempUserCfgs.sessionCalories;
                    _user.Add(tempuse);
                }
               
            }

        }
    }
}
