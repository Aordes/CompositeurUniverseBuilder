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
        public static void FolderButton_OnSelected(Button button)
        {
            for (int i = 0; i < folderList.Count; i++)
            {
                if (folderList[i].button != button)
                {
                    folderList[i].folderScript.DeSelect();
                }
            }
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

            Debug.Log(file.path);
            File.Delete(file.path);
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
            string metaPath = Path.Combine(Path.GetDirectoryName(file.path) + "/" + Path.GetFileNameWithoutExtension(file.path) + "_meta.txt");

            currentMetaProperty = metaProperty;

            UpdateFileOrFolderMetaData(file.metaData, metaProperty, value, intValue);
            AddOrCreateMeta(metaPath, file.metaData);
        }

        public static void AddMetaToFolder(Button button, string metaProperty, bool value, int intValue)
        {
            FolderStruct folder = GetFolderStructFromFolderButton(button);
            string metaPath = Path.Combine(folder.directory.FullName + "_meta.txt");

            currentMetaProperty = metaProperty;

            UpdateFileOrFolderMetaData(folder.metaData, metaProperty, value, intValue);
            AddOrCreateMeta(metaPath, folder.metaData);
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
                    metaData.videoMute = value;
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

        private static void AddOrCreateMeta(string metaPath, MetaData metaData)
        {
            if (!File.Exists(metaPath))
            {
                CreateMetaFile(metaPath, metaData);
            }
            else
            {
                AddToExistingMetaFile(metaPath, metaData);
            }
        }

        private static void CreateMetaFile(string metaPath, MetaData metaData)
        {
            string metaLine = "";
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(metaPath))
            {
                if (metaData.desiredWidth)
                {
                    metaLine = MetaData.DESIRED_WIDTH + " " + metaData.desiredWidthValue + "%";
                    sw.WriteLine(metaLine);
                }
                if (metaData.showOnStart)
                {
                    metaLine = MetaData.SHOW_ON_START + " " + metaData.showOnStart;
                    sw.WriteLine(metaLine);
                }
                if (metaData.videoLoop)
                {
                    metaLine = MetaData.VDEO_LOOP + " " + metaData.videoLoop;
                    sw.WriteLine(metaLine);
                }
                if (!metaData.videoAutoplay)
                {
                    metaLine = MetaData.VDEO_AUTOPLAY + " " + metaData.videoAutoplay;
                    sw.WriteLine(metaLine);
                }
                if (metaData.videoMute)
                {
                    metaLine = MetaData.VDEO_MUTE + " " + metaData.videoMute;
                    sw.WriteLine(metaLine);
                }
            }
            if (metaLine == "") File.Delete(metaPath);
        }

        private static void AddToExistingMetaFile(string metaPath, MetaData metaData)
        {
            Debug.Log(metaData.videoMute);
            string metaLine = "";

            using (StreamWriter sw = new StreamWriter(metaPath, false))
            {
                if (metaData.desiredWidth)
                {
                    metaLine = MetaData.DESIRED_WIDTH + " " + metaData.desiredWidthValue + "%";
                    sw.WriteLine(metaLine);
                }
                if (metaData.showOnStart)
                {
                    metaLine = MetaData.SHOW_ON_START + " " + metaData.showOnStart;
                    sw.WriteLine(metaLine);
                }
                if (metaData.videoLoop)
                {
                    metaLine = MetaData.VDEO_LOOP + " " + metaData.videoLoop;
                    sw.WriteLine(metaLine);
                }
                if (!metaData.videoAutoplay)
                {
                    metaLine = MetaData.VDEO_AUTOPLAY + " " + metaData.videoAutoplay;
                    sw.WriteLine(metaLine);
                }
                if (metaData.videoMute)
                {
                    metaLine = MetaData.VDEO_MUTE + " " + metaData.videoMute;
                    sw.WriteLine(metaLine);
                }
            }
        }
        #endregion

        #region OnFolderSelected
        #endregion
    }
}