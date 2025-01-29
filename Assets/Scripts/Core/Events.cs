using System;
using TMPro;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static bool eventGood, eventBad, eventInfo, eventError, eventHappened;
    public float animalAttack, villageUnderAttack, randomNumber;
    public TMP_Text txtHistoryLog;
    private string _currentHistoryLog;
    public TMP_Text txtAvailableWorkers;

    public GameObject scrollViewObject;

    private float workerEatAmount = 0.20f;
    public float totalEatAmount;

    private float timer = 1f;
    // Use the Info event to display stuff like: "You've entered the bronze age"
    private void StoneAgeEvents()
    {
        animalAttack = 50f; //1% Probably need to lower this or lower the amount of time we generate random numbers
        // Probably need to generate a random number once every day?
        villageUnderAttack = 0.3f; //0.3%
        randomNumber = UnityEngine.Random.Range(0f, 100f);

        if (randomNumber <= animalAttack)
        {
            //AnimalAttack();
        }
        if (randomNumber <= villageUnderAttack)
        {
            // VillageAttack();
        }

        // These random events shouldn't start happening after the first time of launching the game. Maybe make it so that once the player reaches a certain point in the tutorial
        // Or if they reach a certain amount of a building such as potatoField.
    }
    private void AnimalAttack()
    {
        if (Worker.TotalWorkerCount > 0)
        {
            float victoryChance = 50f;
            float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

            // if I upgrade weapons in the future, I can always increase the victory chance percentage, or decrease.
            // Or I can generate the random number and then increment it with a certain value,
            // Call it a modifier, and this modifier will initially be 0%, but with other weapons
            // It might go up. 5%, 10%, 15%. etc.

            if (randomNumberGenerated <= victoryChance)
            {
                eventHappened = true;
                eventGood = true;

                float randomFoodAmount = UnityEngine.Random.Range(0f, 50f);
                randomFoodAmount *= Worker.TotalWorkerCount;
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
                // Should this only affect workers that are assigned to hunters?
                eventHappened = true;
                eventBad = true;
                // if you have no workers home, the animal then eats some of your food?
                // But you should almost always have a worker alive, so I think that's irrelevant
                uint randomWorkerAmount = (uint)UnityEngine.Random.Range(1, Worker.TotalWorkerCount + 1);
                Debug.Log(randomWorkerAmount);
                NotableEvent(string.Format("You've been attacked by an animal. {0} of your people has been killed.", randomWorkerAmount));

                if (Worker.UnassignedWorkerCount - randomWorkerAmount <= 0)
                {
                    Debug.Log(Worker.UnassignedWorkerCount);
                    randomWorkerAmount -= Worker.UnassignedWorkerCount;
                    Worker.UnassignedWorkerCount = 0;
                    txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);
                    foreach (var worker in Worker.Workers)
                    {
                        while (randomWorkerAmount != 0)
                        {
                            if (worker.Value.workerCount > 0)
                            {
                                worker.Value.workerCount--;
                                randomWorkerAmount--;
                                worker.Value.txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", worker.Key, worker.Value.workerCount);
                            }
                            Debug.Log("Random worker amount: " + randomWorkerAmount + " Unassigned workers: " + Worker.UnassignedWorkerCount);
                        }
                    }
                }
                else
                {
                    Worker.UnassignedWorkerCount -= randomWorkerAmount;
                    txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);
                }

                Worker.TotalWorkerCount -= randomWorkerAmount;
            }
        }

        // Here we should roll another dice to see if the player can kill the animal or not.
        // If killed gets a random amount of food between generous values.
        // If the player can't kill the animal then a worker dies or multiple.
    }
    private void VillageAttack()
    {
        if (Worker.TotalWorkerCount > 0)
        {
            float victoryChance = 40f;
            float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);
            if (randomNumberGenerated <= victoryChance)
            {
                eventHappened = true;
                eventGood = true;
                NotableEvent("Your civilization was attacked by a neighboring civilization but you manage to defeat the attackers");
            }
            else
            {
                eventHappened = true;
                eventBad = true;

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
        }
        // Then display everything that has been stolen and also display how many people have been killed and/or injured if we want a injuring system which
        // mioght just be too much effort.
    }
    private void HasNotEnoughEnergy()
    {
        if (Power.hasNotEnoughEnergy)
        {
            eventHappened = true;
            eventError = true;
            NotableEvent("You do not have enough energy production.");
            Power.hasNotEnoughEnergy = false;
        }
    }
    private void LessThanZeroAPS()
    {
        if (Power.hasLessThanZeroAPS)
        {
            eventHappened = true;
            eventError = true;
            NotableEvent("Can't have less than zero resource gain");
            Power.hasLessThanZeroAPS = false;
        }
    }
    private void HasReachedMaxSimulResearch()
    {
        if (Researchable.hasReachedMaxSimulResearch)
        {
            eventHappened = true;
            eventError = true;
            NotableEvent("You can't research any more simultaneously.");
            Researchable.hasReachedMaxSimulResearch = false;
        }
    }
    private void NewCraftingRecipe()
    {
        if (Craftable.isCraftableUnlockedEvent)
        {
            eventHappened = true;
            eventGood = true;
            NotableEvent("You've unlocked a new crafting recipe.");
            Craftable.isCraftableUnlockedEvent = false;
        }
    }
    private void NewResearchAvailable()
    {
        if (Researchable.isResearchableUnlockedEvent)
        {
            eventHappened = true;
            eventGood = true;
            NotableEvent("You've unlocked a new research option.");
            Researchable.isResearchableUnlockedEvent = false;
        }
    }
    private void NewBuildingAvailable()
    {
        if (Building.isBuildingUnlockedEvent)
        {
            eventHappened = true;
            eventGood = true;
            NotableEvent("You've unlocked a new building.");
            Building.isBuildingUnlockedEvent = false;
        }
    }
    private void NewWorkerJobAvailable()
    {
        if (Worker.isWorkerUnlockedEvent)
        {
            eventHappened = true;
            eventGood = true;
            NotableEvent("You've unlocked a new job.");
            Worker.isWorkerUnlockedEvent = false;
        }
    }
    public void GenerateWorker()
    {
        //if (hutSelfCount != 0 && Worker.TotalWorkerCount < hutSelfCount)
        //{
        //    if ((timer -= Time.deltaTime) <= 0)
        //    {
        //        timer = 10f;

        //        eventHappened = true;
        //        eventGood = true;
        //        //Worker.AliveCount++;
        //        Worker.UnassignedWorkerCount++;
        //        Worker.TotalWorkerCount++;
        //        NotableEvent(string.Format("A worker has arrived [{0}]", Worker.TotalWorkerCount));
        //        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);

        //        // Turn this into a bool.
        //        if (AutoToggle.isAutoWorkerOn == 1)
        //        {
        //            AutoWorker.CalculateWorkers();
        //            AutoWorker.AutoAssignWorkers();
        //        }
        //    }
        //}


        eventHappened = true;
        eventGood = true;
        Worker.UnassignedWorkerCount++;
        Worker.TotalWorkerCount++;
        Worker.trackedWorkerCount++;
        NotableEvent(string.Format("A worker has arrived [{0}]", Worker.TotalWorkerCount));
        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);

        // Turn this into a bool.
        if (AutoToggle.isAutoWorkerOn == 1)
        {
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
        }

    }
    private void KillWorker()
    {
        // Killing workers should probably also happen at timer = 10f
        // So here it should just modify the APS of Food specifically ONCE per tick I guess?
        // But only do it once per pick if the APS that it modifies it with changes, otherwise it's just unessecary
        // So I think the amount per second needs to be modified after a worker has starved.
        bool hasWorkerStarved = false;

        // No matter what the amount per second should always change, even if food is already zero and APS is less than zero.
        // Food should just be forces to stay at zero.
        if (Worker.AliveCount > 0 && Resource.Resources[ResourceType.Food].amount + (Resource.Resources[ResourceType.Food].amountPerSecond - (Worker.AliveCount * workerEatAmount)) <= 0)
        {
            // Also if this happens you can set food to zero 'permanently' until APS is >= 0, otherwise you'll be calling code for no reason.
            // Make sure food doesn't go below 0, should probably do that in resources though, but in food specifically,
            // If another resource goes below 0, I want to see it because then it means something went wrong.
            // This is where you starve.
            // Maybe workers that are not working should not minus food per second?

            if (Worker.UnassignedWorkerCount > 0 && !hasWorkerStarved)
            {
                Worker.UnassignedWorkerCount--;
                Worker.AliveCount--;
                hasWorkerStarved = true;
            }
            else if (Worker.UnassignedWorkerCount == 0)
            {
                foreach (var kvp in Worker.Workers)
                {
                    if (kvp.Value.workerCount > 0 && !hasWorkerStarved)
                    {
                        //kvp.Value.workerCount--;
                        Worker.AliveCount--;
                        hasWorkerStarved = true;
                        kvp.Value.OnMinusButton();
                        Worker.UnassignedWorkerCount--;
                        //kvp.Value.txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", kvp.Value.ActualName.ToString(), kvp.Value.workerCount);
                    }
                }
            }


            // So you can just + APS of food here by the eat amount, no need to multiply by workeramount because just one dies at a time.
            // Also modify the resource APS when a worker dies.
            // Worker dies.
            // And make sure in generate workers that they only come back when it's more than zero.

            Resource.Resources[ResourceType.Food].amountPerSecond += workerEatAmount;
            StaticMethods.ModifyAPSText(Resource.Resources[ResourceType.Food].amountPerSecond, Resource.Resources[ResourceType.Food].uiForResource.txtAmountPerSecond);
            eventHappened = true;
            eventBad = true;
            NotableEvent(string.Format("A worker has died [{0}]", Worker.AliveCount));
            txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);

        }
        //else
        //{
        //    // This needs to happen everytime a worker gets generated.
        //    // And then again everytime a worker dies.
        //    // So all of this code needs to be inside generate woker.
        //    Resource.Resources[ResourceType.Food].amountPerSecond += totalEatAmount;
        //    totalEatAmount = 0;
        //    totalEatAmount = Worker.AliveCount * workerEatAmount;
        //    Resource.Resources[ResourceType.Food].amountPerSecond -= totalEatAmount;
        //    Resource.Resources[ResourceType.Food].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[ResourceType.Food].amountPerSecond);
        //}
    }
    private void DecreaseFoodAPS()
    {
        Resource.Resources[ResourceType.Food].amountPerSecond -= workerEatAmount;
        StaticMethods.ModifyAPSText(Resource.Resources[ResourceType.Food].amountPerSecond, Resource.Resources[ResourceType.Food].uiForResource.txtAmountPerSecond);
        //Resource.Resources[ResourceType.Food].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[ResourceType.Food].amountPerSecond);
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
            // How is expensive is this?
            // Resource wise?

            Canvas.ForceUpdateCanvases();
            scrollViewObject.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 0f;

        }

        PopUpNotification.txtBad.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
        PopUpNotification.txtInfo.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
        PopUpNotification.txtGood.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
        PopUpNotification.txtError.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
        //txtNotificationText.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
    }
    void Update()
    {
        // Should maybe have all of these methods in their own class so they execute at the same time instead of in this order.
        //GenerateWorker();
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = 1f;
            StoneAgeEvents();
            //KillWorker();
        }
        NewCraftingRecipe();
        NewResearchAvailable();
        NewBuildingAvailable();
        NewWorkerJobAvailable();
        HasReachedMaxSimulResearch();
        HasNotEnoughEnergy();
        LessThanZeroAPS();
        PopUpNotification.HandleAnim();
    }
}
