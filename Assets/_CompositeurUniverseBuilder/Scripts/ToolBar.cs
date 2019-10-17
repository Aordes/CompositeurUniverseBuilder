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
                if (_currentSelection == value) return;
                else
                {
                    _currentSelection = value;
                    OnSelectionChangeEvent?.Invoke(value.gameObject);
                }

                //set button state
            }
        }

        private void Start() {
            OnSelectionChangeEvent += OnSelectionChange;

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

        private void Update()
        {
            //Debug.Log(_currentSelection.gameObject.name);
        }

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
            Debug.Log(value);
            FileManager.ChangeMetaData(isFile, _currentSelection, MetaData.VDEO_MUTE, value);
        }
        #endregion

        #region ToolBar Buttons On_Click
        private void OpenMetaToMenu_OnClick()
        {
            metaMenu.SetActive(true);
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
        #endregion

        public void OnSelectionChange(GameObject selection)
        {
            Debug.Log("Change");
            isFile = selection.CompareTag("File");
            CloseMetaToMenu();
            UpdateMetaMenuToCurrentSelection();
        }
    }
}