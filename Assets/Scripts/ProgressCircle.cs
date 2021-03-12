using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour
{
    public float _current, _max;
    public Image _progressCircle;

    public void Start()
    {
        GetCurrentFill(_current, _max, _progressCircle);
    }
    public void GetCurrentFill(float current, float max, Image progressCircle)
    {
        float fillAmount = current / max;
        progressCircle.fillAmount = fillAmount;
    }
}
