using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Connections
{
    public GameObject objConnection;
    public bool isHighlighted;
}

public class NodeClass : MonoBehaviour
{
    public static Dictionary<GameObject, NodeClass> Nodes = new Dictionary<GameObject, NodeClass>();

    private Button _btnMain;
    private bool _isUnlocked;
    public GameObject[] neighbours;
    [System.NonSerialized] public RarityType associatedRarityType;
    public Connections[] connections;
    public static List<GameObject> connectionCache = new List<GameObject>();

    public static bool isFirstPurchase = true;
    public static List<GameObject> neighbourCache = new List<GameObject>();

    public float passiveCost;
    private Color colHighlight, colUnhighlight;
    private Transform tformTxtDescription, tformTxtCost, tformObjExpand;
    public TMP_Text txtDescription, txtCost;
    [System.NonSerialized] public GameObject objExpand;
    private GameObject _objPrestigeTree;

    protected void InitializeObjects()
    {
        _objPrestigeTree = gameObject;
        colHighlight = new Color(1, 1, 1, 1);
        colUnhighlight = new Color(0.5137255f, 0.5137255f, 0.5137255f, 1);
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
            GetComponent<Image>().color = colHighlight;
        }
    }
    private void OnExpand()
    {
        // I feel this code and the onbuy code can still be omptimized.
        if (isFirstPurchase)
        {
            foreach (var node in Nodes)
            {
                node.Value.objExpand.SetActive(false);
            }
            objExpand.SetActive(true);
        }
        else if (!_isUnlocked && Prestige.prestigePoints >= passiveCost && neighbourCache.Contains(gameObject))
        {
            foreach (var node in Nodes)
            {
                node.Value.objExpand.SetActive(false);
            }
            objExpand.SetActive(true);
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
                node.Key.GetComponent<Image>().color = colUnhighlight;
            }
        }

        if (Prestige.prestigePoints >= passiveCost && !_isUnlocked && isFirstPurchase)
        {
            isFirstPurchase = false;

            Prestige.prestigePoints -= passiveCost;
            _isUnlocked = true;
            GetComponent<Image>().color = colUnhighlight;

            foreach (var node in Nodes)
            {
                foreach (var neighbour in neighbours)
                {
                    if (neighbourCache != null)
                    {
                        foreach (var neighbourCached in neighbourCache)
                        {
                            if (node.Key == neighbourCached && Prestige.prestigePoints < node.Value.passiveCost)
                            {
                                node.Key.GetComponent<Image>().color = colUnhighlight;
                            }
                        }
                    }

                    if (node.Key == neighbour && !node.Value._isUnlocked && Prestige.prestigePoints >= node.Value.passiveCost)
                    {
                        //Debug.Log("How many times does this happen");
                        #region Connections
                        foreach (var connection in connections)
                        {
                            foreach (var nodeConnect in node.Value.connections)
                            {
                                if (connection.objConnection == nodeConnect.objConnection)
                                {
                                    if (!connectionCache.Contains(connection.objConnection))
                                    {
                                        connection.objConnection.GetComponent<Image>().color = colHighlight;
                                        connectionCache.Add(connection.objConnection);
                                    }
                                    else
                                    {
                                        connection.objConnection.GetComponent<Image>().color = colUnhighlight;
                                    }
                                }
                            }
                        }
                        #endregion
                        node.Key.GetComponent<Image>().color = colHighlight;
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
            GetComponent<Image>().color = colUnhighlight;

            foreach (var node in Nodes)
            {
                foreach (var neighbour in neighbours)
                {
                    if (neighbourCache != null)
                    {
                        if (neighbourCache.Contains(gameObject))
                        {
                            //neighbourCache.Remove(gameObject);
                        }
                        foreach (var neighbourCached in neighbourCache)
                        {
                            if (node.Key == neighbourCached && Prestige.prestigePoints < node.Value.passiveCost)
                            {
                                node.Key.GetComponent<Image>().color = colUnhighlight;
                            }
                        }
                    }

                    if (node.Key == neighbour && !node.Value._isUnlocked && Prestige.prestigePoints >= node.Value.passiveCost)
                    {
                        node.Key.GetComponent<Image>().color = colHighlight;
                        neighbourCache.Add(node.Key);
                    }
                    #region Connections
                    foreach (var connection in connections)
                    {
                        foreach (var nodeConnect in node.Value.connections)
                        {
                            if (connection.objConnection == nodeConnect.objConnection)
                            {
                                if (!connectionCache.Contains(connection.objConnection))
                                {
                                    connection.objConnection.GetComponent<Image>().color = colHighlight;
                                    connectionCache.Add(connection.objConnection);
                                }
                                else
                                {
                                    connection.objConnection.GetComponent<Image>().color = colUnhighlight;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            Prestige.txtPoints.text = string.Format("Prestige Points: {0}", Prestige.prestigePoints);
            objExpand.SetActive(false);
        }
    }
}