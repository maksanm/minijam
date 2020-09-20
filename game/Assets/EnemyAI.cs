using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject SquadLight;
    public GameObject SquadMedium;
    public GameObject SquadHard;

    private SectorsArray sectors;

    public float cooldownTime = 1f;

    private bool isCooldown;


    // Start is called before the first frame update
    void Start()
    {
        sectors = FindObjectOfType<SectorsArray>();
    }

    private IEnumerator SpawnEnemy(float cooldownTime)
    {
        isCooldown = true;

        Vector2 DestionationCoords = sectors.RandomEdgePosition();

     //   GameObject NewEnemyShip = Instantiate(SquadLight, DestionationCoords);

        yield return new WaitForSecondsRealtime(cooldownTime);
        isCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            SpawnEnemy(cooldownTime);
        }
    }
}
