using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive1: Reduce time it takes to research by a certain %.
//ePassive2: Increase production of ALL Workers by a certain %.
//ePassive3: Start next/each run with a certain number of a random Building.
//ePassive4: Start next/each run with a certain number of Workers.
//ePassive5: Increase amount of Prestige Points gained by a certain % or flat amount.
//ePassive6: Decrease initial cost of a random Building.
//ePassive7: Increase production of ALL production Buildings by a certain %.
//ePassive8: Decrease cost of ALL Craftables by a certain %.
//ePassive9: Decrease cost of ALL Researchables by a certain %.
//ePassive10: Increase storage limit of ALL storage Buildings by a certain %.
//ePassive11: Decrease cost of ALL Buildings by a certain %. *This might be a little to strong.

public enum EpicType
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
    Passive10
}

public class EpicPassive : MonoBehaviour
{
    public static Dictionary<EpicType, EpicPassive> EpicPassives = new Dictionary<EpicType, EpicPassive>();
    public EpicType Type;
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