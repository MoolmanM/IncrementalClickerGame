using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public bool xOne, xTen, xHundred, xMax;
    public int increaseAmount, increaseAmountCache, woodcutterWorkerAmount, minerWorkerAmount, farmerWorkerAmount, workerAmountToMinus;
    public GameObject woodcutterText, minerText, farmerText;
    public Animator toggleAnim;

    public void Start()
    {
        increaseAmount = 1;
    }

    public void Button1()
    {
        xOne = true;
        xTen = false;
        xHundred = false;
        xMax = false;

        increaseAmount = 1;
        toggleAnim.SetTrigger("1Idle");
    }

    public void Button10()
    {
        toggleAnim.SetTrigger("2Anim");
        xOne = false;
        xTen = true;
        xHundred = false;
        xMax = false;

        increaseAmount = 10;
        
    }

    public void Button100()
    {
        if (xOne == true)
        {
            toggleAnim.SetTrigger("2to3Anim");
        }
        else if (xTen == true)
        {
            toggleAnim.SetTrigger("2Idle3Anim");
        }

        xOne = false;
        xTen = false;
        xHundred = true;
        xMax = false;

        increaseAmount = 100;
        
    }

    public void ButtonMax()
    {
        if (xOne == true)
        {
            toggleAnim.SetTrigger("2to4Anim");
        }
        else if (xTen == true)
        {
            toggleAnim.SetTrigger("2Idle3to4Anim");
        }
        else if (xHundred == true)
        {
            toggleAnim.SetTrigger("23Idle4Anim");
        }

        xOne = false;
        xTen = false;
        xHundred = false;
        xMax = true;

        increaseAmount = GameManager.Instance.availableWorkers;
    }

    public void OnWoodcutterPlusClick()
    {
        increaseAmountCache = increaseAmount;

        if (GameManager.Instance.availableWorkers >= increaseAmountCache)
        {
            GameManager.Instance.availableWorkers -= increaseAmountCache;
            woodcutterWorkerAmount += increaseAmountCache;
        }
        else if (GameManager.Instance.availableWorkers <= increaseAmountCache)
        {
            increaseAmountCache = GameManager.Instance.availableWorkers;
            GameManager.Instance.availableWorkers -= increaseAmountCache;
            woodcutterWorkerAmount += increaseAmountCache;
        }
        else
        {
            Debug.LogError("Catch all");
        }
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        woodcutterText.GetComponent<TextMeshProUGUI>().text = string.Format("Woodcutters: [{0}]", woodcutterWorkerAmount);
    }

    public void OnWoodcutterMinusClick()
    {
        increaseAmountCache = increaseAmount;

        if (increaseAmountCache >= woodcutterWorkerAmount)
        {
            workerAmountToMinus = woodcutterWorkerAmount;
            woodcutterWorkerAmount -= workerAmountToMinus;
            GameManager.Instance.availableWorkers += workerAmountToMinus;
        }
        else if (xMax == true)
        {
            increaseAmountCache = woodcutterWorkerAmount;
            woodcutterWorkerAmount -= increaseAmountCache;
            GameManager.Instance.availableWorkers += increaseAmountCache;
        }
        else
        {
            woodcutterWorkerAmount -= increaseAmountCache;
            GameManager.Instance.availableWorkers += increaseAmountCache;
        }
        
        GameManager.Instance.availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", GameManager.Instance.availableWorkers, GameManager.Instance.maxWorkers);
        woodcutterText.GetComponent<TextMeshProUGUI>().text = string.Format("Woodcutters: [{0}]", woodcutterWorkerAmount);
    }
}
