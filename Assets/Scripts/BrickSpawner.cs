using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour
{
    [Serializable]
    public class Ground
    {
        public int id;
        public Transform ground;
    }
    
    [SerializeField] GameObject brickPrefab;
    
    public List<Ground> grounds = new List<Ground>();

    public Dictionary<int, List<Brick>> stageBricks = new Dictionary<int, List<Brick>>();

    private void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            SpawnBricks(i, 10, 0);
        }
        SpawnBricks(-1, 10, 0);
    }

    public void SpawnBricks(int id, int brickNum, int groundId)
    {
        for(int i = 0; i <  brickNum; i++)
        {
            GameObject brickObj = Instantiate(brickPrefab);

            if (!stageBricks.ContainsKey(groundId))
            {
                List<Brick> bricksOnGround = new List<Brick>();
                stageBricks.Add(groundId, bricksOnGround);
            }
            stageBricks[groundId].Add(brickObj.GetComponent<Brick>());

            brickObj.transform.SetParent(grounds[groundId].ground.transform);
            brickObj.transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 0.25f, Random.Range(-1.7f, 3.5f));

            Brick brick = brickObj.GetComponent<Brick>();
            if(id == -1)
            {
                brick.SetColor(Random.Range(1, 5));
            }
            else
            {
                brick.SetColor(id);
            }
        }
    }

    public void DestroyBrick(int id, int groundId, GameObject brickObject)
    {
        if (stageBricks.ContainsKey(groundId))
        {
            foreach (Brick brick in stageBricks[groundId])
            {
                if (brick.colorData.id == id)
                {
                    Destroy(brickObject);
                    stageBricks[groundId].Remove(brick);
                    return;
                }
            }
        } 
    }

    public void ClearBricks(int id, int groundId)
    {
        if (stageBricks.ContainsKey(groundId))
        {
            for(int i = stageBricks[groundId].Count - 1; i >= 0; i--)
            {
                if (stageBricks[groundId][i] != null && stageBricks[groundId][i].colorData.id == id)
                {
                    Destroy(stageBricks[groundId][i].gameObject);
                    stageBricks[groundId].Remove(stageBricks[groundId][i]);
                }
            }

                
        }
    }
}
