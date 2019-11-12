///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 17/10/2019 17:54
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Docaret.CompositeurUniverseBuilder {
	public class OpenCloseAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public bool isOpen;

        public void Open()
        {
            Debug.Log("Open");
            animator.ResetTrigger("Open");
            animator.SetTrigger("Open");
            isOpen = true;
            animator.ResetTrigger("Close");
        }

        public void Close()
        {
            Debug.Log("Close");
            animator.SetTrigger("Close");
            isOpen = false;
            animator.ResetTrigger("Open");
        }
    }
}