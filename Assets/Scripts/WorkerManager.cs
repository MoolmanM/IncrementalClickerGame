using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public bool xOne, xTen, xHundred, xMax;
    public int increaseAmount, increaseAmountCache, workerAmountToMinus;
    public GameObject woodcutterText, minerText, farmerText;

    public Animator toggleAnim;
    public static float woodcutterWorkMutliplier, minerWorkMultiplier, farmerWorkMultiplier;
    public static int woodcutterWorkerAmount, minerWorkerAmount, farmerWorkerAmount, availableWorkers, maxWorkers;

    public void Start()
    {
        increaseAmount = 1;
        woodcutterWorkMutliplier = (float)0.15;
        minerWorkMultiplier = (float)0.15;
        farmerWorkMultiplier = (float)0.15;

    }
    public void Selected1()
    {
        xOne = true;
        xTen = false;
        xHundred = false;
        xMax = false;

        increaseAmount = 1;
        toggleAnim.SetTrigger("1Idle");
    }
    public void Selected10()
    {
        toggleAnim.SetTrigger("2Anim");
        xOne = false;
        xTen = true;
        xHundred = false;
        xMax = false;

        increaseAmount = 10;

    }
    public void Selected100()
    {
        if (xOne == true)
        {
            toggleAnim.SetTrigger("2to3Anim");
        }
        else if (xTen == true)
        {
            toggleAnim.SetTrigger("2Idle3Anim");
        }
        else
        {
            toggleAnim.SetTrigger("2Idle3Anim");
        }

        xOne = false;
        xTen = false;
        xHundred = true;
        xMax = false;

        increaseAmount = 100;

    }
    public void SelctedMax()
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

        increaseAmount = availableWorkers;
    }
    private void WorkerButtonPlus(ref int thisWorkerAmount, GameObject thisWorkerObject, string thisWorkerName)
    {
        increaseAmountCache = increaseAmount;

        if ((xMax) || (increaseAmountCache > availableWorkers))
        {
            increaseAmountCache = availableWorkers;
            availableWorkers -= increaseAmountCache;
            thisWorkerAmount += increaseAmountCache;
        }

        else if (increaseAmount <= availableWorkers)
        {
            availableWorkers -= increaseAmountCache;
            thisWorkerAmount += increaseAmountCache;
        }
        else
        {
            Debug.Log("Catch all");
        }

        //availableWorkerText.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", availableWorkers, maxWorkers);
        thisWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: [{1}]", thisWorkerName, thisWorkerAmount);
    }
    private void WorkerButtonMinus(ref int thisWorkerAmount, GameObject thisWorkerObject, string thisWorkerName)
    {
        increaseAmountCache = increaseAmount;

        if ((increaseAmountCache > thisWorkerAmount) || (xMax))
        {
            workerAmountToMinus = thisWorkerAmount;
            thisWorkerAmount -= workerAmountToMinus;
            availableWorkers += workerAmountToMinus;
        }

        else if (increaseAmount <= thisWorkerAmount)
        {
            thisWorkerAmount -= increaseAmountCache;
            availableWorkers += increaseAmountCache;
        }

        else
        {
            Debug.Log("Catch all");
        }

        //availableWorkerText.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", availableWorkers, maxWorkers);
        thisWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: [{1}]", thisWorkerName, thisWorkerAmount);
    }
    public void ButtonWoodcutterPlus()
    {
        WorkerButtonPlus(ref woodcutterWorkerAmount, woodcutterText, "Woodcutters");
    }
    public void ButtonWoodcutterMinus()
    {
        WorkerButtonMinus(ref woodcutterWorkerAmount, woodcutterText, "Woodcutters");
    }
    public void ButtonMinerPlus()
    {
        WorkerButtonPlus(ref minerWorkerAmount, minerText, "Miners");
    }
    public void ButtonMinerMinus()
    {
        WorkerButtonMinus(ref minerWorkerAmount, minerText, "Miners");
    }
    public void ButtonFarmerPlus()
    {
        WorkerButtonPlus(ref farmerWorkerAmount, farmerText, "Farmers");
    }
    public void ButtonFarmerMinus()
    {
        WorkerButtonMinus(ref farmerWorkerAmount, farmerText, "Farmers");
    }
}


