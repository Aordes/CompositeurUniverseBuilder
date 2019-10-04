///-----------------------------------------------------------------
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
        [SerializeField] private GameObject fileContainer;

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
            button.onClick.AddListener(AddFolder);

            //openButton.onClick.AddListener(OnClick_ChangeDirectoryContent);
            //changePreviewButton.onClick.AddListener(OnClick_ChangePreview);
            //deleteButton.onClick.AddListener(OnClick_DeleteButton);

            //inputField.onEndEdit.AddListener(delegate
            //{
            //    OnEnd_EditName(inputField.text);
            //});
        }

        public void AddFolder()
        {
            FileManager.CreateFile();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOverUi = true;
            if (!isControlPanelOpen)
            {
                StartCoroutine(MouseOver());
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOverUi = false;
            if (isControlPanelOpen)
            {
                StartCoroutine(MouseExit());
            }
        }
        #endregion

        
        private IEnumerator MouseOver()
        {
            while (!Input.GetMouseButtonDown(leftClick) || !isMouseOverUi)
            {
                Debug.Log(Input.GetMouseButtonDown(leftClick));
                yield return null;
            }
            fileContainer.SetActive(true);
            isControlPanelOpen = true;
            FileManager.fileGrid = fileContainer.GetComponent<DynamicGrid>();
            StopCoroutine(MouseOver());
        }

        private IEnumerator MouseExit()
        {
            while (!Input.GetMouseButtonDown(leftClick) || isMouseOverUi)
            {
                yield return null;
            }
            fileContainer.SetActive(false);
            isControlPanelOpen = false;
            StopCoroutine(MouseExit());
        }

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
    }
}
