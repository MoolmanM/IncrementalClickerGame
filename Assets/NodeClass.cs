using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeClass : MonoBehaviour
{
    public static Dictionary<GameObject, NodeClass> Nodes = new Dictionary<GameObject, NodeClass>();

    private Button _btnMain;
    private bool _isUnlocked, _isClickable, _isExpanded;
    public GameObject[] neighbours;
    public RarityType associatedRarityType;

    public static bool isFirstPurchase = true;
    public static List<GameObject> neighbourCache = new List<GameObject>();

    public float passiveCost;

    private Transform tformTxtDescription, tformTxtCost, tformObjExpand;
    public TMP_Text txtDescription, txtCost;
    private GameObject objExpand;
    protected void InitializeObjects()
    {
        _isExpanded = false;
        tformTxtDescription = transform.Find("Rarity/ExpandedButton/DescriptionPanel/txtDescription");
        tformTxtCost = transform.Find("Rarity/ExpandedButton/CostPanel/txtCost");
        tformObjExpand = transform.Find("Rarity/ExpandedButton");

        txtDescription = tformTxtDescription.gameObject.GetComponent<TMP_Text>();
        txtCost = tformTxtCost.gameObject.GetComponent<TMP_Text>();
        objExpand = tformObjExpand.gameObject;

        objExpand.SetActive(false);
        _btnMain = GetComponent<Button>();

        _btnMain.onClick.AddListener(OnExpand);
        objExpand.GetComponent<Button>().onClick.AddListener(OnBuy);

        // This code will have to help whenever the player chooses to prestige. Not at start as it is currently.
        // Same with randomizing the rarities and such.
        if (isFirstPurchase && passiveCost <= Prestige.prestigePoints)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
    private void OnExpand()
    {
        foreach (var node in Nodes)
        {
            if (node.Value._isExpanded)
            {
                node.Value.objExpand.SetActive(false);
                node.Value._isExpanded = false;
            }         
        }
        if (_isExpanded)
        {
            objExpand.SetActive(false);
            _isExpanded = false;          
        }
        else
        {
            objExpand.SetActive(true);
            _isExpanded = true;
        }
        
    }
    private void OnBuy()
    {
        // Okay so this code seems to work perfectly
        // Might be some room for optimizations, but will leave it like this now.

        if (isFirstPurchase)
        {
            foreach (var node in Nodes)
            {
                node.Key.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
        }

        if (Prestige.prestigePoints >= passiveCost && !_isUnlocked && isFirstPurchase)
        {
            isFirstPurchase = false;

            Prestige.prestigePoints -= passiveCost;
            _isUnlocked = true;
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            foreach (var node in Nodes)
            {
                foreach (var neighbour in neighbours)
                {
                    if (neighbourCache != null)
                    {
                        if (neighbourCache.Contains(gameObject))
                        {
                            neighbourCache.Remove(gameObject);
                        }
                        foreach (var neighbourCached in neighbourCache)
                        {
                            if (node.Key == neighbourCached && Prestige.prestigePoints < node.Value.passiveCost)
                            {
                                node.Key.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                            }
                        }
                    }

                    if (node.Key == neighbour && !node.Value._isUnlocked && Prestige.prestigePoints >= node.Value.passiveCost)
                    {
                        node.Key.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        neighbourCache.Add(node.Key);
                    }
                }
            }

            Prestige.txtPoints.text = string.Format("Prestige Points: {0}", Prestige.prestigePoints);
            objExpand.SetActive(false);

        }
        else if (Prestige.prestigePoints >= passiveCost && !_isUnlocked && neighbourCache.Contains(gameObject))
        {
            isFirstPurchase = false;

            Prestige.prestigePoints -= passiveCost;
            _isUnlocked = true;
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            foreach (var node in Nodes)
            {
                foreach (var neighbour in neighbours)
                {
                    if (neighbourCache != null)
                    {
                        if (neighbourCache.Contains(gameObject))
                        {
                            neighbourCache.Remove(gameObject);
                        }
                        foreach (var neighbourCached in neighbourCache)
                        {
                            if (node.Key == neighbourCached && Prestige.prestigePoints < node.Value.passiveCost)
                            {
                                node.Key.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                            }
                        }
                    }

                    if (node.Key == neighbour && !node.Value._isUnlocked && Prestige.prestigePoints >= node.Value.passiveCost)
                    {
                        node.Key.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        neighbourCache.Add(node.Key);
                    }
                }
            }

            Prestige.txtPoints.text = string.Format("Prestige Points: {0}", Prestige.prestigePoints);
            objExpand.SetActive(false);
        }
    }
}
