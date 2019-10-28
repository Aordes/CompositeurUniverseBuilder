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
        public List<FolderStruct> folders;
        public List<FileStruct> files;
    }

    public struct FolderStruct
    {
        public Sprite icon;
        public List<FolderStruct> subFolders;
        public List<FileStruct> files;
    }

    public struct FileStruct
    {
        public Sprite preview;
        public FileInfo meta;
        public FileInfo file;
    }

    public class FileImporter {

        private class FileTypes
        {
            public static string PNG = ".png";
            public static string JPG = ".jpg";
            public static string PREVIEW = "_preview";
            public static string BACKGROUND = "_background";
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

            universe.folders = new List<FolderStruct>();
            DirectoryInfo folder;
            for (int i = 0; i < directories.Length; i++)
            {
                GetFolderStruct(directories[i], universe.folders);
            }
        }

        public static void GetFolderStruct(DirectoryInfo folder, List<FolderStruct> list)
        {
            FolderStruct currentFolder;
            Debug.Log(folder.Name + " has " + folder.GetFiles().Length + " files \n" + folder.GetDirectories().Length);

            currentFolder = new FolderStruct();
            currentFolder.files = new List<FileStruct>();

            currentFolder.subFolders = new List<FolderStruct>();
            DirectoryInfo[] directories = folder.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                GetFolderStruct(directories[i], currentFolder.subFolders);
            }
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