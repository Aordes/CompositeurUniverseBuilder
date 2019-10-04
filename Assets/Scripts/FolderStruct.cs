///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #DATE#
///-----------------------------------------------------------------

using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{
    public struct FolderStruct
    {
        public DirectoryInfo directory;
        public Button button;
        public FolderButton folderScript;
        public string path;
        public RawImage image;
        public GameObject folderInstance;
    }
}