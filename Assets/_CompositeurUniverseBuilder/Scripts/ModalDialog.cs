///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 14/10/2019 10:04
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ModalDialog : Dialog {

        [Header("Message")]
        [SerializeField] protected Text txtMessage;

        public Action<bool> OnStatus;
        public Action OnClose;

        public void Init(Action<bool> Callback, string title, string message, string ok, string cancel)
        {
            OnStatus = Callback;

            SetButtonOrder(cancel);

            txtAlert.text = title;
            txtMessage.text = message;

            txtConfirm.text = ok;
            txtCancel.text = cancel;
        }

        private void SetButtonOrder(string cancel)
        {
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
        }

        protected override void ButtonConfirm_OnClick()
        {
            OnStatus?.Invoke(true);
            OnStatus = null;

            OnClose?.Invoke();
        }

        protected override void ButtonCancel_OnClick()
        {
            OnStatus?.Invoke(false);
            OnStatus = null;

            OnClose?.Invoke();
        }
    }
}