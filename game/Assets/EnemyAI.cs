using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject SquadLight;
    public GameObject SquadMedium;
    public GameObject SquadHard;

    private GameObject[] Squads;

    private SectorsArray sectors;
    private StationContoller stationContoller; 

    public float cooldownSpawnTime = 1f;

    private bool isCooldown;


    // Start is called before the first frame update
    void Start()
    {
        sectors = FindObjectOfType<SectorsArray>();
        stationContoller = FindObjectOfType<StationContoller>();

        Squads = new GameObject[]{SquadLight, SquadMedium, SquadHard };
    }

    private IEnumerator SpawnEnemy(float cooldownTime)
    {
        isCooldown = true;
        Debug.Log("SpawnEnemy... ");

        Vector2 DestionationCoords = sectors.RandomEdgePosition();

        int chose = 0;
        int randomValue = Random.Range(0, 100);

        if (randomValue >= 95)
            chose = 2;
        else if (randomValue < 95 && randomValue >= 80)
            chose = 1;
        else if (randomValue < 80)
            chose = 0;
       

        GameObject NewEnemyShip = Instantiate(Squads[chose], new Vector3(DestionationCoords.x, DestionationCoords.y, 0), Quaternion.identity);
        Debug.Log(NewEnemyShip);
        Debug.Log(stationContoller);
        Debug.Log(stationContoller.gameObject.transform.position);
        NewEnemyShip.GetComponent<SquadController>().GetCommand(stationContoller.gameObject.transform.position, "move");

        yield return new WaitForSecondsRealtime(cooldownTime);
        isCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            StartCoroutine(SpawnEnemy(cooldownSpawnTime));
        }
    }
}
