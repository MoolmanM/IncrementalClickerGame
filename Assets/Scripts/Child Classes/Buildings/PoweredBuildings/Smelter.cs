using System.Collections.Generic;
using UnityEngine;

public class Smelter : Power
{
    private Building _building;


    // So for now
    // Furnace needs to deduct from Copper and Tin per second.
    // And increase Bronze per second
    // Every Furnance built will change these amounts by just timing those amounts by the amount of buildings built.
    // You can switch the Furnance on and off.
    // I'm still not sure if I should allow the player to put all off and on, or to have the ability to
    // put every individual building on and off.

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        InitializePower();
    }
}
