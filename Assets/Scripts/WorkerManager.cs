using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkerManager : MonoBehaviour
{
    public bool xOne, xTen, xHundred, xMax;
    public int increaseAmount, woodcutterWorkerAmount, minerWorkerAmount, farmerWorkerAmount;
    public GameObject woodcutterText, minerText, farmerText;

    public void OnXoneClick()
    {
        increaseAmount = 1;
    }

    public void OnXtenClick()
    {
        increaseAmount = 10;
    }

    public void OnXHundredClick()
    {
        increaseAmount = 100;
    }

    public void OnXmaxClick()
    {
        increaseAmount = GameManager.Instance.availableWorkers;
    }

    public void OnWoodcutterPlusClick()
    {
        
        if (GameManager.Instance.availableWorkers >= increaseAmount)
        {
            GameManager.Instance.availableWorkers -= increaseAmount;
            woodcutterWorkerAmount = increaseAmount;
        }
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        woodcutterText.GetComponent<TextMeshProUGUI>().text = string.Format("Woodcutters: [{0}]", woodcutterWorkerAmount);
    }
}
