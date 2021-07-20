using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaft : Building
{
    private Building _building;
    // So this building will produce certain ores. Lets say copper and tin? 
    // ANd then we refine that inot bronze ingots if I remember correctly.
    // That will then be used to craft other stuff.
    // The refining process can maybe be done by a building.
    // Or just automatically when you want to build something.
    // Need to make resourcetypes to modify also an array for buildings. 
    // Because there will also be buildings in the future that will modify more than just one resource.
    // Need I also need to implement a resourcedecrementer like I did with workers. Because certain buildings such as the furnace, will decrement the 'energy' resource.
    // Then need to also start adding storage buildings, also buildings that accomplish misc stuff, such as make other stuff more effcient for example! 

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        _resourceMultiplier = 0.10f;
        _costMultiplier = 1.15f;
        resourceTypeToModify = ResourceType.Food;
        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText();
        //DisplayConsole();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    void Update()
    {
        UpdateResourceCosts();
    }
}
