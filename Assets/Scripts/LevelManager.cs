using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;

    private GameObject currentLevel;
    private Vector3 startPoint;

    private void Start()
    {
        //currentLevel = (GameObject)Instantiate(Resources.Load(MyConst.Path.LEVELS_PATH + "1"));
        startPoint = FindObjectOfType<StartPoint>().Position;

        Instantiate(playerPrefab, startPoint, Quaternion.identity);

        CameraFollow.Instance.FindPlayer(startPoint);
    }
}
