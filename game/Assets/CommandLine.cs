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

    private List<string> BuiltInCommands = new List<string>{"help", "move", "defend", "status"};

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

        childLogs.GetComponent<Text>().text = "move, status";

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
        for (int i = 1; i < CommandData.Length - 1; i++)
        {
            childHeader.GetComponent<Text>().text = "Status:";

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
            else if (Command == "swap" && CommandData.Length == 2)
                Swap();
            else if (Command == "status" && CommandData.Length >= 1)
                Status();
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
