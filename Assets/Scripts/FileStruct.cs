///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.10.2019#
///-----------------------------------------------------------------

using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public struct FileStruct
    {
        public GameObject instance;
        public Button button;
        public Image image;
        public string path;
        public FolderStruct folderstruct;
        public MetaData metaData;
        public FileSelection fileScript;
    }
}