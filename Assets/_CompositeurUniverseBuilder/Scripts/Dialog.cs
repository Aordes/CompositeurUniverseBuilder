///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 15/10/2019 11:41
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class Dialog : MonoBehaviour {

        [Header("Title")]
        [SerializeField] protected Text txtAlert;

        [Header("Confirm Button")]
        [SerializeField] protected Button btnConfirm;
        [SerializeField] protected Text txtConfirm;

        [Header("Cancel Button")]
        [SerializeField] protected Button btnCancel;
        [SerializeField] protected Text txtCancel;

        protected virtual void Start()
        {
            if (btnConfirm)
                btnConfirm.onClick.AddListener(ButtonConfirm_OnClick);
            if (btnCancel)
                btnCancel.onClick.AddListener(ButtonCancel_OnClick);
        }

        protected virtual void ButtonConfirm_OnClick()
        {
        }

        protected virtual void ButtonCancel_OnClick()
        {
        }
    }
}