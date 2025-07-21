using System;
using UnityEngine;

public interface IMortal
{
    public event Action onPlayerDeath;
    public void Die(Vector2 point);
}
