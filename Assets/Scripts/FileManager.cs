﻿///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.09.2019#
///-----------------------------------------------------------------

using SFB;
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
            Object.Destroy(folder.folderInstance);
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
            FileStruct file = GetFileStructFromFileButton(fileGrid.fileList, button);

            Directory.Delete(file.path);
            file.folderstruct.fileList.RemoveAt(file.folderstruct.fileList.IndexOf(file));
            Object.Destroy(file.instance);
        }

        public static void FileButton_RenameFile(string newName, Button button)
        {
            if (Directory.Exists(fileGrid.currentFolderStruct.directory.FullName + "/" + newName) || string.IsNullOrEmpty(newName)) return;

            FileStruct file = GetFileStructFromFileButton(fileGrid.fileList, button);
            Debug.Log(file.folderstruct.directory.FullName);
            file.folderstruct.directory.MoveTo(fileGrid.currentFolderStruct.directory.FullName + "/" + newName);
        }
        #endregion

        #region Utils
        private static FolderStruct GetFolderStructFromFolderButton(Button button)
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

        private static FileStruct GetFileStructFromFileButton(List<FileStruct> fileList, Button button)
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                if (fileList[i].button == button)
                {
                    return fileList[i];
                }
            }
            return fileList[0];
        }
        #endregion
    }
}