using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Color Data")]
public class ColorData : ScriptableObject
{
    public int id;
    public Material material;
}

public enum ColorType
{
    NONE = 0,
    RED = 1,
    YELLOW = 2,
    GREEN = 3,
    BLUE = 4,
    PURPLE = 5
}
