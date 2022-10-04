using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacksView : MonoBehaviour
{
    [SerializeField] private Transform[] startPositions;

    [SerializeField] private FlyableView prefab;
    [SerializeField] private int maxSize; // max count of stacks 

    public int MaxSize => maxSize;

    private List<List<FlyableView>> flyingWheatStacksViews;
    private int h;
    private int i;
    private int j;
    private int curSize;

    public int CurSize => curSize;


    private void Start()
    {
        h = (maxSize / startPositions.Length);
        maxSize = h*startPositions.Length; //limiting maxSize to correct int to fit visually
        InitStacks();
    }

    private void InitStacks()
    {
        flyingWheatStacksViews = new List<List<FlyableView>>();
        for (int k = 0; k < h; k++)
        {
            flyingWheatStacksViews.Add(new List<FlyableView>());
        }

        i = 0;
        j = 0;
        curSize = 0;
      
    }

    public void MoveStacksToTarget(Transform target)
    {
        StartCoroutine(MoveStacksWithDelay(target));
    }

    private IEnumerator MoveStacksWithDelay(Transform target)
    {

        for (int k = flyingWheatStacksViews.Count - 1; k >= 0; k--)
        {
            for (int k1 = flyingWheatStacksViews[k].Count - 1; k1 >= 0; k1--)
            {
                yield return new WaitForSeconds(0.05f);
                flyingWheatStacksViews[k][k1].FlyToTarget(target);
            }
        }
        InitStacks();
    }

    public void AddToStack()
    {
        if (i == h)
        {
            return;
        }

        var stackedWheatBoxView = Instantiate(prefab, startPositions[j]);
        flyingWheatStacksViews[i].Add(stackedWheatBoxView);

        var tr = stackedWheatBoxView.transform;


        tr.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        tr.localRotation = Quaternion.identity;


        var newTransformPosition = Vector3.zero; //changing pos putting to bag
        newTransformPosition.y += i * tr.localScale.x / 2; //considering y to stack on top
        tr.localPosition = newTransformPosition;


        //increment
       
        curSize++;
        if (++j == startPositions.Length)
        {
            i++;
            j = 0;
        }
    }
}