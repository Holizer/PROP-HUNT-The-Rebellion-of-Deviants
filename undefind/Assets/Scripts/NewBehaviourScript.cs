using Unity.VisualScripting;
using UnityEngine;

public static class ComponentUtils
{
    public static void ToggleComponent<T>(GameObject target, bool isEnabled) where T : Behaviour
    {
        T component = target.GetComponent<T>();
        component.enabled = isEnabled;
    }
}
