using System.ComponentModel;
using System.IO;
using System.Text;
using UnityEngine;

namespace Doryu.JBSave
{
    public static class JBSaver
    {
        //Set path
        //C:/../AppData/LocalLow/(ConpanyName)/(ProjectName)/SaveDatas/bins/
        static private string _localPath = Path.Combine(Application.persistentDataPath, "SaveDatas/bins/");

        private static UnicodeEncoding _ByteConverter = new UnicodeEncoding();
        //TogleCrypto Keys (1 ~ 255)
        private static byte[] _keyNums = { 61, 162, 54, 74, 1, 32, 69, 4, 5, 134 };

        /// <summary>
        /// Load the class by its name.
        /// </summary>
        public static bool LoadJson<T>(this T loadClass, string saveFileName) where T : ISavable<T>
        {
            string path = _localPath + saveFileName + ".bin";
            if (File.Exists(path))
            {
                //Byte decryption
                byte[] bytes = File.ReadAllBytes(path).TogleCrypto(false);
                //Byte to json
                string json = string.Concat(_ByteConverter.GetChars(bytes));
                //Json to class
                T res = JsonUtility.FromJson<T>(json);
                loadClass.OnLoadData(res);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Save the class with that name.
        /// </summary>
        public static void SaveJson<T>(this T saveClass, string saveFileName) where T : ISavable<T>
        {
            if (Directory.Exists(_localPath) == false)
            {
                Directory.CreateDirectory(_localPath);
            }
            //Save path
            string path = _localPath + saveFileName + ".bin";
            //Class to json
            string jsonData = JsonUtility.ToJson(saveClass, true);
            //Json to byte and encryption
            byte[] bytes = _ByteConverter.GetBytes(jsonData).TogleCrypto(true);
            saveClass.OnSaveData(saveFileName);

            File.WriteAllBytes(path, bytes);
        }

        public static byte[] TogleCrypto(this byte[] data, bool isEncrypt)
        {
            byte[] changedData = data;

            for (int i = 0; i < data.Length; i++)
            {
                if (isEncrypt) changedData[i] ^= _keyNums[i % _keyNums.Length];
                changedData[i] = (byte)((byte)(changedData[i] << 4) + (byte)(changedData[i] >> 4));
                if (isEncrypt == false) changedData[i] ^= _keyNums[i % _keyNums.Length];
            }

            return changedData;
        }
    }
}