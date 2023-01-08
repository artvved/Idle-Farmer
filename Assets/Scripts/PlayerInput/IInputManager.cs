using System;
using UnityEngine;

public abstract class IInputManager : MonoBehaviour
{
    public abstract event Action<Vector2> InputEvent;
}