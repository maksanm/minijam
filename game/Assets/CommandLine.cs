using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommandLine : MonoBehaviour
{
    static readonly Regex trimmer = new Regex(@"\s\s+");


    private InputField input;

    private SectorsArray sectors;

    private List<string> BuiltInCommands = new List<string>{"help", "move", "swap", "split", "status"};

    public Sprite Ok;
    public Sprite NieOk;
    public Sprite Question;

    private GameObject childHeader;
    private GameObject childImage;
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
        childImage = input.transform.GetChild(4).gameObject;
        childLogs = input.transform.GetChild(5).gameObject;

        childImage.GetComponent<Image>().sprite = null;
        childImage.GetComponent<Image>().enabled = true;
    }

    private void Update()
    {

    }


    private void Help()
    {
        childHeader.GetComponent<Text>().text = "Help";
        childImage.GetComponent<Image>().sprite = Question;

        childLogs.GetComponent<Text>().text = "move, swap, split, status";

    }
    private void Move()
    {
        bool ok = false;

        SquadController[] allSquads = FindObjectsOfType<SquadController>();
        Vector2 moveDestination = new Vector2(0, 0);

        if (sectors.CheckArgument(destination))
        {
            moveDestination = sectors.ConvertArgumentToCoords(destination);
            for (int i = 1; i < CommandData.Length - 1; i++)
            {
                Vector2 destinationCoordinates = new Vector2(0, 0);
                if (sectors.CheckArgument(CommandData[i]))
                {
                    destinationCoordinates = sectors.ConvertArgumentToCoords(CommandData[i]);
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

               
            }
        }
        
        if (ok)
        {
            childHeader.GetComponent<Text>().text = "Success";
            childImage.GetComponent<Image>().sprite = Ok;
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childImage.GetComponent<Image>().sprite = NieOk;
        }
    }
    private void Swap()
    {
        bool ok = false;

        SquadController[] allSquads = FindObjectsOfType<SquadController>();

        Vector2 move1Ship = new Vector2(0, 0);
        Vector2 move2Ship = new Vector2(0, 0);

        Debug.Log("Swap");

        if (sectors.CheckArgument(CommandData[1]) && sectors.CheckArgument(CommandData[2]))
        {
            move1Ship = sectors.ConvertArgumentToCoords(CommandData[1]);
            move2Ship = sectors.ConvertArgumentToCoords(CommandData[2]);

            for (int j = 0; j < allSquads.Length; j++)
            {

                if (allSquads[j].tag != "Allies")
                    continue;

                Vector2 shipCenterCoordinates = sectors.ConvertPositionToCenterCell(allSquads[j].gameObject.transform.position);

                if (move1Ship == shipCenterCoordinates)
                {
                    Debug.Log("swap1");
                    ok = true;
                    allSquads[j].GetCommand(move2Ship, Command);
                }
                else if (move2Ship == shipCenterCoordinates)
                {
                    Debug.Log("swap2");
                    ok = true;
                    allSquads[j].GetCommand(move1Ship, Command);
                }
            }
        }
        if (ok)
        {
            childHeader.GetComponent<Text>().text = "Success";
            childImage.GetComponent<Image>().sprite = Ok;
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childImage.GetComponent<Image>().sprite = NieOk;
        }

    }
    private void Split()
    {
        bool ok = false;

        SquadController[] allSquads = FindObjectsOfType<SquadController>();
        SquadController[] splitedSquads;

        string splitSpec = CommandData[1];

        if (sectors.CheckArgument(CommandData[2]) && sectors.CheckArgument(CommandData[3]) && splitSpec[0] == '-')
        {
            Vector2 origin = sectors.ConvertArgumentToCoords(CommandData[2]);
            Vector2 destination = sectors.ConvertArgumentToCoords(CommandData[3]);



            // -s1ml -> 1 small 1 medium 1 large === -sml === -sm1l === -sml1

            int smallCount = 0;
            int mediumCount = 0;
            int largeCount = 0;

            for (int index = 1; index < splitSpec.Length; index++)
            {
                char letter = splitSpec[index];

                switch (letter)
                {
                    case 's':
                        if (index + 1 < splitSpec.Length)
                        {
                            char letterArg = splitSpec[index + 1];
                            if (Char.IsDigit(letterArg))
                                smallCount = (int)Char.GetNumericValue(letterArg);
                            else
                                smallCount = 1;
                        }
                        else
                        {
                            smallCount = 1;
                        }
                        break;
                    case 'm':
                        if (index + 1 < splitSpec.Length)
                        {
                            char letterArg = splitSpec[index + 1];
                            if (Char.IsDigit(letterArg))
                                mediumCount = (int)Char.GetNumericValue(letterArg);
                            else
                                mediumCount = 1;
                        }
                        else
                        {
                            mediumCount = 1;
                        }
                        break;
                    case 'l':
                        if (index + 1 < splitSpec.Length)
                        {
                            char letterArg = splitSpec[index + 1];
                            if (Char.IsDigit(letterArg))
                                largeCount = (int)Char.GetNumericValue(letterArg);
                            else
                                largeCount = 1;
                        }
                        else
                        {
                            largeCount = 1;
                        }
                        break;
                }
            }

            Debug.Log("Split Counts->" + " s: " + smallCount + " m: " + mediumCount + " l: " + largeCount);

            for (int j = 0; j < allSquads.Length; j++)
            {

                if (allSquads[j].tag != "Allies")
                   continue;

                Vector2 shipCenterCoordinates = sectors.ConvertPositionToCenterCell(allSquads[j].gameObject.transform.position);

                Debug.Log(shipCenterCoordinates + " " + origin);

                if (shipCenterCoordinates == origin)
                {
                    if (allSquads[j].divisionClass == "small" && smallCount > 0)
                    {
                        allSquads[j].GetCommand(destination, "split");
                        smallCount -= 1;
                        ok = true;
                    }
                    else if (allSquads[j].divisionClass == "medium" && mediumCount > 0)
                    {
                        allSquads[j].GetCommand(destination, "split");
                        mediumCount -= 1;
                        ok = true;
                    }
                    else if (allSquads[j].divisionClass == "large" && largeCount > 0)
                    {
                        allSquads[j].GetCommand(destination, "split");
                        largeCount -= 1;
                        ok = true;
                    }

                }
            }

               
            
        }

        if (ok)
        {
            childHeader.GetComponent<Text>().text = "Success";
            childImage.GetComponent<Image>().sprite = Ok;
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childImage.GetComponent<Image>().sprite = NieOk;
        }

    }






    private void Status()
    {
        if (CommandData.Length == 1 || (CommandData.Length == 2 && CommandData[CommandData.Length-1] == " "))
        {
            StationContoller station = FindObjectOfType<StationContoller>();


            childHeader.GetComponent<Text>().text = "Status";
            childImage.GetComponent<Image>().sprite = Question;
            childLogs.GetComponent<Text>().text = "Artifact station HP: " + station.currentHealth.ToString();
        }
        else if (CommandData.Length == 2)
        {
            bool ok = false;

            SquadController[] allSquads = FindObjectsOfType<SquadController>();
            int sqIndex = 0;
            Vector2 destinationCoordinates = new Vector2(0, 0);
            if (sectors.CheckArgument(CommandData[1]))
            {
                destinationCoordinates = sectors.ConvertArgumentToCoords(CommandData[1]);
                for (int j = 0; j < allSquads.Length; j++)
                {
                    if (allSquads[j].tag != "Allies")
                        continue;
                    Vector2 shipCenterCoordinates = sectors.ConvertPositionToCenterCell(allSquads[j].gameObject.transform.position);
                    if ((destinationCoordinates.x == shipCenterCoordinates.x) && (destinationCoordinates.y == shipCenterCoordinates.y))
                    {
                        sqIndex = j;
                        ok = true;
                        break;
                    }
                }
            }
            if (ok)
            {
                childHeader.GetComponent<Text>().text = "Status";
                childImage.GetComponent<Image>().sprite = Question;
                childLogs.GetComponent<Text>().text = "Hp: " + (allSquads[sqIndex].transform.childCount * allSquads[sqIndex].transform.GetComponentInChildren<ShipController>().maxHealth).ToString()
                    + "," + "Damage: " +(allSquads[sqIndex].transform.childCount * allSquads[sqIndex].damage).ToString()+ "," + "Speed: " + 
                    (allSquads[sqIndex].speed).ToString();
            }
            else
            {
                childHeader.GetComponent<Text>().text = "Failed";
                childImage.GetComponent<Image>().sprite = NieOk;
                input.text = "";
            }
        }
    }

    private void SubmitName(string arg0)
    {

        arg0.Trim();
        arg0 = trimmer.Replace(arg0, " ");

        CommandData = arg0.Split(' ');
        Command = CommandData[0].ToLower();

        if (Command != "status")
            destination = CommandData[CommandData.Length-1];

        if (BuiltInCommands.Contains(Command) && sectors)
        {

            if (Command == "help" && CommandData.Length == 1)
                Help();
            else if (Command == "move" && CommandData.Length >= 3)
                Move();
            else if (Command == "swap" && CommandData.Length == 3)
                Swap();
            else if (Command == "status" && CommandData.Length >= 1)
                Status();
            else if (Command == "split" && CommandData.Length == 4)
                Split();
            else
            {
                childHeader.GetComponent<Text>().text = "Failed";
                childImage.GetComponent<Image>().sprite = NieOk;
            }
        }
        else
        {
            childHeader.GetComponent<Text>().text = "Failed";
            childImage.GetComponent<Image>().sprite = NieOk;
        }

        if (Command != "help" && Command != "status")
            childLogs.GetComponent<Text>().text = arg0;

        input.ActivateInputField();
        input.text = "";
    }
}
