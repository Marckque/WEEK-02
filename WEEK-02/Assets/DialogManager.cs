using TMPro;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogManager : MonoBehaviour
{
    private TextMeshProUGUI mainText;

    [Header("Text display")]
    public float minimumDelayCharacterDisplay = 0.1f;
    public float maximumDelayCharacterDisplay = 0.3f;

    [Header("Dialogs")]
    public string[] allDialogs;
    private int dialogIndex;
    private string currentDialog;
    private char[] currentCharacters;

    private float nextDialogDelay;

    private string computerName;
    private bool goToNextDialog;
    private bool waitForInput;

	protected void Start()
    {
        mainText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DisplayText());
	}

    protected void Update()
    {
        if (waitForInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                dialogIndex++;

                waitForInput = false;
                StartCoroutine(DisplayText());
            }
        }
    }

    private char[] StringToCharArray(string value)
    {
        char[] chars = new char[value.Length];
        chars = value.ToCharArray();

        return chars;
    }

    private IEnumerator DisplayText()
    {
        currentDialog = "";

        goToNextDialog = false;

        SpecialDialogEvent();

        currentCharacters = StringToCharArray(allDialogs[dialogIndex]);

        for (int i = 0; i < currentCharacters.Length; i++)
        {
            currentDialog += currentCharacters[i];
            mainText.text = currentDialog;

            yield return new WaitForSeconds(Random.Range(minimumDelayCharacterDisplay, maximumDelayCharacterDisplay));
        }

        if (goToNextDialog)
        {
            yield return new WaitForSeconds(nextDialogDelay);

            dialogIndex++;
            StartCoroutine(DisplayText());
        }
        else
        {
            waitForInput = true;
        }
    }

    private void SpecialDialogEvent()
    {
        switch (dialogIndex)
        {
            case 0:
                AddTextToEnd(true, SystemInfo.deviceModel);
                NextDialogConfiguration(2f);
                break;

            case 1:
                AddTextToEnd(true, SystemInfo.operatingSystem);
                NextDialogConfiguration(2f);
                break;

            case 2:
                NextDialogConfiguration(1f);
                break;

            case 3:
                AddTextToEnd(true, SystemInfo.deviceName + "\n\n I am pleased to meet you! :)");
                NextDialogConfiguration(2f);
                break;

            default:
                break;
        }
    }

    private void AddTextToEnd(bool goToLine, string value)
    {
        if (goToLine)
        {
            allDialogs[dialogIndex] += "\n";
        }
        allDialogs[dialogIndex] += value;
    }

    private void NextDialogConfiguration(float value)
    {
        goToNextDialog = true;
        nextDialogDelay = value;
    }
}