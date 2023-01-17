using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Class responsible for serializing save game obbject into binary
    /// </summary>
    public class SerializationManager
    {
        /// <summary>
        /// Saves object into save file by overriding it
        /// </summary>
        /// <param name="saveName">
        /// Save file name
        /// </param>
        /// <param name="saveData">
        /// Object with save data
        /// </param>
        /// <returns>
        /// True if succided, false if savve havent succeded
        /// </returns>
        public static bool Save(string saveName, object saveData)
        {
            try
            {
                BinaryFormatter formatter = GetBinaryFromatter();

                if (!Directory.Exists(Application.persistentDataPath + "/saves"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/saves");
                }

                string path = $"{Application.persistentDataPath}/saves/{saveName}.save";

                FileStream file = File.Create(path);

                formatter.Serialize(file, saveData);

                file.Close();

                return true;
            }
            catch
            {
                return false;
            }


        }

        /// <summary>
        /// Load game file from file
        /// </summary>
        /// <param name="path">
        /// Path to save file
        /// </param>
        /// <returns>
        /// Game state object
        /// </returns>
        public static object Load(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = GetBinaryFromatter();

            FileStream file = File.Open(path, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {
                Debug.LogErrorFormat("Failed to load file at {0}", path);
                file.Close();
                return null;
            }
        }

        /// <summary>
        /// Provides binary formatters with all surogates
        /// </summary>
        /// <returns>
        /// Sutable to creating save file binary formatter
        /// </returns>
        public static BinaryFormatter GetBinaryFromatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter;
        }
    }
}
