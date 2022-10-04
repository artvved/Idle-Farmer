using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheatDropView : MonoBehaviour
{
    [SerializeField] private int value;

    public int Value => value;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    
    public void ThrowUp()
    {
        var k = 10;
        rb.AddForce(new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(4f, 6f) * k,
            Random.Range(-100f, 100f)
        ));
    }

    public void TurnCollider(bool turn)
    {
        col.enabled = turn;
    }
    
    public void TurnTrigger(bool turn)
    {
        col.isTrigger = turn;
    }
}