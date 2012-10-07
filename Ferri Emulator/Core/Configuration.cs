using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ferri_Emulator.Core
{
    public class Configuration
    {
        string File;
        static Dictionary<string, object> Values = new Dictionary<string, object>();

        public T ReadValue<T>(string Key)
        {
            return (T)Values[Key];
        }

        public T PopValue<T>(string Key, out T value)
        {
            value = (T)Values[Key];
            return (T)value;
        }

        public void CreateFolder(string Folder)
        {
            Directory.CreateDirectory(Folder);
        }

        public void CreateFile(string File)
        {
            var Stream = System.IO.File.Create(File);
            Stream.Close(); // Make sure no errors
        }

        public void SetConfigurationFile(string File)
        {
            this.File = File;
        }

        public void AppendValues(string Key, object Value)
        {
            var Writer = new StreamWriter(File);
            Writer.WriteLine(Key + "=" + Value);
            Writer.Close();
        }

        public void ReadConfigurationFile()
        {
            using (var Reader = new StreamReader(File))
            {
                string Line = null;

                while ((Line = Reader.ReadLine()) != null)
                {
                    if (Line.StartsWith("#") || Line.StartsWith("//") || Line.Length < 1)
                        continue;

                    var Args = Line.Split('=');

                    Values.Add(Args[0], Args[1]);
                }
            }
        }
    }
}
