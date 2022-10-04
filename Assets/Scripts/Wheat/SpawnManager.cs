using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> cutPrefabs;
    [SerializeField] private WheatDropView dropPrefab;
    
    [SerializeField] private int layingDropCount;
    
    private List<WheatDropView> wheatDropViews;
    

    private void Start()
    {
        wheatDropViews = new List<WheatDropView>(layingDropCount);
       
    }

    public WheatDropView SpawnDrop(Vector3 pos)
    {
        var dropGameObject = Instantiate(dropPrefab, pos, Quaternion.identity, transform);
        dropGameObject.ThrowUp();
        
        CleanFromNulls();
        
        if (wheatDropViews.Count == layingDropCount)
        {
            var last = wheatDropViews[0];
            wheatDropViews.RemoveAt(0);
            Destroy(last.gameObject);
        }

        wheatDropViews.Add(dropGameObject);
        return dropGameObject;
    }

    private void CleanFromNulls()
    {
        List<WheatDropView> list = new List<WheatDropView>();
        for (int i = 0; i < wheatDropViews.Count; i++)
        {
            if (wheatDropViews[i]!=null)
            {
                list.Add(wheatDropViews[i]);
            }
        }

        wheatDropViews = list;
    }

    public GameObject SpawnCut(Vector3 pos)
    {
        var cut = cutPrefabs[Random.Range(0, cutPrefabs.Count)];
        var cutGameObject=Instantiate(cut,pos, Quaternion.identity, transform);
        return cutGameObject;
    }
}