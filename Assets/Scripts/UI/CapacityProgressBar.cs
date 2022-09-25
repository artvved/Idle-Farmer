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
        private Coroutine coroutine;

        private void Start()
        {
            fill=0f;
            bar.fillAmount = fill;
        }

        public void ChangeValue(float target)
        {
            
            targetFill = target;
            if (coroutine==null)
            {
                coroutine = StartCoroutine(Fill());
            }
           
        }

        private IEnumerator Fill()
        {
            for (float i = 0; i < 1; i+=Time.deltaTime)
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