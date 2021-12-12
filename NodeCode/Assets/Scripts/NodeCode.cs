using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "NodeCode")]
public class NodeCode : ScriptableObject
{
    public List<CustomNode> Nodes = new List<CustomNode>();

    public List<NodeCodeData> NodeCodeData = new List<NodeCodeData>();
    public List<LinkData> LinkData = new List<LinkData>();

    public void Play()
    {
        foreach(CustomNode node in Nodes)
        {
            Debug.Log("inloop");
            node.Play();
        }
    }
}
