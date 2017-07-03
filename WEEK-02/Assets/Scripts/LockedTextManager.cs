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

    [Header("Delay")]
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

#if UNITY_EDITOR
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

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (debugValue < SaveLoad.savedGameManager.wordsOfPoem.Length - 1)
            {
                minimumDisplayTime = 0f;
                maximumDisplayTime = 0.01f;
                debugValue += 100;
            }

            StopAllCoroutines();

            UpdatePoem();
        }
    }
#endif

    private void CheckComputerID()
    {
        if (!SaveLoad.savedGameManager.uniqueComputersID.Contains(SystemInfo.deviceUniqueIdentifier))
        {
            SaveLoad.savedGameManager.uniqueComputersID.Add(SystemInfo.deviceUniqueIdentifier);

            SelectPoem();
            PoemInWords();
            SetTitleAndPage();
            UpdateBonusChances();

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

    private void UpdateBonusChances()
    {
        if (SaveLoad.savedGameManager.uniqueComputersID.Count > 1)
        {
            float comparer = Mathf.Clamp(0.5f + SaveLoad.savedGameManager.bonusValueComparer, 0.5f, 1f);
            if (Random.value <= comparer)
            {
                SaveLoad.savedGameManager.bonusValue += 1;
                SaveLoad.savedGameManager.bonusValueComparer += 0.1f;
            }
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
            displayedPoem = displayedPoem.Replace("$", "\n"); // creates a line return
            displayedPoem = displayedPoem.Replace("*", "- Fin -");
            displayedPoem = displayedPoem.Replace("+", "- End -");
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