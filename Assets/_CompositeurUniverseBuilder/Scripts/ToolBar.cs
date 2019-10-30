///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 03/10/2019 11:36
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{

    public class ToolBar : MonoBehaviour {

        #region Fields
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

        [Header("Animator")]
        [SerializeField] private OpenCloseAnimator animator;

        [Header("DialogScreen")]
        [SerializeField] private DialogScreen dialogScreen;

        public event Action<GameObject> OnSelectionChangeEvent;
        public event Action<bool> OnDeleteDialog;
        public event Action<bool,string> OnRenameDialog;

        private bool isFile;
        #endregion

        #region Current Selection Get&Set
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
                if (_currentSelection == value) return;
                else if (value == null)
                {
                    CloseToolbar();
                    _currentSelection = value;
                }
                else
                {
                    _currentSelection = value;
                    OnSelectionChangeEvent?.Invoke(value.gameObject);
                }
                //set button state
            }
        }
        #endregion

        #region Unity Methods
        private void Start() {
            OnSelectionChangeEvent += OnSelectionChange;
            OnDeleteDialog += OnDeleteConfirm;
            OnRenameDialog += OnRenameConfirm;

            #region Buttons AddListeners
            if (btnAddMetaToMainFolder)
                btnAddMetaToMainFolder.onClick.AddListener(OpenMetaToMenu_OnClick);

            if (btnRenameMainFolder)
                btnRenameMainFolder.onClick.AddListener(RenameSelection_OnClick);

            if (btnPreviewToMainfolder)
                btnPreviewToMainfolder.onClick.AddListener(PreviewToSelection_OnClick);

            if (btnDeleteMainfolder)
                btnDeleteMainfolder.onClick.AddListener(DeleteSelection_OnClick);

            if (btnAddContent)
                btnAddContent.onClick.AddListener(AddContent_OnClick);
            #endregion

            #region Toggles & Sliders AddListeners
            //MetaData Toggles & Sliders
            if (desiredWidth) desiredWidth.onValueChanged.AddListener((value) => {
                DesiredWidth_OnValueChanged(value);
            });

            if (desiredWidth) desiredWidthSlider.onValueChanged.AddListener((value) => {
                DesiredWidthSlider_OnValueChanged(value);
            });

            if (showOnStart) showOnStart.onValueChanged.AddListener((value) => {
                ShowOnStart_OnValueChanged(value);
            });

            if (videoLoop) videoLoop.onValueChanged.AddListener((value) => {
                VideoLoop_OnValueChanged(value);
            });

            if (videoAutoPlay) videoAutoPlay.onValueChanged.AddListener((value) => {
                VideoAutoPlay_OnValueChanged(value);
            });

            if (videoMute) videoMute.onValueChanged.AddListener((value) => {
                VideoMute_OnValueChanged(value);
            });
            #endregion
        }
        #endregion

        #region Meta Menu
        private void CloseMetaToMenu()
        {
            metaMenu.SetActive(false);
        }

        private void UpdateMetaMenuToCurrentSelection()
        {
            MetaData md;

            if (isFile)
            {
               FileStruct fileStruct = FileManager.GetFileStructFromFileButton(_currentSelection);
               md = fileStruct.metaData;
            }
            else
            {
                FolderStruct folderStruct = FileManager.GetFolderStructFromFolderButton(_currentSelection);
                md = folderStruct.metaData;
            }

            desiredWidth.isOn = md.desiredWidth;
            desiredWidthSlider.value = md.desiredWidthValue;

            showOnStart.isOn = md.showOnStart;

            videoLoop.isOn = md.videoLoop;

            videoAutoPlay.isOn = md.videoAutoplay;

            videoMute.isOn = md.videoMute;
        }
        #endregion

        #region Meta OnValueChanged
        private void DesiredWidth_OnValueChanged(bool value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.DESIRED_WIDTH, value, Mathf.RoundToInt(desiredWidthSlider.value));
        }

        private void DesiredWidthSlider_OnValueChanged(float value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.DESIRED_WIDTH, desiredWidth.isOn, Mathf.RoundToInt(value));
        }

        private void ShowOnStart_OnValueChanged(bool value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.SHOW_ON_START, value);
        }

        private void VideoLoop_OnValueChanged(bool value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.VDEO_LOOP, value);
        }

        private void VideoAutoPlay_OnValueChanged(bool value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.VDEO_AUTOPLAY, value);
        }

        private void VideoMute_OnValueChanged(bool value)
        {
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.VDEO_MUTE, value);
        }
        #endregion

        #region ToolBar Buttons On_Click
        private void OpenMetaToMenu_OnClick()
        {
            metaMenu.SetActive(true);
        }

        #region OnRename
        private void RenameSelection(string newName)
        {
            if (isFile)
            {
                FileManager.FileButton_RenameFile(newName, _currentSelection);
            }
            else
            {
                FileManager.FolderButton_RenameFolder(newName, _currentSelection);
            }
        }

        private void RenameSelection_OnClick()
        {
            dialogScreen.DisplayInputDialog(OnRenameDialog, "Rename", "Ok", "Cancel", "New name");
        }

        private void OnRenameConfirm(bool confirm, string newName)
        {
            if (!confirm) return;
            RenameSelection(newName);
        }
        #endregion

        private void PreviewToSelection_OnClick()
        {
            if (!isFile)
            {
                FileManager.FolderButton_OnChangePreview(_currentFolder);
            }
        }

        #region OnDelete
        private void DeleteSelection()
        {
            if (isFile)
            {
                FileManager.DeleteFileDirectory(_currentSelection);
                _currentSelection = _currentFolder;
                isFile = false;
                UpdateMetaMenuToCurrentSelection();
            }
            else
            {
                FileManager.FolderButton_OnDeleteDirectory(_currentSelection);
                _currentFolder = null;
                CloseToolbar();
            }
        }

        private void DeleteSelection_OnClick()
        {
            dialogScreen.DisplayDialog(OnDeleteDialog, "Delete", "Ok", "Are you shure you want to delete this?", "Cancel"); 
        }

        private void OnDeleteConfirm(bool confirm)
        {
            if (!confirm) return;
            DeleteSelection();
        }
        #endregion

        private void AddContent_OnClick()
        {
            FileManager.FolderButton_OnChangeDirectoryContent(_currentSelection);
        }
        #endregion

        #region Utils
        private void OnSelectionChange(GameObject selection)
        {
            if (_currentSelection != null)
            {
                if (!animator.isOpen) animator.Open();
                isFile = selection.CompareTag("File");
                CloseMetaToMenu();
                UpdateMetaMenuToCurrentSelection();
            }
        }

        private void CloseToolbar()
        {
            animator.Close();
        }
        #endregion
    }
}