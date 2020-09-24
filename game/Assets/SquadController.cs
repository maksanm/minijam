using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SquadController : MonoBehaviour
{
    [Header("Dev")]
    public bool controlByMouse;

    [Header("Combat")]
    public float damage;
    public float lifetimeProjectile;
    public float attackCooldown;
    public float thirstPower;
    [HideInInspector]
    public float stepCooldown;

    [Header("Range")]
    public int visionRange;
    public int engageRange;

    [Header("Move")]
    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    [Header("Effects")]
    public float launchForce;
    public GameObject hitParticleSystem;
    public GameObject damageProjectile;
    
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D collider2d;

    private Vector3Int currentCellPosition;

    private Vector2 target;
    private Vector2 screenPosition;
    private Vector2 worldPosition;
    private Grid grid;

    private float Rotation;

    private string team;
    private bool isEngage;
    private bool isCooldown;
    private bool isKretin;

    private bool isAwaked;

    private GameObject Enemy;
    private Queue<GameObject> Enemies = new Queue<GameObject>();

    private SquadController[] ListOfEnemies;

    private TilemapUI Tilemap;

    private State currentState;

    AudioSource shotAudio;
    AudioSource boomAudio;

    public enum State
    {
        Waiting,
        Regroup,
        Moving,
        Engage,
    }

    private int ButtonTmp;

    void Awake()
    {

        target = Vector2.zero;

        currentState = State.Waiting;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        grid = FindObjectOfType<Grid>();
        Tilemap = FindObjectOfType<TilemapUI>();

        team = tag;

        stepCooldown = attackCooldown / transform.childCount;

        StartCoroutine(FillCell());

        AudioSource[] audio = GetComponents<AudioSource>();
        shotAudio = audio[0];
        boomAudio = audio[1];
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private IEnumerator FillCell()
    {
        isKretin = true;
        yield return new WaitForSecondsRealtime(0.1f);
        currentCellPosition = grid.WorldToCell(transform.position);
        CheckFillCell(currentCellPosition, tag);
        isKretin = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Waiting:
                UpdateWaitingState();
                break;
            case State.Regroup:
                UpdateRegroupState();
                break;
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Engage:
                UpdateEngageState();
                break;
        }
    }

    //  --- Begin states

    

    // Enter state
    private void EnterWaitingState()
    {
        // Changing current state
        currentState = State.Waiting;

        collider2d.isTrigger = false;
        currentWaypoint = 0;
        SetAnim(0);
    }
    private void EnterRegroupState()
    {
        // Changing current state
        currentState = State.Regroup;

    }
    private void EnterMovingState(Vector2 movetarget)
    {
        // Changing current state
        currentState = State.Moving;

        collider2d.isTrigger = true;
        SetAnim(1);
        seeker.StartPath(transform.position, movetarget, OnPathComplete);
    }
    private void EnterEngageState()
    {
        // Changing current state
        currentState = State.Engage;
        //

        collider2d.isTrigger = false;
    }

    // Update state
    private void UpdateWaitingState()
    {
        if (transform.childCount == 0)
        {
            Die();
        }
        else
        {
            CheckEnemies();
            StartCoroutine(FillCell());

            if (target != Vector2.zero)
            {
                SwitchState(State.Moving,target);
            }
        }
    }
    private void UpdateRegroupState()
    {
    }
    private void UpdateMovingState()
    {
         if (path == null)
         {
            return;
         }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            StartCoroutine(FillCell());
            SwitchState(State.Waiting);
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;


            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            Rotation = Mathf.Atan2(path.vectorPath[currentWaypoint].y - transform.position.y, path.vectorPath[currentWaypoint].x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                Vector3Int currentPos = grid.WorldToCell(new Vector2(transform.position.x, transform.position.y));
                CheckFillCellMove(currentPos, tag);
                currentWaypoint++;
            }
        }
        
    }
    private void UpdateEngageState()
    {
        Vector3Int currentPos = grid.WorldToCell(new Vector2(transform.position.x, transform.position.y));

        // Changing color of Grid Cell
        if (currentState == State.Moving)
            CheckFillCellMove(currentPos, tag);
        //


        int distance = 1;

        CheckEnemies();


        if (Enemies.Count != 0 && !Enemy)
        {
            Enemy = Enemies.Dequeue();
        }


        if (Enemies.Count != 0 && Enemy || Enemies.Count == 0 && Enemy)
            distance = CellDistance(transform.position, Enemy.transform.position);

        if (Enemy && distance <= engageRange && transform.childCount != 0)
        {
            Rotation = Mathf.Atan2(Enemy.transform.position.y - transform.position.y,
                                   Enemy.transform.position.x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);

            if (damageProjectile)
            { 
                StartCoroutine(LaunchProjectile());
            }
        }
        else if (Enemy && distance > engageRange && transform.childCount != 0)
        {

            Rotation = Mathf.Atan2(Enemy.transform.position.y - transform.position.y,
                                   Enemy.transform.position.x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);

            Vector2 direction = ((Vector2)Enemy.transform.position - (Vector2)transform.position).normalized;
            Vector2 force = direction * speed/2 * Time.deltaTime;
            rb.AddForce(force);
        }
        else if (transform.childCount == 0)
        {
            Die();
        }
        else if (!Enemy && Enemies.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0);


            // After fight fly to the center of cell
            if (currentCellPosition != grid.WorldToCell(transform.position))
                CheckFillCell(currentCellPosition, "None");


            currentCellPosition = grid.WorldToCell(transform.position);
            Vector2 targetV2 = new Vector2(grid.CellToWorld(currentCellPosition).x + 1.65f, grid.CellToWorld(currentCellPosition).y + 1.65f);

            SwitchState(State.Moving, targetV2);

            CheckFillCell(currentCellPosition, tag);
        }
    }

    // Exit state
    private void ExitWaitingState()
    {

    }
    private void ExitRegroupState()
    {

    }
    private void ExitMovingState()
    {
        if (CellDistance((Vector3)transform.position,(Vector3)target) == 0)
            target = Vector2.zero;
    }
    private void ExitEngageState()
    {

    }

    private void SwitchState(State newState, Vector2 movetarget = default(Vector2))
    {
        switch (currentState)
        {
            case State.Waiting:
                ExitWaitingState();
                break;
            case State.Regroup:
                ExitRegroupState();
                break;
            case State.Moving:
                ExitMovingState();
                break;
            case State.Engage:
                ExitEngageState();
                break;
        }
        switch (newState)
        {
            case State.Waiting:
                EnterWaitingState();
                break;
            case State.Regroup:
                EnterRegroupState();
                break;
            case State.Moving:
                EnterMovingState(movetarget);
                break;
            case State.Engage:
                EnterEngageState();
                break;
        }
    }



    //  --- End states


    private int CellDistance(Vector3 Origin, Vector3 Destination)
    {
        Vector3Int enemyCellPosition = grid.WorldToCell(new Vector2(Origin.x, Origin.y));
        Vector3Int alliseCellPosition = grid.WorldToCell(new Vector2(Destination.x, Destination.y));

        int distance = Mathf.FloorToInt(Vector2.Distance(new Vector2(enemyCellPosition.x, enemyCellPosition.y),
                                                         new Vector2(alliseCellPosition.x, alliseCellPosition.y)));

        return distance;
    }

    private IEnumerator LaunchProjectile()
    {
        if (!isCooldown && (Enemy.transform.childCount != 0 && transform.childCount != 0))
        {
            isCooldown = true;

            int RandomOrigin;
            int RandomTarget;

            Vector2 OriginPosition;
            Vector2 TargetPosition;

            Transform OriginTransform;

            float RotationAlly;

            RandomOrigin = Random.Range(0, transform.childCount);
            RandomTarget = Random.Range(0, Enemy.transform.childCount);

            OriginTransform = transform.GetChild(RandomOrigin);

            OriginPosition = OriginTransform.position;
            TargetPosition = Enemy.transform.GetChild(RandomTarget).position;

            RotationAlly = Mathf.Atan2(TargetPosition.y - OriginPosition.y,
                                       TargetPosition.x - OriginPosition.x);

            OriginTransform.rotation = Quaternion.Euler(0, 0, RotationAlly * Mathf.Rad2Deg);

            Vector2 OriginFirepoint = OriginTransform.GetChild(0).GetChild(Random.Range(0, OriginTransform.GetChild(0).childCount)).position;

            GameObject launchedProjectile = Instantiate(damageProjectile, OriginFirepoint, Quaternion.Euler(0, 0, RotationAlly * Mathf.Rad2Deg));
            BeamProjectile beam = launchedProjectile.GetComponent<BeamProjectile>();
            if (beam)
            {
                shotAudio.Play();
                beam.team = tag;
                beam.damage = damage;
                beam.thirstPower = thirstPower;
                beam.lifetime = lifetimeProjectile;
            }
            launchedProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(RotationAlly), Mathf.Sin(RotationAlly)) * launchForce;

            yield return new WaitForSecondsRealtime(attackCooldown);
            isCooldown = false;
        }
    }

    // GameObject need to get Enemy (transform and etc.)
    public void CallEngage(GameObject enemy)
    {
        SwitchState(State.Engage);
        Enemies.Enqueue(enemy);
    }

    public void GetCommand(Vector2 target, string command)
    {
        if (command == "move")
        {
            this.target = target;
            SwitchState(State.Moving, target);
        }
    }

    public void PlayExplosion()
    {
        boomAudio.Play();
    }

    private void Die()
    {
        boomAudio.Play();
        CheckFillCellMove(currentCellPosition, "None");
        DestroyImmediate(gameObject);
    }

    private void CheckEnemies()
    {
        Vector2 targetTmp;
        Vector3Int targetPos;

        int distance;
        float minDistance = Mathf.Infinity;
        int minDistanceIndex = -1;

        ListOfEnemies = FindObjectsOfType<SquadController>();
       
        for (int i = 0; i < ListOfEnemies.Length; i++)
        {
            if (ListOfEnemies[i].tag == tag)
                continue;

            targetTmp = new Vector2(ListOfEnemies[i].gameObject.transform.position.x, ListOfEnemies[i].gameObject.transform.position.y);
            targetPos = grid.WorldToCell(targetTmp);

            distance = CellDistance(transform.position, ListOfEnemies[i].gameObject.transform.position);

            if (distance < minDistance && !Enemies.Contains(ListOfEnemies[i].gameObject))
            {
                minDistance = distance;
                minDistanceIndex = i;
            }
            
        }

        if (tag == "Enemy" && !Enemies.Contains(FindObjectOfType<StationContoller>().gameObject))
        {
            distance = CellDistance(transform.position, FindObjectOfType<StationContoller>().transform.position);
            if ((distance < minDistance || minDistance == -1) && distance <= engageRange)
            {
                Enemies.Enqueue(FindObjectOfType<StationContoller>().gameObject);
                if (currentState != State.Engage)
                {
                    SwitchState(State.Engage);
                }
            }
        }

        // If enemy in EngageRange -> Initiate Battle
        if (minDistanceIndex != -1 && minDistance <= engageRange)
        {
            ListOfEnemies[minDistanceIndex].gameObject.GetComponent<SquadController>().CallEngage(gameObject);

            Enemies.Enqueue(ListOfEnemies[minDistanceIndex].gameObject);

            if (currentState != State.Engage)
            {
                SwitchState(State.Engage);
            }
                
        }

        if (minDistanceIndex != -1 && (visionRange >= minDistance))
        {
            Rotation = Mathf.Atan2(ListOfEnemies[minDistanceIndex].gameObject.transform.position.y - transform.position.y,
                                   ListOfEnemies[minDistanceIndex].gameObject.transform.position.x - transform.position.x);

            transform.rotation = Quaternion.Euler(0, 0, Rotation * Mathf.Rad2Deg);
        }

    }

    private void CheckFillCellMove(Vector3Int NewCurrentPos, string team)
    {
        if (NewCurrentPos != currentCellPosition)
        {
            if (Tilemap)
            {
                Tilemap.RefreshCell(NewCurrentPos, currentCellPosition, team);
            }
            currentCellPosition = NewCurrentPos;
        }
        else if (team == "None")
            Tilemap.RefreshCell(NewCurrentPos, NewCurrentPos, team);
        else if (team == "Engage")
            Tilemap.RefreshCell(NewCurrentPos, NewCurrentPos, team);
    }

    private void CheckFillCell(Vector3Int Pos, string team)
    {
        if (Tilemap)
        {
            if (team == "None")
            {
                Tilemap.FillCell(Pos, team);
            }
            else
                Tilemap.FillCell(Pos, team);
        }
        currentCellPosition = Pos;
    }

    private Vector3Int GetCellInGrid(Vector2 ObjectPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(ObjectPosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);

        return position;
    }


    void SetAnim(int AnimState)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetInteger("MoveState", AnimState);
        }
    }


}
