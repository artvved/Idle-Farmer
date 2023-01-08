using System;

using UnityEngine;


public class GyroscopeInputManager : IInputManager
{
    public override event Action<Vector2> InputEvent;

    private Vector2 direction;
    

    private void Start()
    {
        direction = new Vector2();
    }

    void Update()
    {
        direction.x = Input.acceleration.x;
        direction.y = Input.acceleration.y;
    }

    private void FixedUpdate()
    {
        InputEvent?.Invoke(direction);
    }
}