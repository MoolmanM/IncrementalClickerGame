using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AchievementType
{
    GotWood,
    GotStone,
    FeedingTime
}

[System.Serializable]
public struct AchievementTier
{
    public float amountNeeded;
    public GameObject objAssociatedTrophy;
    public bool isAchieved;
    public bool isAchieveable;
    public Button btnAchieve;
    public uint gemAmount;
}

public class Achievement : MonoBehaviour
{
    public static Dictionary<AchievementType, Achievement> Achievements = new Dictionary<AchievementType, Achievement>();

    public AchievementType type;
    public AchievementTier[] achievementTier = new AchievementTier[3];
    public string ActualName;
    public string strAmountNeeded;
    public TMP_Text txtAmountNeeded;
    public ResourceType typeNeeded;
    public Image imgProgressBar;
    public GameObject objAchievementPanel;

    public NotificationManager notificationManager;

    // This is just to make sure when you collect an achievement tier, that it collects it in order from smallest to largent element inside the array.
    // And so the player doesn't collect all the tiers on one click, so the player is more aware of what's happening.
    // Might implement a collect all button at some point for all achievements.
    // Also make sure to save this value to playerprefs.

    public int intTierIncrement = 0;

    public void Start()
    {
        for (int i = 0; i < achievementTier.Length; i++)
        {
            achievementTier[i].objAssociatedTrophy.SetActive(false);
            achievementTier[i].btnAchieve.interactable = false;
        }
    }

    public void GetCurrentFill(float amount, float amountNeeded)
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        add = amount;
        div = amountNeeded;
        if (add > div)
        {
            add = div;
        }

