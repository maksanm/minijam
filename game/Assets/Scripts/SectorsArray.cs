using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SectorsArray : MonoBehaviour
{
    public Transform CanvasRoot;
    public Text TextPrefab;

    [HideInInspector]
    public Grid grid;

    private Tilemap tilemap;

    private Vector2[,] sectorsWorld;


    void Start()
    {
        grid = GetComponentInParent<Grid>();
        tilemap = GetComponent<Tilemap>();

        sectorsWorld = new Vector2[tilemap.size.x, tilemap.size.y];
        Vector2 FirstXY = new Vector2(tilemap.CellToWorld(tilemap.origin).x,
                                           tilemap.CellToWorld(new Vector3Int(tilemap.origin.x,tilemap.origin.y+tilemap.size.y-1, 
                                                               tilemap.origin.z)).y);
        Debug.Log(FirstXY);

        float step = grid.cellSize.x;

        Debug.Log(GetComponent<Tilemap>().CellToWorld(GetComponent<Tilemap>().origin));

        for (int i = 0; i < tilemap.size.x; i++)
        {
            for (int j = 0; j < tilemap.size.y; j++)
            {
                Vector2 FixedPosition = ConvertPositionToCenterCell(FirstXY);
                sectorsWorld[i, j] = new Vector2(FixedPosition.x, FixedPosition.y);

                TextCellNumerate(i+1, j, FixedPosition);

                FirstXY = FixedPosition;

                FirstXY.x += step;
            }
            FirstXY.x = tilemap.CellToWorld(tilemap.origin).x;
            FirstXY.y -= step;
        }
    }

    private void TextCellNumerate(int row, int column, Vector2 position)
    {
        float offset = 0.5f;

        Text label = Instantiate<Text>(TextPrefab);
        label.rectTransform.SetParent(CanvasRoot.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x+tilemap.cellSize.x/2-offset, 
                                                           position.y - tilemap.cellSize.y / 2+offset);
        label.text = IndexToChar(column) + row.ToString();
    }

    private char IndexToChar(int index)
    {
        char letter;

        letter = (char)(index + 65);

        return letter;
    }

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

    public bool CheckArgument(string argument)
    {
        bool check = true;

        try
        {

            if (argument.GetType() == typeof(char))
                return false;

            if (argument[0].GetType() != typeof(char))
                return false;

            if (argument.Length == 1)
                return false;

            char letter = char.ToUpper(argument[0]);
            int IndexRow = Int32.Parse(argument.Substring(1, argument.Length - 1)) - 1;
            int IndexColumn = CharToIndex(letter);

            if (IndexRow < 0 || IndexRow >= 20)
                check = false;
            if (IndexColumn < 0 || IndexColumn >= 20)
                check = false;


            return check;
        }
        catch (Exception)            //IndexOutOfRangeException
        {
            Debug.Log("Catched");
            return false;
        }
    }

    public Vector2 ConvertPositionToCenterCell(Vector2 rawCoords)
    {
        Vector3Int cellCoordinates;
        Vector2 coordinates;

        cellCoordinates = grid.WorldToCell(rawCoords);
        coordinates = new Vector2(grid.CellToWorld(cellCoordinates).x + (grid.cellSize.x/2), grid.CellToWorld(cellCoordinates).y + (grid.cellSize.x / 2));

        return coordinates;
    }

    public Vector2 RandomEdgePosition()
    {
        Vector2 coordinates;

        int X;
        int Y;

        if (Random.Range(0,2) == 1)
        {
            if (Random.Range(0, 2) == 1)
            {
                X = 0;
            }
            else
            {
                X = 19;
            }
            coordinates = sectorsWorld[X, Random.Range(0, 20)];
        }
        else
        {
            if (Random.Range(0, 2) == 1)
            {
                Y = 0;
            }
            else
            {
                Y = 19;
            }
            coordinates = sectorsWorld[Random.Range(0, 20), Y];
        }
       

        return coordinates;
    }

    void Update()
    {
        
    }
}
