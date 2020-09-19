using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUI : MonoBehaviour
{
    public TileBase TileToSetDefault;
    public TileBase TileToSetEngage;
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

    public void RefreshCell(Vector3Int NewCellPosition, Vector3Int OldCellPosition, int engageRange, string team)
    {
        int width = 2 * engageRange + 1;

        if (team == "Allies")
        {
            int PosXTmpOld = OldCellPosition.x;
            int PosXTmpNew = NewCellPosition.x;

            int PosYTmpOld = OldCellPosition.y;
            int PosYTmpNew = NewCellPosition.y;

            if ((NewCellPosition.y < OldCellPosition.y) && engageRange != 0)
                PosYTmpOld += 2 * engageRange;
            if ((NewCellPosition.x > OldCellPosition.x) && engageRange != 0)
                PosXTmpOld -= 2 * engageRange;

            for (int i = 0; i < width; i++)
            {
                OldCellPosition.y = PosYTmpOld + engageRange;
                NewCellPosition.y = PosYTmpNew + engageRange;

                OldCellPosition.y -= i;
                NewCellPosition.y -= i;

                for (int j = 0; j < width; j++)
                {
                    OldCellPosition.x = PosXTmpOld - engageRange;
                    NewCellPosition.x = PosXTmpNew - engageRange;

                    OldCellPosition.x += j;
                    NewCellPosition.x += j;

                    map.SetTile(OldCellPosition, TileToSetDefault);
                    map.SetTile(NewCellPosition, TileToSetBlue);
                }

            }
        }
        else if (team == "Enemy")
        {
            int PosXTmpOld = OldCellPosition.x;
            int PosXTmpNew = NewCellPosition.x;

            int PosYTmpOld = OldCellPosition.y;
            int PosYTmpNew = NewCellPosition.y;

            if ((NewCellPosition.y < OldCellPosition.y) && engageRange != 0)
                PosYTmpOld += 2*engageRange;
            if ((NewCellPosition.x > OldCellPosition.x) && engageRange != 0)
                PosXTmpOld -= 2*engageRange;



            for (int i = 0; i < width; i++)
            {
                OldCellPosition.y = PosYTmpOld + engageRange;
                NewCellPosition.y = PosYTmpNew + engageRange;

                OldCellPosition.y -= i;
                NewCellPosition.y -= i;

                for (int j = 0; j < width; j++)
                {
                    OldCellPosition.x = PosXTmpOld - engageRange;
                    NewCellPosition.x = PosXTmpNew - engageRange;

                    OldCellPosition.x += j;
                    NewCellPosition.x += j;

                    map.SetTile(OldCellPosition, TileToSetDefault);
                    map.SetTile(NewCellPosition, TileToSetRed);
                }

            }
        }
        else if (team == "None")
        {
            int PosXTmpOld = OldCellPosition.x;
            int PosXTmpNew = NewCellPosition.x;

            int PosYTmpOld = OldCellPosition.y;
            int PosYTmpNew = NewCellPosition.y;

            for (int i = 0; i < width; i++)
            {

                OldCellPosition.y = PosYTmpOld + engageRange;
                NewCellPosition.y = PosYTmpNew + engageRange;

                OldCellPosition.y -= i;
                NewCellPosition.y -= i;

                for (int j = 0; j < width; j++)
                {
                    OldCellPosition.x = PosXTmpOld - engageRange;
                    NewCellPosition.x = PosXTmpNew - engageRange;

                    OldCellPosition.x += j;
                    NewCellPosition.x += j;

                    map.SetTile(NewCellPosition, TileToSetDefault);
                }

            }
        }
        else if (team == "Engage")
        {
            int PosXTmpOld = OldCellPosition.x;
            int PosXTmpNew = NewCellPosition.x;

            int PosYTmpOld = OldCellPosition.y;
            int PosYTmpNew = NewCellPosition.y;

            for (int i = 0; i < width; i++)
            {

                OldCellPosition.y = PosYTmpOld + engageRange;
                NewCellPosition.y = PosYTmpNew + engageRange;

                OldCellPosition.y -= i;
                NewCellPosition.y -= i;

                for (int j = 0; j < width; j++)
                {
                    OldCellPosition.x = PosXTmpOld - engageRange;
                    NewCellPosition.x = PosXTmpNew - engageRange;

                    OldCellPosition.x += j;
                    NewCellPosition.x += j;

                    map.SetTile(NewCellPosition, TileToSetEngage);
                }

            }
        }
    }

    public void FillCell(Vector3Int Position, int engageRange, string team)
    {
        int width = 2 * engageRange + 1;

        int PosXTmpNew = Position.x;
        int PosYTmpNew = Position.y;

        for (int i = 0; i < width; i++)
        {
            Position.y = PosYTmpNew + engageRange;
            Position.y -= i;

            for (int j = 0; j < width; j++)
            {
                Position.x = PosXTmpNew - engageRange;
                Position.x += j;



                if (team == "Allies")
                    map.SetTile(Position, TileToSetBlue);
                else if (team == "Enemy")
                    map.SetTile(Position, TileToSetRed);
            }

        }

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);

            Debug.Log(clickCellPosition);
            
            map.SetTile(clickCellPosition, TileToSetBlue);
        }
    }
}
