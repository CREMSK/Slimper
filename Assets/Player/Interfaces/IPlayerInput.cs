using System;
using UnityEngine;

public interface IPlayerInput
{
    public Vector2 GetMouseWorldPos();

    public event Action onJumpStart;
    public event Action onJumpEnd;
}
