using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnPerSecondTick(int id);
        public static event OnPerSecondTick onPerSecondTick;

    void OnValueChanged(int id)
    {
        List<Resource> _resourceList = GameManager.Instance.resourceList;

        for (int i = 0; i < _resourceList.Count; i++)
        {
            float _resourceAmount = _resourceList[id].inputValues.resourceAmount;

            if (_resourceAmount != _resourceList[id].inputValues.resourceAmount)
            {
                if (onPerSecondTick != null)
                    onPerSecondTick(id);
                
            }
        }
        
    }
}
  