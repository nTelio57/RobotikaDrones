using System.Linq;
using UnityEngine;

public enum BoxType
{
    None,
    Light, 
    Heavy
}

public class Box : MonoBehaviour
{
    public BoxType BoxType;
    public bool IsNoticed;
    public Hook[] Hooks;

    private void Awake()
    {
        IsNoticed = false;
    }

    public Hook GetHook()
    {
        var hook = Hooks.First(x => x.IsTaken == false);
        hook.IsTaken = true;
        return hook;
    }
}
