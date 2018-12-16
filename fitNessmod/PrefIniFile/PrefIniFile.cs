using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeFitMod
{
    internal class PrefIniFile
    {
        [DllImport("KERNEL32.DLL", CallingConvention =
            CallingConvention.StdCall, CharSet = CharSet.Unicode,
            EntryPoint = "GetPrivateProfileStringW", ExactSpelling = true,
            SetLastError = true)]
        private static extern int GetPrivateProfileString(string
            lpSection, string lpKey, string lpDefault, StringBuilder
            lpReturnString, int nSize, string lpFileName);

        [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WritePrivateProfileStringW", ExactSpelling = true, SetLastError = true)]
        private static extern int WritePrivateProfileString(string lpSection, string lpKey, string lpValue, string lpFileName);

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                if (!File.Exists(value))
                {
                    File.WriteAllText(value, "", Encoding.Unicode);
                }
                this._path = value;
            }
        }

        public PrefIniFile(string INIPath)
        {
            this.Path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            PrefIniFile.WritePrivateProfileString(Section, Key, Value, this.Path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder stringBuilder = new StringBuilder(1023);
            PrefIniFile.GetPrivateProfileString(Section, Key, "", stringBuilder, 1023, this.Path);
            return stringBuilder.ToString();
        }

        private string _path = "";
    }
}
