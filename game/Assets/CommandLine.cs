using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLine : MonoBehaviour
{
    private InputField input;

    private SectorsArray sectors;

    private List<string> BuiltInCommands = new List<string>{"help", "move", "defend", "status"};

    private GameObject childHeader;
    private GameObject childError;
    private GameObject childOk;
    private GameObject childLogs;


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

        sectors = FindObjectOfType<SectorsArray>();

        childHeader = input.transform.GetChild(3).gameObject;
        childError = input.transform.GetChild(4).gameObject;
        childOk = input.transform.GetChild(5).gameObject;
        childLogs = input.transform.GetChild(6).gameObject;

        childError.GetComponent<Image>().enabled = false;
        childOk.GetComponent<Image>().enabled = false;
    }

    private void Update()
    {

    }


    private void Help()
    {
        Debug.Log("Help");
    }
    private void Move()
    {
        bool ok = false;

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
                    ok = true;
                    allSquads[j].GetCommand(moveDestination, Command);
                }
            }
        }
        if (ok)
        {
            childHeader.GetComponent<Text>().text = "Success";
            childError.GetComponent<Image>().enabled = false;
            childOk.GetComponent<Image>().enabled = true;
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childError.GetComponent<Image>().enabled = true;
            childOk.GetComponent<Image>().enabled = false;
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
        Debug.Log(sectors.RandomEdgePosition());

        CommandData = input.text.Split(' ');

        Command = CommandData[0];
        if (Command != "status")
            destination = CommandData[CommandData.Length-1];

        Debug.Log(input.text);

        if (BuiltInCommands.Contains(Command) && sectors)
        {

            if (Command == "help" && CommandData.Length == 1)
                Help();
            else if (Command == "move" && CommandData.Length >= 3)
                Move();
            else if (Command == "defend" && CommandData.Length >= 1)
                Defend();
            else if (Command == "status" && CommandData.Length >= 0)
                Hide();
            else
            {
                childHeader.GetComponent<Text>().text = "Failed";
                childError.GetComponent<Image>().enabled = true;
                childOk.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childError.GetComponent<Image>().enabled = true;
            childOk.GetComponent<Image>().enabled = false;
        }

        childLogs.GetComponent<Text>().text = arg0;

        input.ActivateInputField();
        input.text = "";
    }
}
