///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 03/10/2019 11:36
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.UniverseBuilder
{

    public class ToolBar : MonoBehaviour {

        [Header("Meta")]
        [SerializeField] private Button btnAddMetaToMainFolder;
        [SerializeField] private Button btnAddMetaToSelection;
        [SerializeField] private GameObject metaMenu;

        [Header("MetaData")]
        [SerializeField] private Toggle desiredWidth;
        [SerializeField] private Slider desiredWidthSlider;
        [SerializeField] private Toggle showOnStart;
        [SerializeField] private Toggle videoLoop;
        [SerializeField] private Toggle videoAutoPlay;
        [SerializeField] private Toggle videoMute;

        [Header("Rename")]
        [SerializeField] private Button btnRenameMainFolder;
        [SerializeField] private Button btnRenameSelection;

        [Header("Preview")]
        [SerializeField] private Button btnPreviewToMainfolder;
        [SerializeField] private Button btnPreviewToSelection;

        [Header("Delete")]
        [SerializeField] private Button btnDeleteMainfolder;
        [SerializeField] private Button btnDeleteSelection;

        [Header("Content")]
        [SerializeField] private Button btnAddContent;

        public event Action<GameObject> OnSelectionChangeEvent;

        private bool isFile;

        private Button _currentFolder;
        public Button CurrentFolder
        {
            set {
                _currentFolder = value;               
                //init buttons
            }
        }

        private Button _currentSelection;
        public Button CurrentSelection
        {
                //OnSelectionChange();
            set
            {
                _currentSelection = value;
                if (_currentFolder == value) return;
                else
                {
                    _currentFolder = value;
                    OnSelectionChangeEvent?.Invoke(value.gameObject);
                }

                //set button state
            }
        }

        private void Start () {
            OnSelectionChangeEvent += OnSelectionChange;

            if (btnAddMetaToMainFolder)
                btnAddMetaToMainFolder.onClick.AddListener(AddMetaToSelection_OnClick);

            if (btnRenameMainFolder)
                btnRenameMainFolder.onClick.AddListener(RenameSelection_OnClick);

            if (btnPreviewToMainfolder)
                btnPreviewToMainfolder.onClick.AddListener(PreviewToSelection_OnClick);

            if (btnDeleteMainfolder)
                btnDeleteMainfolder.onClick.AddListener(DeleteSelection_OnClick);

            if (btnAddContent)
                btnAddContent.onClick.AddListener(AddContent_OnClick);
        }

        private void AddMetaToSelection_OnClick()
        {
            metaMenu.SetActive(true);
            FileManager.AddMetaToFile("Hhh", _currentSelection);
        }

        private void RenameSelection_OnClick()
        {
            if (isFile)
            {
                FileManager.FileButton_RenameFile("newName", _currentSelection);
            }
            else
            {
                FileManager.FolderButton_RenameFolder("newName", _currentSelection);
            }
        }

        private void PreviewToSelection_OnClick()
        {
            if (!isFile)
            {
                FileManager.FolderButton_OnChangePreview(_currentFolder);
            }
        }

        private void DeleteSelection_OnClick()
        {
            if (isFile)
            {
                FileManager.DeleteFileDirectory(_currentSelection);
            }
            else
            {
                FileManager.FolderButton_OnDeleteDirectory(_currentSelection);
            }
        }

        private void AddContent_OnClick()
        {
            FileManager.FolderButton_OnChangeDirectoryContent(_currentSelection);
        }

        public void OnSelectionChange(GameObject selection)
        {
            Debug.Log("Change");
            isFile = _currentSelection.gameObject.CompareTag("File");
        }
    }
}