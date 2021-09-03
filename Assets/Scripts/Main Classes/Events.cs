using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Events : MonoBehaviour
{
    public float  animalAttack, villageUnderAttack, randomNumber;
    public TMP_Text txtHistoryLog, txtNotificationText;
    private string _currentHistoryLog;
    public TMP_Text txtAvailableWorkers;
    private bool eventHappened;

    public GameObject scrollViewObject;
    public Animator animatorNotification;

    private float _timer = 1f;

    private void StoneAgeEvents()
    {
        animalAttack = 1f; //1% Probably need to lower this or lower the amount of time we generate random numbers
        // Probably need to generate a random number once every day?
        villageUnderAttack = 0.3f; //0.3%
        randomNumber = UnityEngine.Random.Range(0f, 100f);

        if (randomNumber <= animalAttack)
        {
            AnimalAttack();
            eventHappened = true;
        }
        if (randomNumber <= villageUnderAttack)
        {
            VillageAttack();
            eventHappened = true;
        }

        // These random events shouldn't start happening after the first time of launching the game. Maybe make it so that once the player reaches a certain point in the tutorial
        // Or if they reach a certain amount of a building such as potatoField.
    }
    private bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    private void AnimationHandler()
    {
        if (eventHappened == true)
        {
            if (IsPlaying(animatorNotification, "Notification_Display"))
            {
                animatorNotification.Play("Notification_Display", -1, 0);
            }
            else if (IsPlaying(animatorNotification, "Notification_Down"))
            {
                animatorNotification.SetTrigger("Start_Notification");
            }
            else
            {
                animatorNotification.SetTrigger("Start_Notification");
            }
            eventHappened = false;
        }
    }
    private void AnimalAttack()
    {
        
        float victoryChance = 50f;
        float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);
        if (randomNumberGenerated <= victoryChance)
        {
            float randomFoodAmount = UnityEngine.Random.Range(0f, 500f);
            if (randomFoodAmount + Resource.Resources[ResourceType.Food].amount > Resource.Resources[ResourceType.Food].storageAmount)
            {
                Resource.Resources[ResourceType.Food].amount = Resource.Resources[ResourceType.Food].storageAmount;
            }
            else
            {
                Resource.Resources[ResourceType.Food].amount += randomFoodAmount;

            }
            NotableEvent(string.Format("You've been attacked by an animal. But your people managed to kill it! You've gained {0:0.00} Food", randomFoodAmount));
        }
        else
        {
            uint randomWorkerAmount = (uint)UnityEngine.Random.Range(0, 5);
            
            if (Worker.TotalWorkerCount - randomWorkerAmount <= 0)
            {
                Worker.TotalWorkerCount = 0;
            }
            else
            {
                Worker.TotalWorkerCount -= randomWorkerAmount;
            }
            NotableEvent(string.Format("You've been attacked by an animal. {0} of your people has been killed.", randomWorkerAmount));
        }
        

        // Here we should roll another dice to see if the player can kill the animal or not.
        // If killed gets a random amount of food between generous values.
        // If the player can't kill the animal then a worker dies or multiple.
    }
    private void VillageAttack()
    {

        float victoryChance = 40f;
        float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);
        if (randomNumberGenerated <= victoryChance)
        {
            NotableEvent("Your civilization was attacked by a neighboring civilization but you manage to defeat the attackers");
        }
        else
        {
            uint randomWorkerAmount = (uint)UnityEngine.Random.Range(0, 5);

            if (Worker.TotalWorkerCount - randomWorkerAmount <= 0)
            {
                Worker.TotalWorkerCount = 0;
            }
            else
            {
                Worker.TotalWorkerCount -= randomWorkerAmount;
            }
            NotableEvent(string.Format("Your civilization was attacked by a neighboring civilization, {0} of your people has been killed.", randomWorkerAmount));
        }


        // Then display everything that has been stolen and also display how many people have been killed and/or injured if we want a injuring system which
        // mioght just be too much effort.
    }
    private void NewCraftingRecipe()
    {
        if (Craftable.isUnlockedEvent)
        {
            eventHappened = true;
            NotableEvent("You've unlocked a new crafting recipe.");
            Craftable.isUnlockedEvent = false;
        }
    }
    private void NewResearchAvailable()
    {
        if (Researchable.isUnlockedEvent)
        {
            eventHappened = true;
            NotableEvent("You've unlocked new research.");
            Researchable.isUnlockedEvent = false;
        }
    }
    private void NewBuildingAvailable()
    {
        if (Building.isUnlockedEvent)
        {
            eventHappened = true;
            NotableEvent("You've unlocked a new building.");
            Building.isUnlockedEvent = false;
        }
    }
    private void NewWorkerJobAvailable()
    {
        if (Worker.isUnlockedEvent)
        {
            eventHappened = true;
            NotableEvent("You've unlocked a new job.");
            Worker.isUnlockedEvent = false;
        }
    }
    // When generating workers, display the current total amount of workers next to the notification.
    // Need to rethink the different timers here, maybe have two seperate timers for stoneageevents and workers.
    // Or there could be a more elegant solution.
    // Maybe put the animation part in update without any timer.
    // And then just do something to exit the animation completely after the idle duration perhaps.
    // Then just make sure the animation starts again correctly.
    private void GenerateWorkers()
    {
        if (Worker.TotalWorkerCount < MakeshiftBed._selfCount)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = 10f;

                eventHappened = true;
                Worker.UnassignedWorkerCount++;
                Worker.TotalWorkerCount++;
                NotableEvent(string.Format("A worker has arrived [{0}]", Worker.TotalWorkerCount));
                txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", Worker.UnassignedWorkerCount);
                
                if (AutoToggle.isAutoWorkerOn == 1)
                {
                    AutoWorker.CalculateWorkers();
                    AutoWorker.AutoAssignWorkers();
                }
            }
        }
    }
    private void NotableEvent(string notableEventString)
    {
        //When event triggers, check to see on what panel you are currently.
        //If the panel is not the panel where the event took place. 
        //Point a dot towards that side of the panel.
        //So if workerpanel is active and and event happened on the building panel.
        //Have left dot be assigned to 1.
        //Question is where should I have this code, I'm thinking I should have that code inside here.
        //Currently events gets checked in update, which is completely wrong. It should only execute when an event occurs.
        //Go study events/delegates tomorrow.
        

        // Write to a history log whenever something notable happens. 
        _currentHistoryLog = txtHistoryLog.text;
        if (_currentHistoryLog == "")
        {
            txtHistoryLog.text = string.Format("{0}<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
        }
        else
        {
            txtHistoryLog.text = string.Format("{0}\n<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
            Canvas.ForceUpdateCanvases();
            scrollViewObject.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 0f;

        }

        txtNotificationText.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
    }
    void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 1f;
            StoneAgeEvents();           
        }
       
        GenerateWorkers();
        NewCraftingRecipe();
        NewResearchAvailable();
        NewBuildingAvailable();
        NewWorkerJobAvailable();
        AnimationHandler();
    }
}
