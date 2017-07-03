using TMPro;
using UnityEngine;
using System.Collections;

public class LockedTextManager : MonoBehaviour
{
    [Header("Poems")]
    public string[] titles;
    public TextMeshProUGUI titleText;
    public string[] poems;
    public TextMeshProUGUI poemText;
    public TextMeshProUGUI pageText;
    private string currentPoem;
    private string displayedPoem;

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
        selectedPoem = Random.Range(0, poems.Length);
        SaveLoad.savedGameManager.poem = poems[selectedPoem];
    }

    private void PoemInWords()
    {
        SaveLoad.savedGameManager.wordsOfPoem = SaveLoad.savedGameManager.poem.Split(' ');
    }

    private void UpdatePoem()
    {
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
            if (i < currentCharacters.Length - 1)
            {
                PlayTypingSound();
            }

            displayedPoem += currentCharacters[i];
            displayedPoem = displayedPoem.Replace("$", "\n").Replace("*", "- The end -"); // creates a line return
            poemText.text = displayedPoem;

            yield return new WaitForSeconds(Random.Range(minimumDisplayTime, maximumDisplayTime));
        }
    }

    private void PlayTypingSound()
    {
        int randomClip = Random.Range(0, typingSounds.Length);
        audioSource.clip = typingSounds[randomClip];

        //if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
                
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
        SaveLoad.savedGameManager.title = titles[selectedPoem];
    }

    private void UpdateTitleAndPage()
    {
        titleText.text = SaveLoad.savedGameManager.title;
        pageText.text = SaveLoad.savedGameManager.page + "/" + poems.Length;
    }
}