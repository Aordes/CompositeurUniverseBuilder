///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 18/10/2019 14:52
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder {

    //public struct UniverseStruct
    //{
    //    public List<FolderStruct> folders;
    //    public List<>
    //    public UniverseStruct()
    //}

    public class FileImporter {

        private class FileTypes
        {
            public static string PNG = "png";
            public static string JPG = "jpg";
        }

        private static string[] EXTENSION_SPLIT = new string[] { "." };

        public static void ImportUniverse(DirectoryInfo directory)
        {
            DirectoryInfo[] directories = directory.GetDirectories();
            Debug.Log("Project has " + directories.Length + " folders");

            FileInfo[] files = directory.GetFiles();
            Debug.Log(files.Length + " files");
             
            FileInfo fileInfo;
            for (int i = 0; i < files.Length; i++)
            {
                fileInfo = files[i];
                Debug.Log(files[i].FullName + "\n" + CheckIsImage(fileInfo.Name));
            }
        }

        public static bool CheckIsImage(string fileName)
        {
            string[] fileSplit = fileName.Split(EXTENSION_SPLIT, System.StringSplitOptions.RemoveEmptyEntries);
            string extension = fileSplit[fileSplit.Length - 1];

            return extension == FileTypes.PNG || extension == FileTypes.JPG;
        }

        public static Texture2D ImportImage(FileInfo file)
        {
            byte[] data = File.ReadAllBytes(file.FullName);

            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(data);

            return texture2D;
        }

        public static Sprite CreateSquareSprite(Texture2D texture2D)
        {
            float minSize = Mathf.Min(texture2D.width, texture2D.height);
            return Sprite.Create(texture2D, new Rect((texture2D.width - minSize) / 2, (texture2D.height - minSize) / 2, minSize, minSize), Vector2.zero);
        }
    }
}