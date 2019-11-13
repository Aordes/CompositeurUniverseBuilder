///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 11:39
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{

    public class InputDialog : Dialog {

        [Header("Input")]
        [SerializeField] protected Text txtPlaceHolder;
        [SerializeField] protected InputField txtInput;

        public new event Action<bool, string> OnStatus;
        public Action OnClose;

        public void Init(Action<bool, string> Callback, string title, string ok, string cancel, string placeHolder)
        {
            OnStatus = Callback;

            txtAlert.text = title;
            
            txtPlaceHolder.text = placeHolder;
            txtInput.text = string.Empty;

            txtConfirm.text = ok;
            txtCancel.text = cancel;
        }

        protected override void ButtonConfirm_OnClick()
        {
            string input = txtInput.text;

            if (string.IsNullOrWhiteSpace(input))
            {
                Debug.LogWarning("Input text is empty");
                return;
            }
            
            base.ButtonConfirm_OnClick();

            OnStatus?.Invoke(true, input);
            //OnStatus = null;

            OnClose?.Invoke();
        }

        protected override void ButtonCancel_OnClick()
        {
            base.ButtonCancel_OnClick();

            OnStatus?.Invoke(false, string.Empty);
            OnStatus = null;

            OnClose?.Invoke();
        }
    }
}