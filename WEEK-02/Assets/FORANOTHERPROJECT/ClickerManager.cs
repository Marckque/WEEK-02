using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickerManager : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI latestActionText;

    [Header("Resources")]
    private int resource;
    private int resourceIncrement = 1;

    protected void Start()
    {
        resourceText = GetComponent<TextMeshProUGUI>();
        UpdateResourceUI();
    } 

    protected void Update()
    {
        if (Input.anyKeyDown)
        {
            RandomIncrementation();
        }   
    }

    private void RandomIncrementation()
    {
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                resource += 1;
                break;
            case 1:
                resource *= 3;
                break;
            case 2:
                resource /= 9;
                break;
            case 3:
                resource %= 8;
                break;
            case 4:
                resource *= resource;
                break;
            case 5:
                resource += 3774514;
                break;
            case 6:
                resource -= 288846;
                break;
            case 7:
                resource *= 96;
                break;
            case 8:
                resource *= (int) Mathf.Sin(resource);
                break;
            case 9:
                resource += 9;
                break;
            default:
                break; 
        }

        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        resourceText.text = resource.ToString();
    }

    private void UpdateLatestActionUI()
    {

    }
}