using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : Singleton<CameraFollow>
{

    [SerializeField] private float speed;

    private Transform player;
    private Transform myCamera;
    private Vector3 offset;

    private void Awake()
    {
        myCamera = transform;
    }

    private void Update()
    {
        if(player != null)
        {
            Follow();
        }
    }

    private void Follow()
    {
        myCamera.DOMoveX(player.position.x + offset.x, speed * Time.deltaTime);
        myCamera.DOMoveY(player.position.y + offset.y, speed * Time.deltaTime);
        myCamera.DOMoveZ(player.position.z + offset.z, speed * Time.deltaTime);
    }

    public void FindPlayer(Vector3 startPoint)
    {
        player = FindObjectOfType<Player>().transform;
        offset = myCamera.position - player.position + startPoint;
    }
}
