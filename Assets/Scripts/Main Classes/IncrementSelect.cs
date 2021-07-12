using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementSelect : MonoBehaviour
{
    public static bool IsOneSelected, IsTenSelected, IsHundredSelected, IsMaxSelected;
    public GameObject ImgOne, ImgTen, ImgHundred, ImgMax;
    void Start()
    {
        ImgOne.SetActive(true);
        IsOneSelected = true;
    }
    public void On1()
    {
        ImgOne.SetActive(true);
        ImgTen.SetActive(false);
        ImgHundred.SetActive(false);
        ImgMax.SetActive(false);

        IsOneSelected = true;
        IsTenSelected = false;
        IsHundredSelected = false;
        IsMaxSelected = false;
    }

    public void On10()
    {
        ImgOne.SetActive(false);
        ImgTen.SetActive(true);
        ImgHundred.SetActive(false);
        ImgMax.SetActive(false);

        IsOneSelected = false;
        IsTenSelected = true;
        IsHundredSelected = false;
        IsMaxSelected = false;

    }

    public void On100()
    {
        ImgOne.SetActive(false);
        ImgTen.SetActive(false);
        ImgHundred.SetActive(true);
        ImgMax.SetActive(false);

        IsOneSelected = false;
        IsTenSelected = false;
        IsHundredSelected = true;
        IsMaxSelected = false;
    }

    public void OnMax()
    {
        ImgOne.SetActive(false);
        ImgTen.SetActive(false);
        ImgHundred.SetActive(false);
        ImgMax.SetActive(true);

        IsOneSelected = false;
        IsTenSelected = false;
        IsHundredSelected = false;
        IsMaxSelected = true;
    }
}
