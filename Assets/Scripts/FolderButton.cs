﻿///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #02.09.2019#
///-----------------------------------------------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Com.Docaret.UniverseBuilder
{
    public class FolderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields
        [SerializeField] private InputField inputField;
        [SerializeField] private Text text;
        [SerializeField] private Button openButton;
        [SerializeField] private Button changePreviewButton;
        [SerializeField] private Button deleteButton;

        public GameObject fileContainer;
        public ToolBar toolbar;
        public delegate void OnEndEditFolderNameDelegate(string name, Button button);
        public OnEndEditFolderNameDelegate onEndEditFolderName;

        public delegate void OnClickDelegate(Button button);
        public OnClickDelegate onChangePreview;
        public OnClickDelegate onChangeDirectoryContent;
        public OnClickDelegate onDeleteDirectory;

        private Button button;
        private bool isControlPanelOpen;
        private bool isMouseOverUi;

        private readonly int leftClick = 0;
        private readonly int rightClick = 1;
        #endregion

        #region Unity Methods
        void Awake()
        {
            fileContainer.SetActive(false);

            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(OnSelected);
        }

        private void Start()
        {
            fileContainer.GetComponent<DynamicGrid>().toolBar = toolbar;
        }
        #endregion


        #region Pointer Methods
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isControlPanelOpen)
            {
                StartCoroutine(MouseOver());
            }
        }

        private void OnSelected()
        {
            toolbar.CurrentSelection = button;
            toolbar.CurrentFolder = button;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isControlPanelOpen)
            {
                StartCoroutine(MouseExit());
            }
        }
        #endregion

        #region Coroutine
        private IEnumerator MouseOver()
        {
            while (!Input.GetMouseButtonDown(leftClick) || !EventSystem.current.IsPointerOverGameObject())
            {
                yield return null;
            }
            OpenControlPanel();
        }

        private IEnumerator MouseExit()
        {
            while (!Input.GetMouseButtonDown(leftClick) || EventSystem.current.IsPointerOverGameObject())
            {
                yield return null;
            }
            CloseControlPanel();
        }
        #endregion

        #region CallBack Methods
        private void OnEnd_EditName(string name)
        {
            onEndEditFolderName?.Invoke(name, button);          
        }

        private void OnClick_DeleteButton()
        {
            onDeleteDirectory?.Invoke(button);
        }

        private void OnClick_ChangePreview()
        {
            onChangePreview?.Invoke(button);
        }

        private void OnClick_ChangeDirectoryContent()
        {    
            onChangeDirectoryContent?.Invoke(button);
        }
        #endregion

        public void OpenControlPanel()
        {
            StopCoroutine(MouseOver());
            fileContainer.SetActive(true);
            isControlPanelOpen = true;
            FileManager.fileGrid = fileContainer.GetComponent<DynamicGrid>();
        }

        public void CloseControlPanel()
        {
            StopCoroutine(MouseExit());
            fileContainer.SetActive(false);
            isControlPanelOpen = false;
            toolbar.CurrentFolder = null;
            toolbar.CurrentSelection = null;
        }
    }
}
