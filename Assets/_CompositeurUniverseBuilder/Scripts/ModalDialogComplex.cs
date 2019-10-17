///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 14/10/2019 17:20
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class ModalDialogComplex : ModalDialog {

        [Header("Other Button")]
        [SerializeField] protected Button btnOther;
        [SerializeField] protected Text txtOther;

        public new Action<int> OnStatus;

        protected override void Start()
        {
            base.Start();

            if (btnOther)
                btnOther.onClick.AddListener(ButtonOther_OnClick);
        }

        public void Init(Action<int> Callback, string title, string message, string ok, string alt, string cancel)
        {
            OnStatus = Callback;

            txtAlert.text = title;
            txtMessage.text = message;

            txtConfirm.text = ok;
            txtOther.text = alt;
            txtCancel.text = cancel;
        }

        protected override void ButtonConfirm_OnClick()
        {
            OnStatus?.Invoke(0);
            OnStatus = null;
        }

        protected override void ButtonCancel_OnClick()
        {
            OnStatus?.Invoke(1);
            OnStatus = null;
        }

        protected void ButtonOther_OnClick()
        {
            OnStatus?.Invoke(2);
            OnStatus = null;
        }
    }
}