using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLine : MonoBehaviour
{
    private InputField input;

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
        for (int i = 1; i < CommandData.Length - 1; i++)
        {

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

        //Debug.Log(Command + " " + destination);

        History.Push(input.text);
        //Debug.Log(History);

        if (BuiltInCommands.Contains(Command))
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
        Debug.Log(input.runInEditMode);

        //Debug.Log(arg0);

        //input.ActivateInputField();
    }
}
