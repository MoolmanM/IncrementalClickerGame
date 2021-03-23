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

    private float _timer = 0.1f;
    private readonly float maxValue = 0.01f;

    private float Timer = 0.1f;
    private readonly float MaxValue = 10f;

    private void StoneAgeEvents()
    {
        animalAttack = 1f;
        villageUnderAttack = 0.3f;
        randomNumber = UnityEngine.Random.Range(0f, 100f);

        if (randomNumber <= animalAttack)
        {
            AnimalAttack();
            NotableEvent("You've been attacked by an animal");
        }
        if (randomNumber <= villageUnderAttack)
        {
            NotableEvent("Your village is under attack!");
        }
    }

    private void AnimalAttack()
    {
        // Here we should roll another dice to see if the player can kill the animal or not.
        // If killed gets a random amount of food between generous values.
        // If the player can't kill the animal then a worker dies or multiple.
    }
    private void GenerateWorkers()
    {
        if (Worker.AvailableWorkerCount < MakeshiftBed.SelfCount)
        {
            if ((Timer -= Time.deltaTime) <= 0)
            {
                Timer = MaxValue;

                Worker.AvailableWorkerCount++;
                Debug.Log(Worker.AvailableWorkerCount);
                NotableEvent("A worker has arrived");          
            }
        }
    }

    private void NotableEvent(string notableEventString)
    {
        // Write to a history log whenever something notable happens. 
        _currentHistoryLog = txtHistoryLog.text;
        if (_currentHistoryLog == "")
        {
            txtHistoryLog.text = string.Format("{0}<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
        }
        else
        {
            txtHistoryLog.text = string.Format("{0}\n<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
        }
        
        txtNotificationText.text = string.Format("<b>{0:t}</b> {1}", DateTime.Now, notableEventString);
    }
    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;
            StoneAgeEvents();
            GenerateWorkers();
        }
    }
}
