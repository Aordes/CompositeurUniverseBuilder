///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #DATE#
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
        #endregion

        #region Action Methods
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
            File.Copy(path[0], folder.directory.FullName + "/" + Path.GetFileName(path[0]));
        }

        public static void FolderButton_OnDeleteDirectory(Button button)
        {
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
        #endregion
    }
}