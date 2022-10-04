using System;
using System.Collections;
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

                StartCoroutine(WaitForSec());
                var c = Instantiate(coinPrefab, transform);
                
                
                c.FlyToTarget(target);
            }
        }
    }


    private IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(0.5f);
    }
}