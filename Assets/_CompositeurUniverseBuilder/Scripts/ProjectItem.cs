///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 17:07
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ProjectItem : MonoBehaviour {

        [SerializeField] protected Button btnSelf;
        [SerializeField] protected Text txtTitle;
        [SerializeField] protected Image imgPreview;
        
        protected string itemPath;

        public event Action<string> OnClick;

        private void Start()
        {
            btnSelf.onClick.AddListener(ButtonSelf_OnClick);
        }

        public void Init(string path, string title, Sprite preview)
        {
            if (imgPreview != null)
            {
                imgPreview.sprite = preview;
            }

            txtTitle.text = title;
            itemPath = path;
        }

        private void ButtonSelf_OnClick()
        {
            if (string.IsNullOrWhiteSpace(itemPath))
                throw new InvalidOperationException("no path");

            OnClick?.Invoke(itemPath);
        }
    }
}