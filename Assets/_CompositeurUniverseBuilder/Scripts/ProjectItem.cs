///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 17:07
///-----------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {
    [RequireComponent(typeof(Animator))]
    public class ProjectItem : MonoBehaviour
    {
        public static string BTN_INIT_TRIGGER = "Init";

        [SerializeField] protected Button btnSelf;
        [SerializeField] protected Text txtTitle;
        [SerializeField] protected Image imgPreview;

        protected Animator animator;
        protected DirectoryInfo source;

        public event Action<DirectoryInfo> OnClick;

        public void Init(DirectoryInfo directory, Sprite image)
        {
            if (image == null)
                imgPreview.enabled = false;
            else
                imgPreview.sprite = image;

            txtTitle.text = directory.Name;
            source = directory;

            animator = GetComponent<Animator>();
        }

        public void Show()
        {
            animator.SetTrigger(BTN_INIT_TRIGGER);
            btnSelf.onClick.AddListener(ButtonSelf_OnClick);
        }

        private void ButtonSelf_OnClick()
        {
            if (source == null)
                throw new InvalidOperationException("no path");

            OnClick?.Invoke(source);
        }
    }
}