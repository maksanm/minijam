using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUI : MonoBehaviour
{
    public TileBase TileToSetDefault;
    public TileBase TileToSetBlue;
    public TileBase TileToSetRed;

    private TileBase OldTile;

    private Tilemap map;
    private Camera mainCamera;

    private SquadController[] Squads;

    // Start is called before the first frame update
    void Start()
    {
        Squads = FindObjectsOfType<SquadController>();

        map = GetComponent<Tilemap>();
        mainCamera = Camera.main;
    }

    public void RefreshCell(Vector3Int NewCellPosition, Vector3Int OldCellPosition, string team)
    {

        if (team == "Allies")
        {
            map.SetTile(OldCellPosition, TileToSetDefault);
            map.SetTile(NewCellPosition, TileToSetBlue);
        }
        else if (team == "Enemy")
        {
            map.SetTile(OldCellPosition, TileToSetDefault);
            map.SetTile(NewCellPosition, TileToSetRed);
        }
        else if (team == "None")
        {
            map.SetTile(NewCellPosition, TileToSetDefault);
        }
    }

    public void FillCell(Vector3Int Position, string team)
    {

        if (team == "Allies")
            map.SetTile(Position, TileToSetBlue);
        else if (team == "Enemy")
            map.SetTile(Position, TileToSetRed);
        else if (team == "None")
            map.SetTile(Position, TileToSetDefault);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);


            Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

            Debug.Log(map.CellToWorld(clickCellPosition));
        }
    }
}
