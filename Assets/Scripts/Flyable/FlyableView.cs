using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


public class FlyableView : MonoBehaviour
{
    [SerializeField] private FlyingType type;
    [SerializeField] private float flyTimeK;
 
    public FlyingType Type => type;

   
    public void FlyToTarget(Transform target)
    {
        StartCoroutine(Fly(target));

    }

    private IEnumerator Fly(Transform target)
    {
        
        transform.parent = null;
        for (float i = 0; i < 1; i+=Time.deltaTime/flyTimeK)
        {
            
            transform.position=Vector3.Lerp(transform.position,target.position,i);
            yield return null;
        }
       

        transform.position = target.position;

    }

  
    
    
}