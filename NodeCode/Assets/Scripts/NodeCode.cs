using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeCode : ScriptableObject
{
    public List<NodeCodeData> NodeCodeData = new List<NodeCodeData>();
    public List<LinkData> LinkData = new List<LinkData>();

    public void Play()
    {
        foreach(NodeCodeData node in NodeCodeData)
        {
            Debug.Log(node.title);
            
            node.PlayMethod();
        }
    }
}
