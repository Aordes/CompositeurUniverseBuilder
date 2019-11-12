///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.10.2019#
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {
	public class DynamicGrid : MonoBehaviour
    {
        [SerializeField] private GameObject filePrefab;
        [SerializeField] private RectTransform rectTransform;

        public ToolBar toolBar;
        public FolderStruct currentFolderStruct;
        public List<FileStruct> fileList = new List<FileStruct>();

        private GridLayoutGroup gridLayout;

        private int minXCellSize = 80;
        private int minYCellSize = 20;

  //      private void Awake ()
  //      {
  //          gridLayout = gameObject.GetComponent<GridLayoutGroup>();
  //          rectTransform = gameObject.GetComponent<RectTransform>();
		//}

        public void CreateFile(string path)
        {
            GameObject instance = Instantiate(filePrefab, rectTransform);
            FileStruct fileStruct = new FileStruct();

            fileStruct.instance = instance;
            fileStruct.fileScript = instance.GetComponent<FileSelection>();
            fileStruct.fileScript.toolBar = toolBar;
            fileStruct.fileScript.SetName(Path.GetFileNameWithoutExtension(path));
            fileStruct.folderstruct = currentFolderStruct;
            fileStruct.button = instance.GetComponent<Button>();
            fileStruct.path = path;
            fileStruct.metaData = new MetaData();
            Debug.Log(fileStruct.fileScript.toolBar);

            fileList.Add(fileStruct);
            SetGridSize();
        }

        private void SetGridSize()
        {
            //Vector2 cellSize = new Vector2(minXCellSize, minYCellSize);
            //if (fileList.Count % 4 == 0)
            //{
            //    gridLayout.cellSize = new Vector2(rectTransform.rect.width / fileList.Count, rectTransform.rect.height / fileList.Count);   
            //}
            //gridLayout.constraintCount = 4
        }
		
	}
}