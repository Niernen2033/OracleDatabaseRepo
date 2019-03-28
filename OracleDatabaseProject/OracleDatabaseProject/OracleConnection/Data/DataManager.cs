using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OracleDatabaseProject
{
    static class DataManager
    {
        public static bool Save<T>(string path, List<T> data)
        {
            if (!Directory.Exists(Directory.GetParent(path).Parent.FullName))
            {
                return false;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        try
                        {
                            writer.WriteLine(data[i].ToString());
                        }
                        catch (Exception exc)
                        {
                            DebugManager.Instance.AddLog(exc.Message, null);
                            return false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                DebugManager.Instance.AddLog(exc.Message, null);
                return false;
            }
            return true;
        }

        public static bool Load(string path, out List<string> result)
        {
            result = new List<string>();
            if (!File.Exists(path))
            {            
                return false;
            }
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while(!reader.EndOfStream)
                    {
                        try
                        {
                            string line = reader.ReadLine();
                            result.Add(line);
                        }
                        catch(Exception exc)
                        {
                            DebugManager.Instance.AddLog(exc.Message, null);
                            return false;
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                DebugManager.Instance.AddLog(exc.Message, null);
                return false;
            }
            return true;
        }
    }
}
