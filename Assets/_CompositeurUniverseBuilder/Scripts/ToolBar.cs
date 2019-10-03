///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 03/10/2019 11:36
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ToolBar : MonoBehaviour {

        [Header("Meta")]
        [SerializeField] private Button btnAddMetaToMainFolder;
        [SerializeField] private Button btnAddMetaToSelection;

        [Header("Rename")]
        [SerializeField] private Button btnRenameMainFolder;
        [SerializeField] private Button btnRenameSelection;

        [Header("Preview")]
        [SerializeField] private Button btnPreviewToMainfolder;
        [SerializeField] private Button btnPreviewToSelection;

        [Header("Delete")]
        [SerializeField] private Button btnDeleteMainfolder;
        [SerializeField] private Button btnDeleteSelection;

        public event Action<string> OnAddMeta;
        public event Action<string> OnRenameItem;
        public event Action<string> OnAddPreview;
        public event Action<string> OnDeleteItem;

        private string _currentFolder;
        public string CurrentFolder
        {
            set {
                _currentFolder = value;
                //init buttons
            }
        }

        private string _currentSelection;
        public string CurrentSelection
        {
            set
            {
                _currentSelection = value;

                //set button state
            }
        }

        private void Start () {
            if (btnAddMetaToMainFolder)
                btnAddMetaToMainFolder.onClick.AddListener(AddMetaToMainFolder_OnClick);
            if (btnAddMetaToSelection)
                btnAddMetaToMainFolder.onClick.AddListener(AddMetaToSelection_OnClick);

            if (btnRenameMainFolder)
                btnAddMetaToMainFolder.onClick.AddListener(RenameMainFolder_OnClick);
            if (btnRenameSelection)
                btnAddMetaToMainFolder.onClick.AddListener(RenameSelection_OnClick);

            if (btnPreviewToMainfolder)
                btnAddMetaToMainFolder.onClick.AddListener(PreviewToMainFolder_OnClick);
            if (btnPreviewToSelection)
                btnAddMetaToMainFolder.onClick.AddListener(PreviewToSelection_OnClick);

            if (btnDeleteMainfolder)
                btnAddMetaToMainFolder.onClick.AddListener(DeleteMainFolder_OnClick);
            if (btnDeleteSelection)
                btnAddMetaToMainFolder.onClick.AddListener(DeleteSelection_OnClick);
        }

        private void AddMetaToMainFolder_OnClick()
        {
            OnAddMeta?.Invoke(_currentFolder);
        }

        private void AddMetaToSelection_OnClick()
        {
            OnAddMeta?.Invoke(_currentSelection);
        }

        private void RenameMainFolder_OnClick()
        {
            OnRenameItem?.Invoke(_currentFolder);
        }

        private void RenameSelection_OnClick()
        {
            OnRenameItem?.Invoke(_currentSelection);
        }

        private void PreviewToMainFolder_OnClick()
        {
            OnAddPreview?.Invoke(_currentFolder);
        }

        private void PreviewToSelection_OnClick()
        {
            OnAddPreview?.Invoke(_currentSelection);
        }

        private void DeleteMainFolder_OnClick()
        {
            OnDeleteItem(_currentFolder);
        }

        private void DeleteSelection_OnClick()
        {
            OnDeleteItem(_currentSelection);
        }

        public void OnSelectionChange(GameObject selection)
        {

        }
    }
}