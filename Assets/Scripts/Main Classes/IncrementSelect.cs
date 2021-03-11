using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementSelect : MonoBehaviour
{
    public static bool OneSelected, TenSelected, HundredSelected, MaxSelected;
    public GameObject imageOne, imageTen, imageHundred, imageMax;
    public void On1()
    {
        imageOne.SetActive(true);
        imageTen.SetActive(false);
        imageHundred.SetActive(false);
        imageMax.SetActive(false);

        OneSelected = true;
        TenSelected = false;
        HundredSelected = false;
        MaxSelected = false;
    }

    public void On10()
    {
        imageOne.SetActive(false);
        imageTen.SetActive(true);
        imageHundred.SetActive(false);
        imageMax.SetActive(false);

        OneSelected = false;
        TenSelected = true;
        HundredSelected = false;
        MaxSelected = false;

    }

    public void On100()
    {
        imageOne.SetActive(false);
        imageTen.SetActive(false);
        imageHundred.SetActive(true);
        imageMax.SetActive(false);

        OneSelected = false;
        TenSelected = false;
        HundredSelected = true;
        MaxSelected = false;
    }

    public void OnMax()
    {
        imageOne.SetActive(false);
        imageTen.SetActive(false);
        imageHundred.SetActive(false);
        imageMax.SetActive(true);

        OneSelected = false;
        TenSelected = false;
        HundredSelected = false;
        MaxSelected = true;
    }
}
