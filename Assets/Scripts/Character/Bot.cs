using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Bot : MonoBehaviour
{
    #region comments
    [SerializeField] public Transform skin;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float gravityScale;
    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private GameObject brickStacker;
    [SerializeField] private GameObject playerBrickPrefab;
    [SerializeField] private GameObject stairsBrickPrefab;
    [SerializeField] private LayerMask stairsMask;
    [SerializeField] private Transform targetForBot;
 

    private Rigidbody rb;
    private Vector3 direction;
    private Brick brick;
    private PlayerBrick playerBrick;
    private StairsBrick stairsBrick;
    private GameObject stairs;
    public ColorData colorData;
    public List<PlayerBrick> bricksCollected = new List<PlayerBrick>();
    private Ray ray;
    private RaycastHit hit;
    private int stairsBrickId = -1;
    public int currentStage = 0;
    private BrickSpawner brickSpawner;
    private IState currentState;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colorData = ColorChanger.Instance.GetColorData(3);
        skin.GetComponent<MeshRenderer>().material = colorData.material;
        brickSpawner = FindObjectOfType<BrickSpawner>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ChangeState(new IdleState());
        

    }

    private void FixedUpdate()
    {
        ray = new Ray(transform.position + direction * 0.2f, new Vector3(0, -1, 0));
        Physics.Raycast(ray, out hit, 3f);
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);

        CheckForStairs();
        CheckForBrickStairs();


        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    public Transform SetGroundTarget()
    {
        return targetForBot;
    }

    public Transform SetBrickTarget()
    {
        Transform brickTarget;
        for (int i = 0; i < brickSpawner.stageBricks[currentStage].Count; i++)
        {
            if (brickSpawner.stageBricks[currentStage][i] != null && brickSpawner.stageBricks[currentStage][i].colorData.id == colorData.id)
            {
                brickTarget = brickSpawner.stageBricks[currentStage][i].transform;
                return brickTarget;
            }
        }
        return null;
    }

    public void GoTowardsTarget(Transform target)
    {
        navMeshAgent.destination = target.position;
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    private void CheckForBrickStairs()
    {
        if (hit.collider.CompareTag(MyConst.Tag.STAIRSBRICK))
        {
            stairsBrick = hit.collider.gameObject.GetComponent<StairsBrick>();
            stairsBrickId = stairsBrick.colorData.id;

            if (stairsBrickId != colorData.id)
            {
                if (bricksCollected.Count > 0)
                {
                    ReplaceBrick();
                }
            }
        }
    }

    private void CollectBrick(Brick brick)
    {
        GameObject brickPrefab = Instantiate(playerBrickPrefab, brickStacker.transform.position, Quaternion.identity);
        playerBrick = brickPrefab.GetComponent<PlayerBrick>();
        bricksCollected.Add(playerBrick);
        playerBrick.SetColor(colorData.id);
        brickPrefab.transform.SetParent(brickStacker.transform);
        brickPrefab.transform.forward = transform.forward;
        brickPrefab.transform.position = new Vector3(brickPrefab.transform.position.x, brickPrefab.transform.position.y + heightOffset * bricksCollected.Count, brickPrefab.transform.position.z);

        brickSpawner.DestroyBrick(colorData.id, currentStage, brick.brickObject);
    }

    private void SetBrick()
    {
        GameObject brickToBeDestroyed = bricksCollected[bricksCollected.Count - 1].brickObject;
        Destroy(brickToBeDestroyed);

        bricksCollected.RemoveAt(bricksCollected.Count - 1);

        GameObject brickPrefab = Instantiate(stairsBrickPrefab, stairs.transform.position + new Vector3(0f, 0f, 0f), stairsBrickPrefab.transform.rotation);
        brickPrefab.GetComponent<StairsBrick>().SetColor(colorData.id);

        Destroy(stairs);

        brickSpawner.SpawnBricks(colorData.id, Random.Range(2, 3), currentStage);
    }

    private void ReplaceBrick()
    {
        GameObject brickToBeDestroyed = bricksCollected[bricksCollected.Count - 1].brickObject;
        Destroy(brickToBeDestroyed);

        bricksCollected.RemoveAt(bricksCollected.Count - 1);

        GameObject brickPrefab = Instantiate(stairsBrickPrefab, hit.collider.transform.position, stairsBrickPrefab.transform.rotation);
        brickPrefab.GetComponent<StairsBrick>().SetColor(colorData.id);

        Destroy(hit.collider.gameObject);

        brickSpawner.SpawnBricks(colorData.id, Random.Range(3, 4), currentStage);
    }

    private bool CanMoveForward(float forwardMove)
    {
        if (hit.collider == null || forwardMove < 0)
        {
            return true;
        }

        if (hit.collider.CompareTag(MyConst.Tag.STAIRS))
        {
            return false;
        }

        if (hit.collider.CompareTag(MyConst.Tag.STAIRSBRICK))
        {
            stairsBrick = hit.collider.gameObject.GetComponent<StairsBrick>();
            stairsBrickId = stairsBrick.colorData.id;

            if (stairsBrickId != colorData.id)
            {
                if (bricksCollected.Count > 0)
                {
                    ReplaceBrick();
                    return true;
                }
                return false;
            }

        }

        return true;
    }

    private void CheckForStairs()
    {
        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.gameObject.CompareTag(MyConst.Tag.STAIRS))
        {
            stairs = hit.collider.gameObject;

            if (bricksCollected.Count > 0)
            {
                SetBrick();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(MyConst.Tag.BRICK))
        {
            brick = other.GetComponent<Brick>();
            if (brick.colorData.id == colorData.id)
            {
                CollectBrick(brick);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Ground ground = collision.gameObject.GetComponent<Ground>();
            if (currentStage != ground.id)
            {
                currentStage = ground.id;

                brickSpawner.SpawnBricks(colorData.id, 5, currentStage);
                if (currentStage > 0)
                {
                    brickSpawner.ClearBricks(colorData.id, currentStage - 1);
                }
            }
        }
    }
    #endregion
}
