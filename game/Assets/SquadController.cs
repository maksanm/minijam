using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SquadController : MonoBehaviour
{
    [Header("Combat")]
    public float damage;
    public float attackCooldown;

    [Header("Range")]
    public float visionRange;
    public float engageRange;

    [Header("Move")]
    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    [Header("Effects")]
    public float launchForce;
    public GameObject damageProjectile;
    
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private Vector2 target;
    private Vector2 screenPosition;
    private Vector2 worldPosition;
    private Grid grid;

    private float Rotation;

    private string team;
    private bool isEngage;


    private GameObject Enemy;
    private SquadController[] ListOfEnemies;

    private State currentState;
    private enum State
    {
        Waiting,
        Defend,
        Moving,
        Engage,

    }

    private int ButtonTmp;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Waiting;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        grid = FindObjectOfType<Grid>();

        team = tag;

        if (team == "Allies")
            ButtonTmp = 1;
        else
            ButtonTmp = 0;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Waiting:
                UpdateWaitingState();
                break;
            case State.Defend:
                UpdateDefendState();
                break;
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Engage:
                UpdateEngageState();
                break;
        }

   

    
    }

    private void UpdateWaitingState()
    {
        if (Input.GetMouseButtonDown(ButtonTmp))
        {
            StartMove();
        }
        else
        {
            CheckEnemies();
        }
    }
    private void UpdateDefendState()
    {
    }
    private void UpdateMovingState()
    {
        if (Input.GetMouseButtonDown(ButtonTmp))
        {
            StartMove();
        }
        else
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                currentState = State.Waiting;

                currentWaypoint = 0;

                SetAnim(0);
                reachedEndOfPath = true;
                return;
            }
            else
            {
                SetAnim(1);
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            Rotation = Mathf.Atan2(path.vectorPath[currentWaypoint].y - transform.position.y, path.vectorPath[currentWaypoint].x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);


            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }
    private void UpdateEngageState()
    {
        if (Enemy)
        {
            Rotation = Mathf.Atan2(Enemy.transform.position.y - transform.position.y,
                                   Enemy.transform.position.x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);

            //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            //Vector2 force = direction * speed * Time.deltaTime;

            int RandomOrigin;
            int RandomTarget;

            Vector2 OriginPosition;
            Vector2 TargetPosition;
            float RotationAlly;

            if (damageProjectile)
            { 

                RandomOrigin = Random.Range(0, transform.childCount);
                RandomTarget = Random.Range(0, Enemy.transform.childCount);

                OriginPosition = transform.GetChild(RandomOrigin).position;
                TargetPosition = Enemy.transform.GetChild(RandomTarget).position;

                RotationAlly = Mathf.Atan2(TargetPosition.y - OriginPosition.y,
                                           TargetPosition.x - OriginPosition.x);

                transform.GetChild(RandomOrigin).rotation = Quaternion.Euler(0, 0, RotationAlly * Mathf.Rad2Deg);

                LaunchProjectile(OriginPosition, TargetPosition, RotationAlly);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).rotation = new Quaternion(0, 0, 0, 1);

            currentState = State.Waiting;
        }
    }
    
    private IEnumerator LaunchProjectile(Vector2 origin, Vector2 target, float angle)
    {
       
        GameObject launchedProjectile = Instantiate(damageProjectile, origin, new Quaternion(0,0,angle*Mathf.Rad2Deg, 1));
        launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * launchForce;
        Debug.Log("OK");
        yield return new WaitForSecondsRealtime(attackCooldown);
    }

    // GameObject need to get Enemy (transform and etc.)
    public void CallEngage(GameObject enemy)
    {
        currentState = State.Engage;
        Enemy = enemy;
    }

    private void CheckEnemies()
    {

        Vector2 targetTmp;
        Vector3Int targetPos;

        Vector3Int currentPos = grid.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0));
        float distance;
        float minDistance = Mathf.Infinity;
        int minDistanceIndex = -1;

        ListOfEnemies = FindObjectsOfType<SquadController>();
        for (int i = 0; i < ListOfEnemies.Length; i++)
        {
            if (ListOfEnemies[i].tag == tag)
                continue;

            targetTmp = new Vector2(ListOfEnemies[i].gameObject.transform.position.x, ListOfEnemies[i].gameObject.transform.position.y);
            targetPos = grid.WorldToCell(targetTmp);

            distance = Vector2.Distance(new Vector2(currentPos.x, currentPos.y), new Vector2(targetPos.x, targetPos.y));

            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceIndex = i;
            }
            
        }

        // If enemy in EngageRange -> Initiate Battle
        if (minDistanceIndex != -1 && minDistance == engageRange)
        {
            Enemy = ListOfEnemies[minDistanceIndex].gameObject;
            currentState = State.Engage;
        }

        // If enemy in VisionRange -> Can be attacked in DefendMode \\ In WaitingMode just watch on enemy
        if (minDistanceIndex != -1 && (visionRange >= minDistance || visionRange + (Mathf.Sqrt(2)-1) == minDistance))
        {
            Rotation = Mathf.Atan2(ListOfEnemies[minDistanceIndex].gameObject.transform.position.y - transform.position.y,
                                   ListOfEnemies[minDistanceIndex].gameObject.transform.position.x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);
        }

    }

    private Vector3Int GetCellInGrid(Vector2 ObjectPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(ObjectPosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);

        return position;
    }

    private void StartMove()
    {
        // Changing current state to Moving
        currentState = State.Moving;
        Debug.Log(currentState);

        screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector3Int position = GetCellInGrid(screenPosition);

        // Converting to WorldCoordinates
        target = new Vector2(grid.CellToWorld(position).x + 1.65f, grid.CellToWorld(position).y + 1.65f);

        SetAnim(1);
        seeker.StartPath(transform.position, target, OnPathComplete);
    }


    void SetAnim(int AnimState)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetInteger("MoveState", AnimState);
        }
    }


}
