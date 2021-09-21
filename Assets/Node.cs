using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : NodeClass
{

    private NodeClass _nodeClass;

    void Awake()
    {
        _nodeClass = GetComponent<NodeClass>();
        Nodes.Add(gameObject, _nodeClass);

        InitializeObjects();
    }
}
