///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #20.09.2019#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.Docaret.CompositeurUniverseBuilder
{
	public class ButtonAnimator : MonoBehaviour {

        [SerializeField] private Animator animator;
        [SerializeField] private Button button;

        private bool isOpen;
	
		private void Start ()
        {
            button.onClick.AddListener(OnClick_OpenClose);
		}

        private void OnClick_OpenClose()
        {
            if (!isOpen)
            {
                animator.SetTrigger("open");
                isOpen = true;
            }
            else
            {
                animator.SetTrigger("close");
                isOpen = false;
            }
        }	
	}
}