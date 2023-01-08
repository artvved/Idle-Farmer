using System;

using UnityEngine;


public class JoystickInputManager : IInputManager
{
    [SerializeField] private Joystick joystick;
    public override event Action<Vector2> InputEvent;

    private Vector2 direction;
    
    private void Start()
    {
        direction = new Vector2();
    }

    void Update()
    {
        direction.x = joystick.Direction.x;
        direction.y = joystick.Direction.y;
    }

    private void FixedUpdate()
    {
        InputEvent?.Invoke(direction);
    }
}