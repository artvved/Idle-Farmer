using System;
using System.Collections;
using DG.Tweening;
using UI;
using UnityEngine;


public class WheatReceiver : MonoBehaviour
{
    [SerializeField] private FlyableView coinPrefab;
    [SerializeField] private Transform target;
    
    


    private void OnTriggerEnter(Collider other)
    {
        var flyableView = other.gameObject.GetComponentInParent<FlyableView>();
        if (flyableView != null)
        {
            if (flyableView.Type == FlyingType.WHEAT)
            {
                Destroy(other.gameObject);

                StartCoroutine(WaitForSecAndInstantiate());
              
            }
        }
    }


    private IEnumerator WaitForSecAndInstantiate()
    {
        yield return new WaitForSeconds(0.5f);
        var c = Instantiate(coinPrefab, transform);
        c.FlyToTarget(target);
    }
}