using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeCode")]
public class NodeCode : ScriptableObject
{
    public List<CustomNode> Nodes = new List<CustomNode>();
    
    
}
