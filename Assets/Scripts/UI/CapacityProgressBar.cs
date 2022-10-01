using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CapacityProgressBar : MonoBehaviour
    {
        [SerializeField] private Image bar;
        private float fill;

        private float targetFill;
        private float animK;
      
        private Coroutine coroutine;

        private void Start()
        {
            fill=0f;
            bar.fillAmount = fill;
        }

        public void ChangeValue(float target)
        {
            targetFill = target;
            if (target==0)
            {
                animK  = 4f;
            }else 
                animK = 2f;
           
            if (coroutine==null)
            {
                coroutine = StartCoroutine(ChangeCoroutine());
            }
           
        }

        private IEnumerator ChangeCoroutine()
        {
            for (float i = 0; i < 1; i+=Time.deltaTime/animK )
            {
                fill=Mathf.Lerp(fill,targetFill,i);
                bar.fillAmount = fill;
                yield return null;
            }
            fill=targetFill;
            bar.fillAmount = fill;
            coroutine = null;
        }
    }
}