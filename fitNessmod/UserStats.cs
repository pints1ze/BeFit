using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeFitMod
{
    public class UserStats
    {
        private float? _height;
        internal UserStats(string fullPath)
        {
            fullPath = fullPath;

        }
        public string Name //UserName
        {
            get
            {
                return _userStats.user.userName;
            }
            set
            {
                ///Open Keypad and input, sets after backbutton pressed
            }
        }
        public string AuthorName
        {
            get
            {
                return _userStats.user.goals;
            }
            set
            {
                ///Open Keypad and input, sets after backbutton pressed
            }
        }
        public Sprite CoverImage
        {
            get
            {
                return _userStats.user.icon;
            }
        }
        public Transform ViewPoint
        {
            get
            {
                return _userStats.user.viewPoint;
            }
        }
        public float Height
        {
            get
            {
                return _height.Value;
            }
        }
        public bool AllowHeightChange
        {
            get
            {
                return _userStats.user.AllowHeightChange;
            }
        }
        public bool IsLoaded
        {
            get { return _userStats.UserStats != null; }
        }
        public
    }
}
