///-----------------------------------------------------------------
///   Author : Soren SZABO                    
///   Date   : 14/10/2019 15:39
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder {

    public class DialogScreen : MonoBehaviour {

        [SerializeField] private Animator animator;
        [SerializeField] private string initTrigger = "Init";
        [SerializeField] private string removeTrigger = "Remove";
        [SerializeField] private ModalDialog modalDialog;
        [SerializeField] private ModalDialogComplex modalDialogComplex;

        private void Start () {
            DisplayDialog(Test, "Test", "Confirm", "no", "Cancel");
        }

        public void Test(bool yes)
        {
            Debug.Log("Result : " + yes);
        }

        public void DisplayDialog(Action<bool> Callback, string title, string ok, string message = "", string cancel = "")
        {
            modalDialog.gameObject.SetActive(true);
            modalDialogComplex.gameObject.SetActive(false);

            modalDialog.Init(Callback, title, message, ok, cancel);
            modalDialog.OnStatus += CloseScreen;

            animator.SetTrigger(initTrigger);
        }

        public void DisplayDialogComplex(Action<int> Callback, string title, string ok, string cancel, string alt, string message = "")
        {
            modalDialogComplex.gameObject.SetActive(true);
            modalDialog.gameObject.SetActive(false);

            modalDialogComplex.Init(Callback, title, message, ok, alt, cancel);
            modalDialogComplex.OnStatus += CloseScreen;

            animator.SetTrigger(initTrigger);
        }

        private void CloseScreen(bool isTrue)
        {
            animator.SetTrigger(removeTrigger);
        }

        private void CloseScreen(int id)
        {
            animator.SetTrigger(removeTrigger);
        }
    }
}