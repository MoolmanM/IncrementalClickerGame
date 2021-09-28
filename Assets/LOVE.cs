using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LOVE : MonoBehaviour
{
    void Start()
    {
        TMP_Text txtLove = gameObject.GetComponent<TMP_Text>();
        string loveString = "I LOVE YOU SO DAMN MUCH IT'S CRAZY, THIS WORLD WILL NEVER KNOW MUHAHAHAHAHAHAHAHAHAHAHAHAHA";

        StartCoroutine(EffectTypewriter(loveString, txtLove));
    }
    private IEnumerator EffectTypewriter(string text, TMP_Text uiText)
    {
        foreach (char character in text.ToCharArray())
        {
            Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            uiText.text += character;
            uiText.color = randomColor;
            yield return new WaitForSeconds(0.10f);
        }
    }
}
