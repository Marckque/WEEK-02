using TMPro;
using UnityEngine;
using System.Collections;

public class LockedTextManager : MonoBehaviour
{
    [Header("Poems")]
    public TextMeshProUGUI titleText;
    public Transform poemsRoot;
    public TextMeshProUGUI poemText;
    public TextMeshProUGUI pageText;
    private string currentPoem;
    private string displayedPoem;

    private TextMeshProUGUI[] allPoems;

    public float minimumDisplayTime;
    public float maximumDisplayTime;

    [Header("Sounds")]
    public AudioClip[] typingSounds;
    private AudioSource audioSource;

    private int selectedPoem;
    private int debugValue;

    protected void Start()
    { 
        SaveLoad.Load();

        CreateManager();
        CheckComputerID();

        audioSource = GetComponent<AudioSource>();

        UpdateTitleAndPage();
        UpdatePoem();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (debugValue < SaveLoad.savedGameManager.wordsOfPoem.Length - 1)
            {
                debugValue += 1;
            }

            StopAllCoroutines();

            UpdatePoem();
        }
    }

    private void CheckComputerID()
    {
        if (!SaveLoad.savedGameManager.uniqueComputersID.Contains(SystemInfo.deviceUniqueIdentifier))
        {
            SaveLoad.savedGameManager.uniqueComputersID.Add(SystemInfo.deviceUniqueIdentifier);

            SelectPoem();
            PoemInWords();
            SetTitleAndPage();

            if (Random.value <= 0.5f)
            {
                SaveLoad.savedGameManager.bonusValue += 1;
            }

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
        allPoems = new TextMeshProUGUI[poemsRoot.childCount];
        for (int i = 0; i < allPoems.Length; i++)
        {
            allPoems[i] = poemsRoot.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

        selectedPoem = Random.Range(0, allPoems.Length);
        SaveLoad.savedGameManager.poem = allPoems[selectedPoem].text;
    }

    private void PoemInWords()
    {
        SaveLoad.savedGameManager.wordsOfPoem = SaveLoad.savedGameManager.poem.Split(' ');
    }

    private void UpdatePoem()
    {
        currentPoem = "";

        int maxValue = Mathf.Clamp(SaveLoad.savedGameManager.uniqueComputersID.Count + SaveLoad.savedGameManager.bonusValue + debugValue, 0, SaveLoad.savedGameManager.wordsOfPoem.Length);
        for (int i = 0; i < maxValue; i++)
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
            if (i < currentCharacters.Length - 1)
            {
                PlayTypingSound();
            }

            displayedPoem += currentCharacters[i];
            displayedPoem = displayedPoem.Replace("$", "\n").Replace("*", "- Fin -"); // creates a line return
            poemText.text = displayedPoem;

            yield return new WaitForSeconds(Random.Range(minimumDisplayTime, maximumDisplayTime));
        }
    }

    private void PlayTypingSound()
    {
        int randomClip = Random.Range(0, typingSounds.Length);
        audioSource.clip = typingSounds[randomClip];

        audioSource.Play();                
    }

    private char[] StringToCharArray(string value)
    {
        char[] chars = new char[value.Length];
        chars = value.ToCharArray();

        return chars;
    }

    private void SetTitleAndPage()
    {
        SaveLoad.savedGameManager.page = selectedPoem + 1;
        SaveLoad.savedGameManager.title = allPoems[selectedPoem].name;
    }

    private void UpdateTitleAndPage()
    {
        titleText.text = SaveLoad.savedGameManager.title;
        pageText.text = "Poem n°" + SaveLoad.savedGameManager.page + " out of " + poemsRoot.childCount + ".";
    }
}