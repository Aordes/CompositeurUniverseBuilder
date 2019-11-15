///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 11:39
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{

    public class InputDialog : Dialog
    {

        [Header("Input")]
        [SerializeField] protected Text txtPlaceHolder;
        [SerializeField] protected InputField txtInput;

        public event Action<bool, string> OnStatus;
        public Action OnClose;
        private bool isValidated;

        private Action doAction;

        public void Init(Action<bool, string> Callback, string title, string ok, string cancel, string placeHolder)
        {
            OnStatus = Callback;

            txtAlert.text = title;
            txtPlaceHolder.text = placeHolder;

            txtInput.text = string.Empty;
            isValidated = false;

            txtConfirm.text = ok;
            txtCancel.text = cancel;

            StartCoroutine(SelectInput());
            doAction = DoActionCheckForEnter;
        }

        public IEnumerator SelectInput()
        {
            yield return null;
            txtInput.Select();
        }

        private void Update()
        {
            doAction?.Invoke();
        }

        private void DoActionCheckForEnter()
        {
            if (txtInput.isFocused)
                isValidated = true;

            if (isValidated && !string.IsNullOrWhiteSpace(txtInput.text) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
                ButtonConfirm_OnClick();
        }

        protected override void ButtonConfirm_OnClick()
        {
            string input = txtInput.text;

            if (string.IsNullOrWhiteSpace(input))
            {
                Debug.LogWarning("Input text is empty");
                return;
            }

            Action<bool, string> CachedCallback = OnStatus;
            OnStatus = null;
            doAction = null;

            CachedCallback?.Invoke(true, input);
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