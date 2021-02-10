using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float _timer, _timerForResources, _timerForDayIncrement, incrementThis;
    private string seasonText;
    private int day, year, seasonCount;

    private void Start()
    {
        _timer = 0f;
        _timerForDayIncrement = 5f;
        _timerForResources = 0.1f;
    }
    private void Update()
    {
        float _foodResourceAmount = GameManager.Instance.resourceList[0].inputValues.resourceAmount;

        if ((_timerForDayIncrement -= Time.deltaTime) <= 0)
        {
            _timer = _timerForResources;
            GameManager.Instance.UpdateResources();
        }
        
        
    }
    private void CaculateSeason()
    {
        day++;
        if (seasonCount == 0)
        {
            seasonText = "Spring";
        }
        else if (seasonCount == 1)
        {
            seasonText = "Summer";
        }
        else if (seasonCount == 2)
        {
            seasonText = "Fall";
        }
        else if (seasonCount == 3)
        {
            seasonText = "Winter";
        }
        else
        {
            seasonCount = 0;
        }

        if (day == 100 && seasonCount == 3)
        {
            year++;
            seasonCount++;
            day = 0;
        }

        else if (day == 100)
        {
            seasonCount++;
            day = 0;
        }

    }
}
