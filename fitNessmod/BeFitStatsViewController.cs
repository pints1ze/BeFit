using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRUI;
using HMUI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using TMPro;
using System.Collections.Generic;

namespace BeFitMod
{
    class BeFitStatsViewController : VRUIViewController, TableView.IDataSource
    {
        private Button _backButton;
        private Button _pageUpButton;
        private Button _pageDownButton;
        private TextMeshProUGUI _versionNumber;
        private TableView _tableView;
        public Action onBackPressed;
        private LevelListTableCell _tableCellTemplate;
        public string[] __BeFitNames;
        public string[] __BeFitAuthors;
        public string[] __BeFitPaths;
        public Sprite[] __BeFitCovers;
        private IReadOnlyList<ListValues> StatsList = Plugin.Instance.UsersLoader.Users;

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            if (firstActivation) FirstActivation();


        }

        protected override void DidDeactivate(DeactivationType deactivationType)
        {

        }

        private void FirstActivation()
        {
            _tableCellTemplate = Resources.FindObjectsOfTypeAll<LevelListTableCell>().First(x => x.name == "LevelListTableCell");

            RectTransform container = new GameObject("AvatarsListContainer", typeof(RectTransform)).transform as RectTransform;
            container.SetParent(rectTransform, false);
            container.sizeDelta = new Vector2(70f, 0f);

            _tableView = new GameObject("AvatarsListTableView").AddComponent<TableView>();
            _tableView.gameObject.AddComponent<RectMask2D>();
            _tableView.transform.SetParent(container, false);

            (_tableView.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
            (_tableView.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
            (_tableView.transform as RectTransform).sizeDelta = new Vector2(0f, 60f);
            (_tableView.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f);

            _tableView.SetPrivateField("_preallocatedCells", new TableView.CellsGroup[0]);
            _tableView.SetPrivateField("_isInitialized", false);
            _tableView.dataSource = this;

            _tableView.didSelectRowEvent += _TableView_DidSelectRowEvent;

            _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), container, false);
            (_pageUpButton.transform as RectTransform).anchoredPosition = new Vector2(0f, 30f);
            _pageUpButton.interactable = true;
            _pageUpButton.onClick.AddListener(delegate ()
            {
                _tableView.PageScrollUp();
            });

            _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), container, false);
            (_pageDownButton.transform as RectTransform).anchoredPosition = new Vector2(0f, -30f);
            _pageDownButton.interactable = true;
            _pageDownButton.onClick.AddListener(delegate ()
            {
                _tableView.PageScrollDown();
            });

            _versionNumber = BeatSaberUI.CreateText(rectTransform, Plugin.Instance.Version, new Vector2(-10f, 10f));
            (_versionNumber.transform as RectTransform).anchorMax = new Vector2(1f, 0f);
            (_versionNumber.transform as RectTransform).anchorMin = new Vector2(1f, 0f);
            _versionNumber.fontSize = 5;
            _versionNumber.color = Color.white;

            try
            {
                if (_backButton == null)
                {
                    _backButton = BeatSaberUI.CreateBackButton(rectTransform as RectTransform);
                    _backButton.onClick.AddListener(delegate ()
                    {
                        onBackPressed();

                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void _TableView_DidSelectRowEvent(TableView sender, int row)
        {
            //Display user data
        }

        TableCell TableView.IDataSource.CellForRow(int row)
        {
            LevelListTableCell tableCell = _tableView.DequeueReusableCellForIdentifier("BeFitStatsCell") as LevelListTableCell;
            if (tableCell == null)
            {
                tableCell = Instantiate(_tableCellTemplate);
                tableCell.reuseIdentifier = "BeFitStatsCell";
            }
            try
            {
                tableCell.songName = __BeFitNames[row];
                tableCell.author = __BeFitAuthors[row];
                tableCell.coverImage = __BeFitCovers[row] ?? Sprite.Create(Texture2D.blackTexture, new Rect(), Vector2.zero);
            }
            catch (Exception e)
            {
                tableCell.songName = "Don goofd";
                tableCell.author = "Why do dis";
                tableCell.coverImage = null;
                Console.WriteLine(e);
            }
            return tableCell;

        }
        int TableView.IDataSource.NumberOfRows()
        {
            return StatsList.Count;
        }

        float TableView.IDataSource.RowHeight()
        {
            return 10f;
        }
    }
}

