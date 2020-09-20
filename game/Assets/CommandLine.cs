using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLine : MonoBehaviour
{
    private InputField input;

    private SectorsArray sectors;

    private List<string> BuiltInCommands = new List<string>{"help", "move", "defend", "hide"};
    private Stack<string> History = new Stack<string>();

    private Stack<string> Buffor = new Stack<string>();

    private string[] CommandData;
    private string Command;

    private string destination;

    void Start()
    {

        input = GetComponentInChildren<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;

        input.ActivateInputField();
        Debug.Log(input.runInEditMode);

        sectors = FindObjectOfType<SectorsArray>();
    }

    private void Update()
    {
        // UNDO
        //if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && History.Count!=0)
        //{
        //    Debug.Log("Undo");
        //    Buffor.Push(History.Pop());
        //}
        //else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && Buffor.Count!=0)
        //{
        //    Debug.Log("ReUndo");
        //    History.Push(Buffor.Pop());
        //}
        //else
        //{
        //    Buffor.Clear();
        //}
    }


    private void Help()
    {
        Debug.Log("Help");
    }
    private void Move()
    {
        SquadController[] allSquads = FindObjectsOfType<SquadController>();
        Vector2 moveDestination = sectors.ConvertArgumentToCoords(destination);

        for (int i = 1; i < CommandData.Length - 1; i++)
        {
            Vector2 destinationCoordinates = sectors.ConvertArgumentToCoords(CommandData[i]);

            for (int j = 0; j < allSquads.Length; j++)
            {
                if (allSquads[j].tag != "Allies")
                    continue;
                Vector2 shipCenterCoordinates = sectors.ConvertPositionToCenterCell(allSquads[j].gameObject.transform.position);

                if ((destinationCoordinates.x == shipCenterCoordinates.x) && (destinationCoordinates.y == shipCenterCoordinates.y))
                {
                    allSquads[j].GetCommand(shipCenterCoordinates, moveDestination, Command);
                }
            }
        }
    }
    private void Defend()
    {
        for (int i = 1; i < CommandData.Length - 1; i++)
        {

        }
    }
    private void Hide()
    {
        for (int i = 1; i < CommandData.Length - 1; i++)
        {

        }
    }

    private void SubmitName(string arg0)
    {
        CommandData = input.text.Split(' ');

        Command = CommandData[0];
        destination = CommandData[CommandData.Length-1];

        History.Push(input.text);
        Debug.Log(input.text);

        if (BuiltInCommands.Contains(Command) && sectors)
        {

            if (Command == "help" && CommandData.Length == 1)
                Help();
            else if (Command == "move" && CommandData.Length >= 3)
                Move();
            else if (Command == "defend" && CommandData.Length >= 1)
                Defend();
            else if (Command == "hide" && CommandData.Length >= 1)
                Hide();
            else
            {
                //Debug.Log("Error");
            }
        }
        else
        {
            //Debug.Log("Error");
        }


        input.text = "";
    }
}
