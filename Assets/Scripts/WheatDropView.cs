using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheatDropView : MonoBehaviour
{
    [SerializeField] private int value;

    public int Value => value;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    public void ThrowUp()
    {
        var k = 10;
        rb.AddForce(new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(4f, 6f) * k,
            Random.Range(-100f, 100f)
        ));
        //rb.AddTorque(new Vector3(0,Random.Range(-50f,50f),0));
    }
}