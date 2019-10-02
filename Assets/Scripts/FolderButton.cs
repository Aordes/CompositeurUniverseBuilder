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
    public class FolderButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
    {
        #region Fields
        [SerializeField] private InputField inputField;
        [SerializeField] private Text text;
        [SerializeField] private Button openButton;
        [SerializeField] private Button changePreviewButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private GameObject controlPanel;

        public delegate void OnEndEditFolderNameDelegate(string name, Button button);
        public OnEndEditFolderNameDelegate onEndEditFolderName;

        public delegate void OnClickDelegate(Button button);
        public OnClickDelegate onChangePreview;
        public OnClickDelegate onChangeDirectoryContent;
        public OnClickDelegate onDeleteDirectory;

        private Button button;

        private readonly int leftClick = 0;
        private readonly int rightClick = 1;
        #endregion

        #region Unity Methods
        void Awake()
        {
            controlPanel.SetActive(false);

            button = gameObject.GetComponent<Button>();

            openButton.onClick.AddListener(OnClick_ChangeDirectoryContent);
            changePreviewButton.onClick.AddListener(OnClick_ChangePreview);
            deleteButton.onClick.AddListener(OnClick_DeleteButton);

            inputField.onEndEdit.AddListener(delegate
            {
                OnEnd_EditName(inputField.text);
            });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButtonDown(leftClick))
            {
                controlPanel.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
            for (int i = 0; i < raycastResultList.Count; i++)
            {
                if (!raycastResultList[i].gameObject == controlPanel && Input.GetMouseButtonDown(leftClick))
                {
                    Debug.Log("Helllo");
                    controlPanel.SetActive(false);
                }
            }
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
    }
}
