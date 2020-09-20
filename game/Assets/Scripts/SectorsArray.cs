using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectorsArray : MonoBehaviour
{
    [HideInInspector]
    public Grid grid;

    private Vector2[,] sectorsWorld = new Vector2[20, 20];


    // Po4ti univeraslno, no niezvestno kak polu4it FirstXY
    void Start()
    {
        grid = GetComponentInParent<Grid>();

        //Vector2 startPointOffset = grid.transform.position;
        Vector2 FirstXY = new Vector2(-19.3f,0f);
        Debug.Log(grid);
        float step = grid.cellSize.x;

        //for (int i = 0; i < 20; i++)
        //{
        //    for (int j = 0; j < 20; j++)
        //    {
        //        sectors[i, j] = new Vector2(-20.67f + 64.55f / 40 + 64.55f * i / 20, -62.65f + 64.55f / 40 + 64.55f * j / 20);
        //        Debug.Log(sectors[i, j]);
        //    }
        //}


        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Vector2 FixedPosition = ConvertPositionToCenterCell(FirstXY);
                sectorsWorld[i, j] = new Vector2(FixedPosition.x, FixedPosition.y);
                FirstXY = FixedPosition;

                FirstXY.x += step;
                //Debug.Log(sectorsWorld[i, j]);
            }
            FirstXY.x = -19.3f;
            FirstXY.y -= step;
        }
    }
    // 61,4 length, x0 = -19,4 , y0 = 0.

    private int CharToIndex(char letter)
    {
        int index;

        index  = (int)(letter - '0')-17;

        return index;
    }

    public Vector2 ConvertArgumentToCoords(string argument)
    {
        Vector2 Coordinates;


        char letter = char.ToUpper(argument[0]);
        int IndexRow = Int32.Parse(argument.Substring(1, argument.Length-1)) - 1;
        int IndexColumn = CharToIndex(letter);

        Coordinates = sectorsWorld[IndexRow,IndexColumn];

        Debug.Log(Coordinates);

        return Coordinates;
    }

    public Vector2 ConvertPositionToCenterCell(Vector2 rawCoords)
    {
        Vector3Int cellCoordinates;
        Vector2 coordinates;

        cellCoordinates = grid.WorldToCell(rawCoords);
        coordinates = new Vector2(grid.CellToWorld(cellCoordinates).x + (grid.cellSize.x/2), grid.CellToWorld(cellCoordinates).y + (grid.cellSize.x / 2));

        return coordinates;
    }

    void Update()
    {
        
    }
}
