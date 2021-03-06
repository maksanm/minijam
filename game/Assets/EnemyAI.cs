﻿using System.Collections;
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
    int spawnNumber = 0;
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

        Vector2 DestionationCoords = sectors.RandomEdgePosition();

        int choise = 0;
        int randomValue = Random.Range(0, 100);

        if (randomValue >= 95)
            choise = 2;
        else if (randomValue < 95 && randomValue >= 80)
            choise = 1;
        else if (randomValue < 80)
            choise = 0;
       

        GameObject NewEnemyShip = Instantiate(Squads[choise], new Vector3(DestionationCoords.x, DestionationCoords.y, 0), Quaternion.identity);

        NewEnemyShip.GetComponent<SquadController>().GetCommand(stationContoller.gameObject.transform.position, "move");

        yield return new WaitForSecondsRealtime(cooldownTime);
        isCooldown = false;
        if (cooldownSpawnTime > 3)
            cooldownSpawnTime -= 0.05f;
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
