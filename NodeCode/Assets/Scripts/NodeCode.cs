using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeCode")]
public class NodeCode : ScriptableObject
{
    public List<Node> Nodes = new List<Node>();
    
    
    public void CreateNode(Type type)
    {

    }
}
