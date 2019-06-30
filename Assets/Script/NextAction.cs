using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="InputAction")]
public class NextAction : ScriptableObject
{
    public enum Action
    {
        none,
        left,
        right,
        rotate,
        drop
    }

    public Action action;
}
