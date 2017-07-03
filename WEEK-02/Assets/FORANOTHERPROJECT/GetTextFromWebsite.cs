using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTextFromWebsite : MonoBehaviour
{
    TMPro.TextMeshProUGUI poetryText;

    protected void Start()
    {
        poetryText = GetComponent<TMPro.TextMeshProUGUI>();
        
        
        //poetryText.text = w.ToString();        
    }

    private IEnumerator WWWRequest(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

        }
    }
}