using System;

public interface IInteractable
{
    public enum Direction
    {
        Left,
        Right
    }
    public virtual bool Interact(Player playerState) { return false; }
    public virtual bool Swipe(Player playerState, Direction direction) { return false; }

}
