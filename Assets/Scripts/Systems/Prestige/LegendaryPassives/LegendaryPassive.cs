using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LegendaryType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
    Passive6,
    Passive7,
    Passive8,
    Passive9,
    Passive10,
}

public class LegendaryPassive : MonoBehaviour
{
    public static Dictionary<LegendaryType, LegendaryPassive> LegendaryPassives = new Dictionary<LegendaryType, LegendaryPassive>();
    public LegendaryType Type;
    [System.NonSerialized] public string description;
    protected int _index;

    public virtual void InitializePermanentStat()
    {

    }
    public virtual void InitializePrestigeStat()
    {

    }
    public virtual void InitializePrestigeButton()
    {

    }
    public virtual void InitializePrestigeButtonCrafting(CraftingType craftingType)
    {

    }
    public virtual void InitializePrestigeButtonBuilding(BuildingType buildingType)
    {

    }
    public virtual void InitializePrestigeButtonResearch(ResearchType researchType)
    {

    }
    public virtual void InitializePrestigeButtonWorker(WorkerType workerType)
    {

    }
    public virtual CraftingType ReturnCraftingType()
    {
        return CraftingType.None;
    }
    public virtual BuildingType ReturnBuildingType()
    {
        return BuildingType.None;
    }
    public virtual ResearchType ReturnResearchType()
    {
        return ResearchType.None;
    }
    public virtual WorkerType ReturnWorkerType()
    {
        return WorkerType.None;
    }
}
