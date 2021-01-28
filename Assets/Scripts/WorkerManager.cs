using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public bool xOne, xTen, xHundred, xMax;
    public int increaseAmount, woodcutterWorkerAmount, minerWorkerAmount, farmerWorkerAmount;
    public GameObject woodcutterText, minerText, farmerText;
    public Animator toggleAnim;

    public void Start()
    {
        increaseAmount = 1;
    }

    public void Button1()
    {
        increaseAmount = 1;
        toggleAnim.SetTrigger("One");
    }

    public void Button10()
    {
        increaseAmount = 10;
        toggleAnim.SetTrigger("Ten");
    }

    public void Button100()
    {
        increaseAmount = 100;
        toggleAnim.SetTrigger("Hundred");
    }

    public void ButtonMax()
    {
        increaseAmount = GameManager.Instance.availableWorkers;
        toggleAnim.SetTrigger("Max");
        xMax = true;

    }

    public void OnWoodcutterPlusClick()
    {
        
        if (GameManager.Instance.availableWorkers >= increaseAmount)
        {
            GameManager.Instance.availableWorkers -= increaseAmount;
            woodcutterWorkerAmount += increaseAmount;
        }
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        woodcutterText.GetComponent<TextMeshProUGUI>().text = string.Format("Woodcutters: [{0}]", woodcutterWorkerAmount);
    }

    public void OnWoodcutterMinusClick()
    {
        if (woodcutterWorkerAmount >= increaseAmount)
        {
            woodcutterWorkerAmount -= increaseAmount;
            GameManager.Instance.availableWorkers += increaseAmount;
        }
        else if (woodcutterWorkerAmount <= increaseAmount)
        {
            increaseAmount = woodcutterWorkerAmount;
            GameManager.Instance.availableWorkers += increaseAmount;
        }
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        woodcutterText.GetComponent<TextMeshProUGUI>().text = string.Format("Woodcutters: [{0}]", woodcutterWorkerAmount);
    }
}
