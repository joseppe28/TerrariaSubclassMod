namespace Content.Player_Moded;

// All the special states needed for the new subclasses like blocking for tanks.
public enum PlayerState
{
    Idle,
    Blocking,
    Stunned,
    Enraged,
    Casting,
    Reloading
}