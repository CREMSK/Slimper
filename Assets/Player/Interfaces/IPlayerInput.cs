using System;
using UnityEngine;

public interface IPlayerInput
{
    public event Action onJumpStart;
    public event Action onJumpEnd;
}
