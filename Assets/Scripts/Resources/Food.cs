using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Resource
{
    private Resource _resource;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
    }

    private void Start()
    {
        Debug.Log(_resource);
    }
}
