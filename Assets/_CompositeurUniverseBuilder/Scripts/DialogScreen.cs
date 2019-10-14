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
        [SerializeField] private ModalAlert modalAlert;

        private void Start () {
            DisplayDialog(Test, "Test", "Confirm", "no", "Cancel");
        }

        public void Test(bool yes)
        {
            Debug.Log("Result : " + yes);
        }

        public void DisplayDialog(Action<bool> Callback, string title, string ok, string message = "", string cancel = "")
        {
            modalAlert.gameObject.SetActive(true);
            modalAlert.Init(Callback, title, message, ok, cancel);

            modalAlert.OnStatus += CloseScreen;

            animator.SetTrigger(initTrigger);
        }

        private void CloseScreen(bool obj)
        {
            animator.SetTrigger(removeTrigger);
        }
    }
}