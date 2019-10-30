///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 18/10/2019 14:52
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
        public DirectoryInfo fileInfo;
    }

    public struct UniverseFileStruct
    {
        public Sprite preview;
        public string meta;
        public FileInfo fileInfo;
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
            public static string FILE = "file://";
            public static string META = "_meta";
            public static string PREVIEW = "_preview";
        }

        public static IEnumerator ImportUniverse(DirectoryInfo directory)
        {
            //Folders
            DirectoryInfo[] directories = directory.GetDirectories();
            Debug.Log("Project has " + directories.Length + " folders");
            yield return null;

            //Files
            FileInfo[] files = directory.GetFiles();
            Debug.Log(files.Length + " files");
            yield return null;

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
                        yield return ImportSprite(fileInfo, (output) => { output = universe.background;});
                        //yield return null;
                    }
                }
            }

            universe.folders = new List<UniverseFolderStruct>();
            for (int i = 0; i < directories.Length; i++)
            {
                yield return GetFolderStruct(directories[i], universe.folders);
            }

            Debug.Log(universe);
        }

        public static IEnumerator GetFolderStruct(DirectoryInfo folder, List<UniverseFolderStruct> list)
        {
            if (folder.Name.StartsWith(FileTypes.UNDERSCORE) || folder.Name.EndsWith(FileTypes.CONTENT_FOLDER))
                yield break;

            UniverseFolderStruct currentFolder;
            Debug.Log(folder.Name + " has " + folder.GetFiles().Length + " files \n" + folder.GetDirectories().Length + " folders");

            currentFolder = new UniverseFolderStruct
            {
                fileInfo = folder
            };

            yield return GetItemPreview(folder, FileTypes.BACKGROUND + FileTypes.ALL_EXTENSION, (output) => { currentFolder.icon = output; });
            yield return GetFileStruct(folder, currentFolder.files);

            currentFolder.subFolders = new List<UniverseFolderStruct>();
            DirectoryInfo[] directories = folder.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                yield return GetFolderStruct(directories[i], currentFolder.subFolders);
            }

            list.Add(currentFolder);
        }

        private static IEnumerator GetFileStruct(DirectoryInfo folder, List<UniverseFileStruct> universeFiles)
        {
            universeFiles = new List<UniverseFileStruct>();

            FileInfo[] files = folder.GetFiles();
            yield return null;

            UniverseFileStruct fileStruct;
            DirectoryInfo fileDirectory;
            string fileName;


            for (int i = 0; i < files.Length; i++)
            {
                fileName = files[i].Name;
                fileDirectory = files[i].Directory;

                if (!(fileName.Contains(FileTypes.BACKGROUND) || fileName.Contains(FileTypes.META)))
                {
                    fileStruct = new UniverseFileStruct
                    {
                        fileInfo = files[i],
                        meta = GetItemMeta(fileDirectory, fileName)
                    };

                    float t = Time.realtimeSinceStartup;
                    GetItemMeta(fileDirectory, fileName);
                    Debug.Log("Got meta in " + (Time.realtimeSinceStartup - t) + "s");

                    yield return GetItemPreview(fileDirectory, fileName, (output) => { fileStruct.preview = output; });

                    universeFiles.Add(fileStruct);
                }
            }
        }

        private static string GetItemMeta(DirectoryInfo directoryInfo, string fileName)
        {
            FileInfo[] files = directoryInfo.GetFiles(fileName);

            if (files.Length != 0)
            {
                return string.Empty;
            }


            return File.ReadAllText(files[0].FullName, Encoding.UTF8);
        }

        public static IEnumerator GetItemPreview(DirectoryInfo directoryInfo, string fileName, Action<Sprite> Callback)
        {
            FileInfo[] files = directoryInfo.GetFiles(fileName);

            if (files.Length == 0)
            {
                yield break;
            }

            Texture2D texture2D = null;
            yield return ImportTexture(files[0], (output) => { texture2D = output; });

            Callback?.Invoke(SquareSpriteFromTexture(texture2D));
        }

        public static bool CheckIsImage(string fileName)
        {
            //Debug.Log(Path.GetExtension(fileName));
            string extension = Path.GetExtension(fileName);
            return (extension == FileTypes.PNG || extension == FileTypes.JPG);
        }

        public static IEnumerator ImportTexture(FileInfo file, Action<Texture2D> Callback)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(FileTypes.FILE + file.FullName))
            {
                yield return request.SendWebRequest();

                if (string.IsNullOrEmpty(request.error))
                {
                    Callback?.Invoke(DownloadHandlerTexture.GetContent(request));
                }
                else
                {
                    Debug.LogError(request.error);
                    yield break;
                }
            }

            //byte[] data = File.ReadAllBytes(file.FullName);

            //Texture2D texture2D = new Texture2D(2, 2);
            //texture2D.LoadImage(data);

            //return texture2D;
        }

        public static IEnumerator ImportSprite(FileInfo file, Action<Sprite> Callback)
        {
            Texture2D texture2D = null;
            yield return ImportTexture(file, (output) => { texture2D = output; });

            Callback?.Invoke(Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero));
            Debug.Log("Imported " + file.Name);
        }


        #region Synchronous
        public static Sprite SquareSpriteFromTexture(Texture2D texture2D)
        {
            float minSize = Mathf.Min(texture2D.width, texture2D.height);
            return Sprite.Create(texture2D, new Rect((texture2D.width - minSize) / 2, (texture2D.height - minSize) / 2, minSize, minSize), Vector2.zero);
        }
        #endregion
    }
}