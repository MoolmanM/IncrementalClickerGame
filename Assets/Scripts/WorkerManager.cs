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
    public Toggle toggle1, toggle10, toggle100, toggleMax;

    public void Start()
    {
        increaseAmount = 1;
    }
    /*public void XOneToggle()
    {
        increaseAmount = 1;
        toggleAnim.SetTrigger("One");
    }

    public void XTenToggle()
    {
        increaseAmount = 10;
        toggleAnim.SetTrigger("Ten");
    }

    public void XHundredToggle()
    {
        increaseAmount = 100;
        toggleAnim.SetTrigger("Hundred");
    }

    public void XMaxToggle()
    {
        increaseAmount = GameManager.Instance.availableWorkers;
        toggleAnim.SetTrigger("Max");
        xMax = true;

    }*/

    public void ToggleManager()
    {
        while (toggle1.isOn)
        {
            increaseAmount = 1;
            toggleAnim.SetTrigger("One");
        }
        while (toggle10.isOn)
        {
            increaseAmount = 10;
            toggleAnim.SetTrigger("One");
            toggleAnim.SetTrigger("Ten");
        }
        while (toggle100.isOn)
        {
            increaseAmount = 100;
            toggleAnim.SetTrigger("One");
            toggleAnim.SetTrigger("Ten");
            toggleAnim.SetTrigger("Hundred");
        }
        while (toggleMax.isOn)
        {
            increaseAmount = GameManager.Instance.availableWorkers;
            toggleAnim.SetTrigger("One");
            toggleAnim.SetTrigger("Ten");
            toggleAnim.SetTrigger("Hundred");
            toggleAnim.SetTrigger("Max");
        }
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