        fillAmount += add / div;
        imgProgressBar.fillAmount = fillAmount;
    }

    public void Update()
    {
        CheckIfAchieveable();
        UpdateAchievement();
    }
    public void OnAchieveButton()
    {
        if (achievementTier[intTierIncrement].isAchieveable)
        {
            achievementTier[intTierIncrement].objAssociatedTrophy.SetActive(true);
            achievementTier[intTierIncrement].isAchieved = true;
            achievementTier[intTierIncrement].isAchieveable = false;
            for (int i = 0; i < achievementTier.Length; i++)
            {
                if (achievementTier[i].isAchieveable)
                {
                    achievementTier[i].btnAchieve.interactable = true;

                    if (!notificationManager.objEnterOptionsNotification.activeSelf)
                    {
                        notificationManager.objEnterOptionsNotification.SetActive(true);
                    }
                    if (!notificationManager.objAchievementNotification.activeSelf)
                    {
                        notificationManager.objAchievementNotification.SetActive(true);
                    }
                }
                else
                {
                    achievementTier[i].btnAchieve.interactable = false;

                    if (notificationManager.objAchievementNotification.activeSelf)
                    {
                        notificationManager.objAchievementNotification.SetActive(false);
                    }
                    if (notificationManager.objEnterOptionsNotification.activeSelf)
                    {
                        notificationManager.objEnterOptionsNotification.SetActive(false);
                    }
                }
            }
        }

        intTierIncrement++;
    }
    private void CheckIfAchieveable()
    {
        // For every achievement tier achieved you need to send a notification.
        // I don't think I should number the achievement, just show a message and then a dot to point the player in the direction.
        // This should go for all other notifications as well, remove the number it's not needed. Just a nice big dot that's visible and attracts attention.

        for (int i = 0; i < achievementTier.Length; i++)
        {
            if (Resource.Resources[typeNeeded].amountPerSecond != 0 || Resource.Resources[typeNeeded].amount != 0)
            {
                if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded + achievementTier[2].amountNeeded) && !achievementTier[2].isAchieved && !achievementTier[2].isAchieveable)
                {
                    // Send notification
                    // If you reached tier 2, but you haven't collected tier 1 yet, I should make it collect tier 1 first, then the player has to click again to collect tier 2
                    if (!notificationManager.objEnterOptionsNotification.activeSelf)
                    {
                        notificationManager.objEnterOptionsNotification.SetActive(true);
                        notificationManager.objAchievementNotification.SetActive(true);
                    }
                    achievementTier[2].isAchieveable = true;
                    achievementTier[2].btnAchieve.interactable = true;
                }
                else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded) && !achievementTier[1].isAchieved && !achievementTier[1].isAchieveable)
                {
                    if (!notificationManager.objEnterOptionsNotification.activeSelf)
                    {
                        notificationManager.objEnterOptionsNotification.SetActive(true);
                        notificationManager.objAchievementNotification.SetActive(true);
                    }
                    achievementTier[1].isAchieveable = true;
                    achievementTier[1].btnAchieve.interactable = true;
                }
                else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded) && !achievementTier[0].isAchieved && !achievementTier[0].isAchieveable)
                {
                    if (!notificationManager.objEnterOptionsNotification.activeSelf)
                    {
                        notificationManager.objEnterOptionsNotification.SetActive(true);
                        notificationManager.objAchievementNotification.SetActive(true);
                    }
                    achievementTier[0].isAchieveable = true;
                    achievementTier[0].btnAchieve.interactable = true;
                }
                else
                {
                    //Debug.Log("This shouldn't happen");
                    //GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                    //strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                }
            }
            txtAmountNeeded.text = strAmountNeeded;
        }
    }
    private void UpdateAchievement()
    {
        // Updates the values of achievements, should only happen when you open the achievement menu.
        // When inside the achievement menu it should update dynamically.
        // When outside of the menu it should also check if you have completed an achievement so you can send a notification to the player.
        // But when checking for updates to the value when outside of the menu, it should check less often to save resources and it should also not update the text values.
        if (objAchievementPanel.GetComponent<GraphicRaycaster>().enabled)
        {
            // For every achievement tier achieved you need to send a notification.
            // I don't think I should number the achievement, just show a message and then a dot to point the player in the direction.
            // This should go for all other notifications as well, remove the number it's not needed. Just a nice big dot that's visible and attracts attention.

            for (int i = 0; i < achievementTier.Length; i++)
            {
                if (Resource.Resources[typeNeeded].amountPerSecond != 0 || Resource.Resources[typeNeeded].amount != 0)
                {
                    if (Resource.Resources[typeNeeded].trackedAmount >= achievementTier[0].amountNeeded && !achievementTier[0].isAchieved)
                    {
                        GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                        strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                    }
                    else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded) && !achievementTier[1].isAchieved)
                    {
                        GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[1].amountNeeded);
                        strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(achievementTier[1].amountNeeded), NumberToLetter.FormatNumber(achievementTier[1].amountNeeded), typeNeeded);
                    }
                    else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded + achievementTier[2].amountNeeded) && !achievementTier[2].isAchieved)
                    {
                        GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[2].amountNeeded);
                        strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(achievementTier[2].amountNeeded), NumberToLetter.FormatNumber(achievementTier[2].amountNeeded), typeNeeded);
                    }
                    else
                    {
                        if (achievementTier[2].isAchieved)
                        {
                            GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount - (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded), achievementTier[2].amountNeeded);
                            strAmountNeeded = "Completed";
                        }
                        else if (achievementTier[1].isAchieved)
                        {
                            GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount - (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded), achievementTier[2].amountNeeded);
                            strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", Resource.Resources[typeNeeded].trackedAmount - (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded), NumberToLetter.FormatNumber(achievementTier[2].amountNeeded), typeNeeded);
                        }
                        else if (achievementTier[0].isAchieved)
                        {
                            GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount - (achievementTier[0].amountNeeded), achievementTier[1].amountNeeded);
                            strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", Resource.Resources[typeNeeded].trackedAmount - achievementTier[0].amountNeeded, NumberToLetter.FormatNumber(achievementTier[1].amountNeeded), typeNeeded);

                        }
                        else
                        {
                            GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                            strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                        }
                        //strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                        //GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                        //strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                    }
                }
                txtAmountNeeded.text = strAmountNeeded;
            }
        }
    }
    private void DepreactedUpdateAchievement()
    {
        // Updates the values of achievements, should only happen when you open the achievement menu.
        // When inside the achievement menu it should update dynamically.
        // When outside of the menu it should also check if you have completed an achievement so you can send a notification to the player.
        // But when checking for updates to the value when outside of the menu, it should check less often to save resources and it should also not update the text values.
        if (!objAchievementPanel.activeSelf)
        {
            // For every achievement tier achieved you need to send a notification.
            // I don't think I should number the achievement, just show a message and then a dot to point the player in the direction.
            // This should go for all other notifications as well, remove the number it's not needed. Just a nice big dot that's visible and attracts attention.

            for (int i = 0; i < achievementTier.Length; i++)
            {
                if (Resource.Resources[typeNeeded].amountPerSecond != 0 || Resource.Resources[typeNeeded].amount != 0)
                {
                    if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded + achievementTier[2].amountNeeded))
                    {
                        // Send notification
                        achievementTier[2].isAchieveable = true;
                    }
                    else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded))
                    {
                        achievementTier[1].isAchieveable = true;
                    }
                    else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded))
                    {
                        achievementTier[0].isAchieveable = true;
                    }
                    else
                    {

                        //GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                        //strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                    }
                }
                txtAmountNeeded.text = strAmountNeeded;
            }
        }
        else
        {
            if (Resource.Resources[typeNeeded].amountPerSecond != 0 || Resource.Resources[typeNeeded].amount != 0)
            {
                if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded + achievementTier[2].amountNeeded))
                {
                    achievementTier[2].isAchieveable = true;
                }
                else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded + achievementTier[1].amountNeeded))
                {
                    achievementTier[1].isAchieveable = true;
                }
                else if (Resource.Resources[typeNeeded].trackedAmount >= (achievementTier[0].amountNeeded))
                {
                    achievementTier[0].isAchieveable = true;
                }
                else
                {
                    //GetCurrentFill(Resource.Resources[typeNeeded].trackedAmount, achievementTier[0].amountNeeded);
                    //strAmountNeeded = string.Format("{0:0.00}/{1} - {2}", NumberToLetter.FormatNumber(Resource.Resources[typeNeeded].trackedAmount), NumberToLetter.FormatNumber(achievementTier[0].amountNeeded), typeNeeded);
                }
            }
            txtAmountNeeded.text = strAmountNeeded;
        }
    }
}
