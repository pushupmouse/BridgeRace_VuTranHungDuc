using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] public GameObject brickObject;

    public ColorData colorData;


    public void SetColor(int id)
    {
        colorData = ColorChanger.Instance.GetColorData(id);
        transform.GetComponent<MeshRenderer>().material = colorData.material;
    }
}
