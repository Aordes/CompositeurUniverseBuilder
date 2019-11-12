///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 14/10/2019 15:39
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class DialogScreen : MonoBehaviour {

        public static DialogScreen Instance { get; private set; }

        [SerializeField] private Animator animator;
        [SerializeField] private string initTrigger = "Init";
        [SerializeField] private string removeTrigger = "Remove";

        [Header("Dialogs")]
        [SerializeField] private ModalDialog modalDialog;
        [SerializeField] private ModalDialogComplex modalDialogComplex;
        [SerializeField] private InputDialog inputDialog;

        ///DEBUG ========================================================================
        private void Start()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);

            Instance = this;

            inputDialog.gameObject.SetActive(false);
            modalDialog.gameObject.SetActive(false);
            modalDialogComplex.gameObject.SetActive(false);

            inputDialog.OnClose += CloseScreen;
            modalDialog.OnClose += CloseScreen;
            modalDialogComplex.OnClose += CloseScreen;
        //    DisplayDialog(Test, "Test", "Confirm", "no", "Cancel");
        }

        //public void Test(bool yes)
        //{
        //    Debug.Log("Result : " + yes);
        //    DisplayDialogComplex(End, "Complex test", "OK", "Cancel", "No", "AAAAAAAA");
        //}

        //public void End(int state)
        //{
        //    Debug.Log("State : " + state);
        //    DisplayInputDialog(TestInput, "Input", "ok", "cancel");
        //}

        //private void TestInput(bool result, string output)
        //{
        //    Debug.Log("result :" + result + " content :" + output);
        //}
        //END DEBUG ====================================================================

        public void DisplayDialog(Action<bool> Callback, string title, string ok, string message = "", string cancel = "")
        {
            modalDialog.gameObject.SetActive(true);

            if (modalDialogComplex)
                modalDialogComplex.gameObject.SetActive(false);
            if (inputDialog)
                inputDialog.gameObject.SetActive(false);

            modalDialog.Init(Callback, title, message, ok, cancel);

            animator.SetTrigger(initTrigger);
        }

        public void DisplayDialogComplex(Action<int> Callback, string title, string ok, string cancel, string alt, string message = "")
        {
            modalDialogComplex.gameObject.SetActive(true);

            if (modalDialog) 
                modalDialog.gameObject.SetActive(false);
            if (inputDialog)
                inputDialog.gameObject.SetActive(false);

            modalDialogComplex.Init(Callback, title, message, ok, alt, cancel);

            animator.SetTrigger(initTrigger);
        }

        public void DisplayInputDialog(Action<bool, string> Callback, string title, string ok, string cancel, string placeHolder = "")
        {
            modalDialogComplex.gameObject.SetActive(true);

            inputDialog.gameObject.SetActive(true);
           
            if (modalDialog) 
                modalDialog.gameObject.SetActive(false);
            if (modalDialogComplex)
                modalDialogComplex.gameObject.SetActive(false);

            inputDialog.Init(Callback, title, ok, cancel, placeHolder);

            animator.SetTrigger(initTrigger);
        }

        public void CloseScreen()
        {
            animator.SetTrigger(removeTrigger);
        }
    }
}