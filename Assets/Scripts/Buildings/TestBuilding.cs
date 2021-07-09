using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestBuilding : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceCostTest
    {
        public ResourceType resourceType;
        [System.NonSerialized] public float currentAmount;
        public float costAmount;
        [System.NonSerialized] public UiForResourceCost uiForResourceCost;
    }

    public ResourceCostTest[] resourceCostTest;
    public GameObject costPrefab;
    public Transform _tformBody;

    public void Start()
    {
        for (int i = 0; i < resourceCostTest.Length; i++)
        {
            GameObject newObj = Instantiate(costPrefab, _tformBody);
            Transform _tformNewObj = newObj.transform;
            Transform _tformCostName = _tformNewObj.Find("Cost_Name_Panel/Text_CostName");
            Transform _tformCostAmount = _tformNewObj.Find("Cost_Amount_Panel/Text_CostAmount");

            resourceCostTest[i].uiForResourceCost.textCostName = _tformCostName.GetComponent<TMP_Text>();
            resourceCostTest[i].uiForResourceCost.textCostAmount = _tformCostAmount.GetComponent<TMP_Text>();

            resourceCostTest[i].currentAmount = Resource.Resources[resourceCostTest[i].resourceType].amount;
            resourceCostTest[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", resourceCostTest[i].currentAmount, resourceCostTest[i].costAmount);
            resourceCostTest[i].uiForResourceCost.textCostName.text = string.Format("{0}", resourceCostTest[i].resourceType.ToString());
        }
    }
}
