using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Decrease cost of random Researchable by a certain %.
public class uPassive7 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private ResearchType researchTypeChosen;
    private float permanentAmount = 0.023f, prestigeAmount = 0.115f;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
    }
    private void ChooseRandomResearchable()
    {
        List<ResearchType> researchTypesInCurrentRun = new List<ResearchType>();

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.IsUnlocked)
            {
                researchTypesInCurrentRun.Add(researchable.Key);
            }
        }
        if (researchTypesInCurrentRun.Count >= Prestige.researchablesUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, researchTypesInCurrentRun.Count);
            researchTypeChosen = researchTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.researchablesUnlockedInPreviousRun.Count);
            researchTypeChosen = Prestige.researchablesUnlockedInPreviousRun[_index];
        }
    }
    private void AddToPrestigeCache(float percentageAmount)
    {
        if (!PrestigeCache.prestigeBoxResearchableCostSubtraction.ContainsKey(researchTypeChosen))
        {
            PrestigeCache.prestigeBoxResearchableCostSubtraction.Add(researchTypeChosen, percentageAmount);
        }
        else
        {
            PrestigeCache.prestigeBoxResearchableCostSubtraction[researchTypeChosen] += percentageAmount;
        }
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        if (!PermanentCache.permanentBoxResearchableCostSubtraction.ContainsKey(researchTypeChosen))
        {
            PermanentCache.permanentBoxResearchableCostSubtraction.Add(researchTypeChosen, percentageAmount);
        }
        else
        {
            PermanentCache.permanentBoxResearchableCostSubtraction[researchTypeChosen] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease cost to research '{0}' by {1}%", Researchable.Researchables[researchTypeChosen].ActualName, percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomResearchable();
        ModifyStatDescription(permanentAmount);
        AddToPermanentCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomResearchable();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonResearch(ResearchType researchType)
    {
        AddToPrestigeCache(prestigeAmount);
    }
    public override ResearchType ReturnResearchType()
    {
        return researchTypeChosen;
    }
}
