using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class ColorChanger : Singleton<ColorChanger>
{
    [SerializeField] ColorData[] colors;

    private ColorData colorData;


    public ColorData GetRandomColorData()
    {
        colorData = colors[Random.Range(1, colors.Count())];
        return colorData;
    }

    public ColorData GetColorData(int id)
    {
        colorData = colors[id];
        return colorData;
    }
}
