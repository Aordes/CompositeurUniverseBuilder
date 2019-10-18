///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 17:07
///-----------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ProjectItem : MonoBehaviour {

        [SerializeField] protected Button btnSelf;
        [SerializeField] protected Text txtTitle;
        [SerializeField] protected Image imgPreview;
        
        protected DirectoryInfo source;
        public event Action<DirectoryInfo> OnClick;

        private void Start()
        {
            btnSelf.onClick.AddListener(ButtonSelf_OnClick);
        }

        public void Init(DirectoryInfo directory, Sprite preview)
        {
            if (preview == null)
                imgPreview.enabled = false;
            else
                imgPreview.sprite = preview;

            txtTitle.text = directory.Name;
            source = directory;
        }

        private void ButtonSelf_OnClick()
        {
            if (source == null)
                throw new InvalidOperationException("no path");

            OnClick?.Invoke(source);
        }
    }
}