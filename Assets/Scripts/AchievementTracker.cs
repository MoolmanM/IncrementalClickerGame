using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AchievementTracker : MonoBehaviour
{
    [Button]
    public void UpdateAchievements()
    {
        foreach (var item in Resource.Resources)
        {
            if (item.Value.isUnlocked)
            {
                Debug.Log(item.Value.Type + ": " + item.Value.trackedAmount);
            }   
        }
        
        // Remember to save these values to playerprefs and add the afk amount to these value as well.
    }
}
