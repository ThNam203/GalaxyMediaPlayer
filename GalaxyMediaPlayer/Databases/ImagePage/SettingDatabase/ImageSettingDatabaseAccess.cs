using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GalaxyMediaPlayer.Databases.ImagePage.SettingDatabase
{
    internal class ImageSettingDatabaseAccess
    {

        private static readonly string DATABASE_PATH = AppDomain.CurrentDomain.BaseDirectory + "Databases\\ImagePage\\SettingDatabase\\Database.txt";

        public static string[] GetAllData()
        {
            CreateDatabaseFileIfNotExist();
            string[] lines = File.ReadAllLines(DATABASE_PATH);
            return lines;
        }

        public static void SaveFolderToDatabase(string newFolderPath)
        {
            CreateDatabaseFileIfNotExist();

            // Nam: check if it existed, if yes then exit, not saving
            List<string> folders = File.ReadAllLines(DATABASE_PATH).ToList();
            for (int i = 0; i < folders.Count; i++)
            {
                // Nam: if newFolder is the same or is child of another path in db, we cancel
                if (folders[i] == newFolderPath || IsParentFolder(folders[i], newFolderPath)) return;

                // Nam: if newFolder is parent of an existed folder
                if (folders[i].StartsWith(newFolderPath))
                {
                    folders.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            folders.Add(newFolderPath);
            File.WriteAllLines(DATABASE_PATH, folders.ToArray());
        }

        public static void RemoveFolder(string deletePath)
        {
            string[] paths = File.ReadAllLines(DATABASE_PATH);
            List<string> tempFile = new List<string>();

            foreach (string path in paths)
            {
                if (path == deletePath)
                {
                    continue;
                }

                tempFile.Add(path);
            }

            CreateDatabaseFileIfNotExist();
            File.WriteAllLines(DATABASE_PATH, tempFile.ToArray());
        }

        private static bool IsParentFolder(string parentFolder, string childFolder)
        {
            DirectoryInfo di1 = new DirectoryInfo(parentFolder);
            DirectoryInfo di2 = new DirectoryInfo(childFolder);
            bool isParent = false;

            while (di2.Parent != null)
            {
                if (di2.Parent.FullName == di1.FullName)
                {
                    isParent = true;
                    break;
                }
                else di2 = di2.Parent;
            }

            return isParent;
        }

        private static void CreateDatabaseFileIfNotExist()
        {
            if (!File.Exists(DATABASE_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DATABASE_PATH));

                FileStream fs = File.Create(DATABASE_PATH);
                fs.Close();
            }
        }
    }
}
