﻿///-----------------------------------------------------------------
/// Author : #Adrien Bordes#
/// Date : #01.10.2019#
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{
	public class DynamicGrid : MonoBehaviour
    {
        [SerializeField] private GameObject filePrefab;

        private GridLayoutGroup gridLayout;
        private RectTransform rectTransform;

        private int minXCellSize = 80;
        private int minYCellSize = 20;
        private List<FileStruct> fileList = new List<FileStruct>();

        private void Awake ()
        {
            gridLayout = gameObject.GetComponent<GridLayoutGroup>();
            rectTransform = gameObject.GetComponent<RectTransform>();
		}

        public void CreateFile()
        {
            GameObject instance = Instantiate(filePrefab, gameObject.transform);
            FileStruct fileStruct = new FileStruct();
            fileStruct.instance = instance;
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