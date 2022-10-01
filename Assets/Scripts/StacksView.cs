using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacksView : MonoBehaviour
{
    [SerializeField] private Transform[] startPositions;
   
    [SerializeField] private StackedWheatBoxView prefab;
    [SerializeField] private int limit;

    public int Limit => limit;

    private List<List<StackedWheatBoxView>> stackedWheatBoxViews;
    private int h;
    private int i;
    private int j;
    
    
    private void Start()
    {
        h = (limit / startPositions.Length);
        stackedWheatBoxViews = new List<List<StackedWheatBoxView>>();
        for (int k = 0; k < h; k++)
        {
            stackedWheatBoxViews.Add(new List<StackedWheatBoxView>());
        }
    }


    public void AddToStack()
    {
        
        if (i==h)
        {
            return;
        }
        var stackedWheatBoxView = Instantiate(prefab, startPositions[j]);
        stackedWheatBoxViews[i].Add(stackedWheatBoxView);
       stackedWheatBoxView.GetComponent<Rigidbody>().isKinematic = true;


       var tr = stackedWheatBoxView.transform;

       
       tr.localScale=new Vector3(0.5f,0.5f,0.5f);
       tr.localRotation=Quaternion.identity;
       
       
       var newTransformPosition =  Vector3.zero;; //changing pos putting to bag
       newTransformPosition.y += i*tr.localScale.x/2; //considering y to stack on top
       tr.localPosition = newTransformPosition;
       
      
       
      // stackedWheatBoxView.TurnCollider(false);
       
       //increment
       if ( ++j== startPositions.Length)
       {
           i++;
           j = 0;
           
       }

      
    }
}
