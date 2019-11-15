///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #02.09.2019#
///-----------------------------------------------------------------

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class FolderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI folderName;
        [SerializeField] private Button openButton;
        [SerializeField] private Button changePreviewButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private OpenCloseAnimator openCloseAnimator;
        [SerializeField] private GameObject outline;
        [SerializeField] private GameObject containerOutline;

        public GameObject fileContainer;
        public RawImage image;
        public ToolBar toolbar;
        public delegate void OnEndEditFolderNameDelegate(string name, Button button);
        public OnEndEditFolderNameDelegate onEndEditFolderName;

        public event Action<Button> onSelected;
        public event Action closeSlidePanels;

        private Button button;
        private bool isControlPanelOpen;
        private bool isMouseOverUi;

        private readonly int leftClick = 0;
        //private readonly int rightClick = 1;
        #endregion

        #region Unity Methods
        void Awake()
        {
            openCloseAnimator.Close();
            //fileContainer.SetActive(false);

            button = gameObject.GetComponent<Button>();
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
                StopAllCoroutines();
                StartCoroutine(MouseOver());
            }
        }

        private void OnSelected()
        {
            onSelected?.Invoke(button);
            toolbar.CurrentSelection = button;
            toolbar.CurrentFolder = button;
            ShowOutline();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isControlPanelOpen)
            {
                StopAllCoroutines();
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
            DeSelect();
        }
        #endregion

        #region Open & Close Panel Methods
        public void OpenControlPanel()
        {
            StopCoroutine(MouseOver());
            closeSlidePanels?.Invoke();
            openCloseAnimator.Open();
            isControlPanelOpen = true;
            FileManager.fileGrid = fileContainer.GetComponent<DynamicGrid>();
            OnSelected();
        }

        public void CloseControlPanel()
        {
            StopCoroutine(MouseExit());
            openCloseAnimator.Close();
            //fileContainer.SetActive(false);
            isControlPanelOpen = false;
            toolbar.CurrentFolder = null;
            toolbar.CurrentSelection = null;
        }

        public void DeSelect()
        {
            StopAllCoroutines();
            CloseControlPanel();
            HideOutline();
        }
        #endregion

        #region Utils
        public void SetName(string name)
        {
            folderName.text = name;
        }

        public void ShowOutline()
        {
            outline.SetActive(true);
            containerOutline.SetActive(true);
        }

        public void HideOutline()
        {
            outline.SetActive(false);
            containerOutline.SetActive(false);
        }
        #endregion
    }
}
