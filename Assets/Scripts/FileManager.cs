///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.09.2019#
///-----------------------------------------------------------------

using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder {
	public class FileManager {

        #region Fields
        public static ExtensionFilter[] supportedImageExtantions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        public static List<FolderStruct> folderList = new List<FolderStruct>();
        public static DynamicGrid fileGrid;

        private static string currentMetaProperty;
        
        #endregion

        #region Folder Methods
        public static void FolderButton_OnChangePreview(Button button)
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select a Preview", "", supportedImageExtantions, false);

            if (path.Length == 0) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);
            string newPath = folder.directory.FullName + "/_preview.png";

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path[0], newPath);

            WWW www = new WWW(newPath);
            folder.image.texture = www.texture;
        }

        public static void FolderButton_OnChangeDirectoryContent(Button button)
        {
            string[] path = StandaloneFileBrowser.OpenFilePanel("Select Documents", "", "", false);
            if (path.Length == 0) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);

            for (int i = 0; i < path.Length; i++)
            {
                File.Copy(path[i], folder.directory.FullName + "/" + Path.GetFileName(path[i]));
                CreateFile(folder.directory.FullName + "/" + Path.GetFileName(path[i]));
            }
        }

        public static void FolderButton_OnDeleteDirectory(Button button)
        {
            Debug.Log("Delete Directory");
            FolderStruct folder = GetFolderStructFromFolderButton(button);

            folder.directory.Delete(true);
            folderList.RemoveAt(folderList.IndexOf(folder));
            UnityEngine.Object.Destroy(folder.folderInstance);
        }

        public static void FolderButton_RenameFolder(string newName, Button button)
        {
            if (Directory.Exists(DirectoryData.CurrentUniversePath + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            FolderStruct folder = GetFolderStructFromFolderButton(button);
            folder.directory.MoveTo(DirectoryData.CurrentUniversePath + "/" + newName);
        }
        #endregion

        #region File Methods
        public static void CreateFile(string path)
        {
            fileGrid.CreateFile(path);
        }

        public static void DeleteFileDirectory(Button button)
        {
            Debug.Log("Delete File");
            FileStruct file = GetFileStructFromFileButton(button);

            Directory.Delete(file.path);
            file.folderstruct.fileList.RemoveAt(file.folderstruct.fileList.IndexOf(file));
            UnityEngine.Object.Destroy(file.instance);
        }

        public static void FileButton_RenameFile(string newName, Button button)
        {
            if (Directory.Exists(fileGrid.currentFolderStruct.directory.FullName + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            FileStruct file = GetFileStructFromFileButton(button);
            Debug.Log(file.folderstruct.directory.FullName);
            file.folderstruct.directory.MoveTo(fileGrid.currentFolderStruct.directory.FullName + "/" + newName);
        }
        #endregion

        #region Meta
        public static void ChangeMetaData(bool isFile, Button button, string metaProperty, bool value, int intValue = 0)
        {
            if (isFile)
            {
                AddMetaToFile(button, metaProperty, value, intValue);
            }
            else
            {
                AddMetaToFolder(button, metaProperty, value, intValue);
            }
        }

        public static void AddMetaToFile(Button button, string metaProperty, bool value, int intValue)
        {
            FileStruct file = GetFileStructFromFileButton(button);
            string metaPath = Path.Combine(file.path + "_meta.txt");

            currentMetaProperty = metaProperty;

            if (intValue == 0) metaProperty += " " + value;
            else metaProperty += " " + intValue + "%";

            UpdateFileOrFolderMetaData(file.metaData, metaProperty, value, intValue);
            AddOrCreateMeta(metaProperty, metaPath);

        }

        public static void AddMetaToFolder(Button button, string metaProperty, bool value, int intValue)
        {
            FolderStruct folder = GetFolderStructFromFolderButton(button);
            string metaPath = Path.Combine(folder.directory.FullName + "_meta.txt");

            currentMetaProperty = metaProperty;

            if (intValue == 0) metaProperty += " " + value;
            else metaProperty += " " + intValue + "%";

            UpdateFileOrFolderMetaData(folder.metaData, metaProperty, value, intValue);
            AddOrCreateMeta(metaProperty, metaPath);
        }

        private static void UpdateFileOrFolderMetaData(MetaData metaData, string metaProperty, bool value, int intValue = 0)
        {
            switch (metaProperty)
            {
                case MetaData.DESIRED_WIDTH:
                    metaData.desiredWidth = value;
                    metaData.desiredWidthValue = intValue;
                    break;
                case MetaData.SHOW_ON_START:
                    metaData.showOnStart = value;
                    break;
                case MetaData.VDEO_LOOP:
                    metaData.videoLoop = value;
                    break;
                case MetaData.VDEO_AUTOPLAY:
                    metaData.videoAutoplay = value;
                    break;
                case MetaData.VDEO_MUTE:
                    metaData.videoAutoplay = value;
                    break;
            }
        }
        #endregion

        #region Utils
        public static FolderStruct GetFolderStructFromFolderButton(Button button)
        {
            for (int i = 0; i < folderList.Count; i++)
            {
                if (folderList[i].button == button)
                {
                    return folderList[i];
                }
            }
            return folderList[0];
        }

        public static FileStruct GetFileStructFromFileButton(Button button)
        {
            for (int i = 0; i < fileGrid.fileList.Count; i++)
            {
                if (fileGrid.fileList[i].button == button)
                {
                    return fileGrid.fileList[i];
                }
            }
            return fileGrid.fileList[0];
        }

        private static void AddOrCreateMeta(string meta, string metaPath)
        {
            if (!File.Exists(metaPath))
            {
                CreateMetaFile(meta, metaPath);
            }
            else
            {
                AddToExistingMetaFile(meta, metaPath);
            }
        }

        private static void CreateMetaFile(string meta, string metaPath)
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(metaPath))
            {
                sw.WriteLine(meta);
            }
        }

        private static void AddToExistingMetaFile(string meta, string metaPath)
        {
            // Open the file to read from.
            using (StreamReader sr = File.OpenText(metaPath))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s == meta) return;
                }

                using (StreamWriter sw = new StreamWriter(metaPath))
                {
                    sw.WriteLine(meta);
                }
            }
        }
        #endregion
    }
}