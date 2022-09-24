using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatChunk : MonoBehaviour
{
    [SerializeField] private List<GameObject> cutPrefabs;
    [SerializeField] private WheatDropView dropPrefab;

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
        var cut = cutPrefabs[Random.Range(0, cutPrefabs.Count)];
        var cutGameObject=Instantiate(cut, view.transform.position, Quaternion.identity, transform);

        //spawn drop
        var dropGameObject = Instantiate(dropPrefab,  view.DropSpawnPosition.position, Quaternion.identity, transform);
        dropGameObject.ThrowUp();

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
