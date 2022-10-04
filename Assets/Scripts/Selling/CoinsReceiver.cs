using System;
using DG.Tweening;
using UI;
using UnityEngine;

public class CoinsReceiver : MonoBehaviour
{
    public event Action CoinReceiveEvent; 
    
    private void OnTriggerEnter(Collider other)
    {
        var flyableView = other.gameObject.GetComponentInParent<FlyableView>();
        if (flyableView != null)
        {
            if (flyableView.Type == FlyingType.COIN)
            {
                Destroy(other.gameObject);
                CoinReceiveEvent?.Invoke();
                
            }
        }
    }
}