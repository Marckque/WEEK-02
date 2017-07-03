using TMPro;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LockedTextManager : MonoBehaviour
{
    [Header("Poems")]
    public string[] poems;
    private string currentPoem;
    private string displayedPoem;
    private TextMeshProUGUI poemText;
    public int debugValue;

    protected void Start()
    {
        SaveLoad.Load();

        CreateManager();
        CheckComputerID();
        UpdatePoem();

    }



    private void CheckComputerID()
    {
        if (!SaveLoad.savedGameManager.uniqueComputersID.Contains(SystemInfo.deviceUniqueIdentifier))
        {
            SaveLoad.savedGameManager.uniqueComputersID.Add(SystemInfo.deviceUniqueIdentifier);

            SelectPoem();
            PoemInWords();

            SaveLoad.Save(SaveLoad.savedGameManager);
        }
    }

    private void CreateManager()
    {
        if (SaveLoad.savedGameManager == null)
        {
            Manager newManager = new Manager();
            SaveLoad.Save(newManager);
        }
    }

    private void SelectPoem()
    {
        int selectedPoem = Random.Range(0, poems.Length);
        SaveLoad.savedGameManager.poem = poems[selectedPoem];
    }

    private void PoemInWords()
    {
        SaveLoad.savedGameManager.wordsOfPoem = SaveLoad.savedGameManager.poem.Split(' ');
    }

    private void UpdatePoem()
    {
        poemText = GetComponent<TextMeshProUGUI>();

        currentPoem = "";

        for (int i = 0; i < SaveLoad.savedGameManager.uniqueComputersID.Count + debugValue; i++)
        {
            currentPoem += SaveLoad.savedGameManager.wordsOfPoem[i] + " ";
        }

        StartCoroutine(DisplayPoem());
    }

    private IEnumerator DisplayPoem()
    {
        char[] currentCharacters = StringToCharArray(currentPoem);

        displayedPoem = "";

        for (int i = 0; i < currentCharacters.Length; i++)
        {
            displayedPoem += currentCharacters[i];
            displayedPoem = displayedPoem.Replace("$", "\n"); // creates a line return

            poemText.text = displayedPoem;

            yield return new WaitForSeconds(Random.Range(0f, 0f));
        }
    }

    private char[] StringToCharArray(string value)
    {
        char[] chars = new char[value.Length];
        chars = value.ToCharArray();

        return chars;
    }
}
