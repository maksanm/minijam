using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SquadController : MonoBehaviour
{
    public float visionRange;
    public float engageRange;

    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    public float angularSpeed = 0.02f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private Vector2 target;
    private Vector2 screenPosition;
    private Vector2 worldPosition;

    private float Rotation;


    private bool isEngage;


    private State currentState;
    private enum State
    {
        Waiting,
        Defend,
        Moving,
        Engage,

    }

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

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

    void UpdateWaitingState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }
    }
    void UpdateDefendState()
    {
    }
    void UpdateMovingState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            currentState = State.Waiting;
            Debug.Log(currentState);

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
    void UpdateEngageState()
    {
    }

    private void Move()
    {
        // Changing current state to Moving
        currentState = State.Moving;
        Debug.Log(currentState);

        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Grid grid;
        grid = FindObjectOfType<Grid>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);

        Debug.Log(grid.CellToWorld(position).x + 1.65f + " " + grid.CellToWorld(position).y + 1.65f);

        target = new Vector2(grid.CellToWorld(position).x + 1.65f, grid.CellToWorld(position).y + 1.65f);

        SetAnim(1);
        seeker.StartPath(transform.position, target, OnPathComplete);
    }

    //private void SwitchState(State state)
    //{
    //    currentState = state;
    //}

    void SetAnim(int AnimState)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetInteger("MoveState", AnimState);
        }
    }

    void FixedUpdate()
    {
        

    }

}
