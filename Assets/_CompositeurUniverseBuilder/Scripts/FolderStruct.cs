///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.09.2019#
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public struct FolderStruct
    {
        public DirectoryInfo directory;
        public Button button;
        public FolderButton folderScript;
        public string path;
        public RawImage image;
        public GameObject folderInstance;
        public List<FileStruct> fileList;
        public MetaData metaData;
    }
}