using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{

    private int firstOnce = 0;
    private int firstTwice = 0;
    private int secondOnce = 0;
    private int secondTwice = 0;

    private Text timer;
    public bool mainScene = true;
    public static string time;
    bool firstOne = true;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Text>();
    }

    private IEnumerator TimerCountDown()
    {
        isPaused = true;
        yield return new WaitForSecondsRealtime(1f);

        firstOnce += 1;
        if (firstOnce > 9)
        {
            firstOnce = 0;
            firstTwice += 1;
        }
        if (firstTwice >= 6)
        {
            firstTwice = 0;
            secondOnce += 1;
        }
        if (secondOnce > 9)
        {
            secondOnce = 0;
            secondTwice += 1;
        }
        if (secondTwice > 9)
        {
            Debug.Log("LOL");
        }

        timer.text = buildString();
        isPaused = false;
    }

    private string buildString()
    {
        string text;

        string[] chars = { "j", "j", ":", "j", "j" };

        chars[chars.Length-1] = firstOnce.ToString();

        chars[chars.Length-2] = firstTwice.ToString();
        chars[chars.Length-4] = secondOnce.ToString();
        chars[chars.Length-5] = secondTwice.ToString();

        text = string.Join("",chars);

        return text;
    }

    void Update()
    {
        if (!isPaused && mainScene)
        {
            StartCoroutine(TimerCountDown());
            time = timer.text;
        }
        else if(firstOne)
        {
            timer.text = time;
            firstOne = !firstOne;
        }
    }
}