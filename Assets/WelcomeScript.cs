using TMPro;
using UnityEngine;

public class WelcomeScript : MonoBehaviour
{
    public GameObject objPrefabEarnedPanel, objPrefabSpacer;
    public Transform tformMainPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (TimeManager.hasPlayedBefore)
        {
            foreach (var resource in Resource.Resources)
            {
                if (resource.Value.isUnlocked)
                {
                    GameObject newObj = Instantiate(objPrefabEarnedPanel, tformMainPanel);
                    Instantiate(objPrefabSpacer, tformMainPanel);

                    Transform _tformNewObj = newObj.transform;
                    Transform _tformEarnedName = _tformNewObj.Find("Resource_Name");
                    Transform _tformEarnedAmount = _tformNewObj.Find("Resource_Amount");

                    TMP_Text txtName = _tformEarnedName.GetComponent<TMP_Text>();
                    TMP_Text txtAmount = _tformEarnedAmount.GetComponent<TMP_Text>();

                    float amountEarnedWhileAFK = (float)(TimeManager.difference.TotalSeconds * resource.Value.amountPerSecond);
                    float differenceAmount = resource.Value.storageAmount - amountEarnedWhileAFK;

                    if (resource.Value.amountPerSecond > 0)
                    {
                        if (resource.Value.amount == resource.Value.storageAmount)
                        {
                            txtAmount.text = string.Format("<color=#D71C2A>{0:0.00}</color> / {1:0.00}", 0, amountEarnedWhileAFK);
                        }
                        else if (amountEarnedWhileAFK <= resource.Value.storageAmount - resource.Value.amount)
                        {
                            txtAmount.text = string.Format("{0:0.00}", differenceAmount);
                        }
                        else
                        {
                            txtAmount.text = string.Format("<color=#D71C2A>{0:0.00}</color> / {1:0.00}", differenceAmount, amountEarnedWhileAFK);
                        }
                    }
                    else
                    {
                        txtAmount.text = string.Format("<color=#D71C2A>N/A</color>");
                    }

                    txtName.text = resource.Value.name;
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
