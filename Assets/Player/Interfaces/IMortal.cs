using System;

public interface IMortal
{
    public event Action onPlayerDeath;
    public void Die();
}
