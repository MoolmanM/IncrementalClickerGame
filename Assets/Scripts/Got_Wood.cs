using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Got_Wood : Achievement
{
    private Achievement _achievement;

    void Awake()
    {
        _achievement = GetComponent<Achievement>();
        Achievements.Add(type, _achievement);
    }
}
