using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public string seasonString;
    private int day, year, season, seasonCount;
    public void CalculateSeason()
    {
        day++;
        if (seasonCount == 0)
        {
            seasonString = "Spring";
        }
        else if (seasonCount == 1)
        {
            seasonString = "Summer";
        }
        else if (seasonCount == 2)
        {
            seasonString = "Fall";
        }
        else if (seasonCount == 3)
        {
            seasonString = "Winter";
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
