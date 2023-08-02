using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Comsole
{
    class IniFile 
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section)
        {
            var RetVal = new StringBuilder(65535);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 65535, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
