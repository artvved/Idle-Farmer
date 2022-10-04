using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatEarView : MonoBehaviour
{
    
    [SerializeField] private Transform dropSpawnPosition;


    private Animator animator;
    
    
    public event Action CutAction;
    public bool IsCut { get; set; }
    private Collider col;

   

    public Transform DropSpawnPosition => dropSpawnPosition;

    private void Start()
    {
        IsCut = false;
        col = GetComponent<Collider>();
        animator=GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var pl = other.gameObject.GetComponent<ScytheView>();
        if (pl != null)
        {
            OnCut();
        }
    }

    private void OnCut()
    {
        IsCut = true;
        CutAction?.Invoke();
    }


    public void TurnCollider(bool en)
    {
        col.enabled = en;
    }

    public void PlayGrowthAnimation()
    {
        animator.SetTrigger("Growth");
    }
    public void PlayCutAnimation()
    {
        animator.SetTrigger("Cut");
    }
    
    


}
