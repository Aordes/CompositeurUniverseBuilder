///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 18/10/2019 14:52
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public struct UniverseStruct
    {
        public Sprite background;
        public List<UniverseFolderStruct> folders;
        public List<UniverseFileStruct> files;
    }

    public struct UniverseFolderStruct
    {
        public Sprite icon;
        public List<UniverseFolderStruct> subFolders;
        public List<UniverseFileStruct> files;
    }

    public struct UniverseFileStruct
    {
        public Sprite preview;
        public FileInfo meta;
        public FileInfo file;
    }

    public class FileImporter {

        private class FileTypes
        {
            public static string CONTENT_FOLDER = ".content";
            public static string JPG = ".jpg";
            public static string PNG = ".png";
            public static string TXT = ".txt";
            public static string ALL_EXTENSION = ".*";

            public static string UNDERSCORE = "_";
            public static string BACKGROUND = "_background";
            public static string FOLDER_ICON = "_icon";
            public static string META = "_meta";
            public static string PREVIEW = "_preview";
        }

        public static void ImportUniverse(DirectoryInfo directory)
        {
            //Folders
            DirectoryInfo[] directories = directory.GetDirectories();
            Debug.Log("Project has " + directories.Length + " folders");

            //Files
            FileInfo[] files = directory.GetFiles();
            Debug.Log(files.Length + " files");

            UniverseStruct universe = new UniverseStruct();
             
            FileInfo fileInfo;
            for (int i = 0; i < files.Length; i++)
            {
                fileInfo = files[i];
                Debug.Log(fileInfo.FullName + "\n" + CheckIsImage(fileInfo.Name));
                if (CheckIsImage(fileInfo.Name))
                {
                    if (fileInfo.Name.Contains(FileTypes.BACKGROUND))
                    {
                        universe.background = ImportSprite(fileInfo);
                    }
                }
            }

            universe.folders = new List<UniverseFolderStruct>();
            for (int i = 0; i < directories.Length; i++)
            {
                GetUniverseFolderStruct(directories[i], universe.folders);
            }

        }

        public static void GetUniverseFolderStruct(DirectoryInfo folder, List<UniverseFolderStruct> list)
        {
            if (folder.Name.StartsWith(FileTypes.UNDERSCORE) || folder.Name.EndsWith(FileTypes.CONTENT_FOLDER))
                return;

            UniverseFolderStruct currentFolder;
            Debug.Log(folder.Name + " has " + folder.GetFiles().Length + " files \n" + folder.GetDirectories().Length);

            currentFolder = new UniverseFolderStruct();
            currentFolder.icon = GetItemPreview(folder, FileTypes.FOLDER_ICON + FileTypes.ALL_EXTENSION);

            currentFolder.files = new List<UniverseFileStruct>();
            FileInfo[] files = folder.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (!(files[i].Name.Contains(FileTypes.FOLDER_ICON) || files[i].Name.Contains(FileTypes.META)))
                    currentFolder.files.Add(
                        new UniverseFileStruct()
                        {
                            file = files[i],
                            preview = GetItemPreview(files[i].Directory, files[i].Name)
                        }
                    );
            }

            currentFolder.subFolders = new List<UniverseFolderStruct>();
            DirectoryInfo[] directories = folder.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                GetUniverseFolderStruct(directories[i], currentFolder.subFolders);
            }

            list.Add(currentFolder);
        }

        public static Sprite GetItemPreview(DirectoryInfo directoryInfo, string fileName)
        {
            FileInfo[] files = directoryInfo.GetFiles(fileName);

            Sprite image;

            if (files.Length != 0)
            {
                image = SquareSpriteFromTexture(ImportImage(files[0]));
                return image;
            }

            return null;
        }



        public static bool CheckIsImage(string fileName)
        {
            //Debug.Log(Path.GetExtension(fileName));
            string extension = Path.GetExtension(fileName);
            return (extension == FileTypes.PNG || extension == FileTypes.JPG);
        }

        public static Texture2D ImportImage(FileInfo file)
        {
            byte[] data = File.ReadAllBytes(file.FullName);

            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(data);

            return texture2D;
        }

        public static Sprite ImportSprite(FileInfo file)
        {
            Texture2D texture2D = ImportImage(file);
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        }

        public static Sprite SquareSpriteFromTexture(Texture2D texture2D)
        {
            float minSize = Mathf.Min(texture2D.width, texture2D.height);
            return Sprite.Create(texture2D, new Rect((texture2D.width - minSize) / 2, (texture2D.height - minSize) / 2, minSize, minSize), Vector2.zero);
        }
    }
}