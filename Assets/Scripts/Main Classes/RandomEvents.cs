using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    public bool animalAttack;
    public float animalAttackChance;
    public float GlobalChanceNumber;

    private void Start()
    {
        animalAttackChance = 0.50f;
    }
    public void CheckEventOccurence()
    {
        GlobalChanceNumber = Random.Range(0, 1);
        Debug.Log(GlobalChanceNumber);
        if (animalAttackChance == GlobalChanceNumber)
        {
            animalAttack = true;
            Debug.Log(animalAttack + " You're being attack by animals");
        }
    }
}
