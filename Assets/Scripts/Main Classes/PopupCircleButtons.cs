using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

public class PopupCircleButtons : MonoBehaviour
{
    //public Button btnPlus1, btnPlus10, btnPlus100, btnPlusMax, btnMinus1, btnMinus10, btnMinus100, btnMinusMax;
    public static int changeAmount;

    public void Start()
    {
        //btnPlus1.onClick.AddListener(delegate {OnButtonPlus1(selfCount, poweredCount);});
        //btnPlus10.onClick.AddListener(delegate { OnButtonPlus10(selfCount, poweredCount); });
        //btnPlus100.onClick.AddListener(delegate { OnButtonPlus100(selfCount, poweredCount); });
        //btnPlusMax.onClick.AddListener(delegate { OnButtonPlusMax(selfCount, poweredCount); });
        //btnMinus1.onClick.AddListener(delegate { OnButtonMinus1(poweredCount); });
        //btnMinus10.onClick.AddListener(delegate { OnButtonMinus10(poweredCount); });
        //btnMinus100.onClick.AddListener(delegate { OnButtonMinus100(poweredCount); });
        //btnMinusMax.onClick.AddListener(delegate { OnButtonMinusMax(poweredCount); });
    }
    private static void ButtonPlusMethod(int incrementAmount, uint currentSelfCount, uint poweredOnCount, TMP_Text tmpText)
    {
        uint difference = currentSelfCount - poweredOnCount;
        if (currentSelfCount > poweredOnCount && (incrementAmount + changeAmount) >= difference)
        {
            changeAmount = (int)difference;
        }
        else if (currentSelfCount > poweredOnCount && changeAmount < difference)
        {
            changeAmount += incrementAmount;
        }
        else
        {
            changeAmount += 0;
        }

        if (changeAmount > 0)
        {
            tmpText.color = Color.green;
            tmpText.text = "+" + (changeAmount).ToString();
        }
        else if(changeAmount < 0)
        {
            tmpText.color = Color.red;
            tmpText.text = (changeAmount).ToString();
        }
        else
        {
            tmpText.color = Color.white;
            tmpText.text = "";
        }
    }
    public static void OnButtonPlus1(uint currentSelfCount, uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonPlusMethod(1, currentSelfCount, poweredOnCount, tmpText);
    }
    public static void OnButtonPlus10(uint currentSelfCount, uint poweredOnCount, TMP_Text tmpText)
{
        ButtonPlusMethod(10, currentSelfCount, poweredOnCount, tmpText);
    }
    public static void OnButtonPlus100(uint currentSelfCount, uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonPlusMethod(100, currentSelfCount, poweredOnCount, tmpText);
    }
    public static void OnButtonPlusMax(uint currentSelfCount, uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonPlusMethod((int)currentSelfCount, currentSelfCount, poweredOnCount, tmpText);
    }
    private static void ButtonMinusMethod(int decrementAmount, uint poweredOnCount, TMP_Text tmpText)
    {
        if ((changeAmount + decrementAmount) < -poweredOnCount)
        {
            changeAmount = -(int)poweredOnCount;
        }
        else
        {
            changeAmount += decrementAmount;
        }

        if (changeAmount > 0)
        {
            tmpText.color = Color.green;
            tmpText.text = "+" + (changeAmount).ToString();
        }
        else if (changeAmount < 0)
        {
            tmpText.color = Color.red;
            tmpText.text = (changeAmount).ToString();
        }
        else
        {
            tmpText.color = Color.white;
            tmpText.text = "";
        }
    }
    public static void OnButtonMinus1(uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonMinusMethod(-1, poweredOnCount, tmpText);
    }
    public static void OnButtonMinus10(uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonMinusMethod(-10, poweredOnCount, tmpText);
    }
    public static void OnButtonMinus100(uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonMinusMethod(-100, poweredOnCount, tmpText);
    }
    public static void OnButtonMinusMax(int selfCount, uint poweredOnCount, TMP_Text tmpText)
    {
        ButtonMinusMethod(-selfCount, poweredOnCount, tmpText);
    }
}
