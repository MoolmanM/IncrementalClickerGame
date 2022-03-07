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
                        if (amountEarnedWhileAFK + resource.Value.amount >= resource.Value.storageAmount)
                        {
                            // Can also made just type here "Storage Limit" In red color.
                            txtAmount.text = string.Format("<color=#D71C2A>{0:0.00}</color> / {1:0.00}", 0, amountEarnedWhileAFK);
                            resource.Value.amount = resource.Value.storageAmount;
                        }
                        else
                        {
                            txtAmount.text = string.Format("{0:0.00}", amountEarnedWhileAFK);
                            resource.Value.amount += amountEarnedWhileAFK;
                            //if (amountEarnedWhileAFK <= resource.Value.storageAmount - resource.Value.amount)
                            //{
                            //    txtAmount.text = string.Format("{0:0.00}", differenceAmount);
                            //}
                            //else
                            //{
                            //    txtAmount.text = string.Format("<color=#D71C2A>{0:0.00}</color> / {1:0.00}", differenceAmount, amountEarnedWhileAFK);
                            //}
                        }
                    }
                    else
                    {
                        txtAmount.text = string.Format("<color=#D71C2A>N/A</color>");
                    }

                    txtName.text = resource.Value.Type.ToString();
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
