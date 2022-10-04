using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WheatChunk : MonoBehaviour
{
    [Inject] private SpawnManager spawnManager;
    
    private WheatEarView[] wheatEarViews;
    
    private void Start()
    {
        wheatEarViews = GetComponentsInChildren<WheatEarView>();

        for (int i = 0; i < wheatEarViews.Length; i++)
        {
            var w = wheatEarViews[i];
            w.CutAction += OnWheatCut;
        }
    }

    private void OnWheatCut()
    {
        WheatEarView view = null;
        foreach (var v in wheatEarViews)
        {
            if (v.IsCut)
            {
                view = v;
                v.IsCut = false;
                break;
            }
        }
        if (view==null)
        {
            return;
        }

       
        view.TurnCollider(false);
        view.PlayCutAnimation();
            
        //spawn cut
        var cutGameObject=spawnManager.SpawnCut(view.transform.position);
       
        //spawn drop
        var drop=spawnManager.SpawnDrop(view.DropSpawnPosition.position);
       
        StartCoroutine(GrowthCoroutine(view,cutGameObject));
    }

    private IEnumerator GrowthCoroutine(WheatEarView view,GameObject cutGameObject)
    {
       
        yield return new WaitForSeconds(5f);
        view.PlayGrowthAnimation();
       
        yield return new WaitForSeconds(1f);
        Destroy(cutGameObject);
        view.TurnCollider(true);
        
    }




}
