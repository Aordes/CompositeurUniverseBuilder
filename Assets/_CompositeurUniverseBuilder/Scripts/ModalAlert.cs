///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 14/10/2019 10:04
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ModalAlert : MonoBehaviour {

        [Header("Texts")]
        [SerializeField] protected Text txtAlert;
        [SerializeField] protected Text txtMessage;

        [Header("Buttons")]
        [SerializeField] protected Button btnConfirm;
        [SerializeField] protected Button btnCancel;

        [Header("Button Texts")]
        [SerializeField] protected Text txtConfirm;
        [SerializeField] protected Text txtCancel;

        public Action<bool> OnStatus;

        private void Start () {
            if (btnConfirm)
                btnConfirm.onClick.AddListener(ButtonConfirm_OnClick);
            if (btnCancel)
                btnCancel.onClick.AddListener(ButtonCancel_OnClick);
        }

        public void Init(Action<bool> Callback, string title, string message, string ok, string cancel)
        {
            OnStatus = Callback;

            if (string.IsNullOrWhiteSpace(cancel))
            {
                btnCancel.interactable = false;
                btnConfirm.transform.SetSiblingIndex(btnConfirm.transform.parent.childCount);
            }
            else 
            {
                btnCancel.interactable = true;
                btnConfirm.transform.SetSiblingIndex(0);
            }

            txtAlert.text = title;
            txtMessage.text = message;

            txtConfirm.text = ok;
            txtCancel.text = cancel;
        }

        private void ButtonConfirm_OnClick()
        {
            OnStatus?.Invoke(true);
            OnStatus = null;
        }

        private void ButtonCancel_OnClick()
        {
            OnStatus?.Invoke(false);
            OnStatus = null;
        }
    }
}