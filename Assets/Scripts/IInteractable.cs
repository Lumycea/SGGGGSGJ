using UnityEngine;

public interface IInteractable
{
    public virtual bool Interact(Player playerState) { return false; }
    public virtual bool Grab(Player playerState) { return false; }
}
