using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OracleDatabaseProject
{
    static class XmlManager
    {
        //Functions***************************
        public static bool Load<T>(string name, out T instance, bool IsCryptoOn = false)
        {
            try
            {
                using (FileStream fileStream = File.Open(name, FileMode.Open))
                {
                    if (IsCryptoOn)
                    {
                        ICryptoTransform key = GetCryptoTransform(false);
                        if (key == null)
                        {
                            instance = default(T);
                            return false;
                        }

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, key, CryptoStreamMode.Read))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(T));
                            instance = (T)xml.Deserialize(cryptoStream);

                        }
                        return true;
                    }
                    else
                    {
                        using (TextReader textReader = new StreamReader(fileStream))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(T));
                            instance = (T)xml.Deserialize(textReader);
                        }
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                DebugManager.Instance.AddLog("Class 'XmlManager' in 'Load' function:" + exc.ToString(), null);
                instance = default(T);
                return false;
            }
        }

        public static bool Save<T>(T obj, string name, bool IsCryptoOn = false)
        {
            try
            {
                using (FileStream fileStream = File.Open(name, FileMode.Create))
                {
                    if (IsCryptoOn)
                    {
                        ICryptoTransform key = GetCryptoTransform(true);
                        if (key == null)
                        {
                            return false;
                        }

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, key, CryptoStreamMode.Write))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(T));
                            xml.Serialize(cryptoStream, obj);

                        }
                        return true;
                    }
                    else
                    {
                        using (TextWriter textWriter = new StreamWriter(fileStream))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(T));
                            xml.Serialize(textWriter, obj);
                        }
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                DebugManager.Instance.AddLog("Class 'XmlManager' in 'Save' function:" + exc.ToString(), null);
                return false;
            }
        }

        private static ICryptoTransform GetCryptoTransform(bool encrypt)
        {
            ICryptoTransform key = null;
            byte[] sha512 = new byte[64];
            string keyData = "THISISDEFAULTKEYFORCRYPTOTRANSFORM";
            try
            {
                using (SHA512CryptoServiceProvider sha512CSP = new SHA512CryptoServiceProvider())
                {
                    sha512 = sha512CSP.ComputeHash(Encoding.ASCII.GetBytes(keyData));
                }
            }
            catch (Exception exc)
            {
                DebugManager.Instance.AddLog("Class 'XmlManager' in 'GetCryptoTransform' function: " + exc.ToString(), null);
                return null;
            }

            byte[] shaKey = new byte[32];
            byte[] shaIv = new byte[16];
            for (int i = 0; i < 32; i++)
            {
                if (i < 16)
                {
                    shaIv[i] = sha512[i * 3];
                }
                shaKey[i] = sha512[i * 2];
            }

            try
            {
                using (AesCryptoServiceProvider aesCSP = new AesCryptoServiceProvider())
                {
                    aesCSP.Mode = CipherMode.CBC;
                    if (encrypt)
                    {
                        key = aesCSP.CreateEncryptor(shaKey, shaIv);
                    }
                    else
                    {
                        key = aesCSP.CreateDecryptor(shaKey, shaIv);
                    }
                }
            }
            catch (Exception exc)
            {
                DebugManager.Instance.AddLog("Class 'XmlManager' in 'GetCryptoTransform' function: " + exc.ToString(), null);
                return null;
            }

            return key;
        }
    }
}
