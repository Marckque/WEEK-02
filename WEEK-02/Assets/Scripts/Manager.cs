using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Manager
{
    public static Manager Instance;
    public List<string> uniqueComputersID = new List<string>();

    public string poem;
    public string title;
    public string[] wordsOfPoem;
    public int page;
    public int bonusValue;

    public Manager()
    {
        Instance = this;
    }
}