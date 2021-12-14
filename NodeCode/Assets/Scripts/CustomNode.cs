using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class CustomNode : Node
{
    public string GUID;

    public bool EntryPoint = false;

    public Port GeneratePort(Direction portDirection, Type type, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, portDirection, capacity, type);
    }

    public void FullRefresh()
    {
        RefreshExpandedState();
        RefreshPorts();
    }

    public virtual List<object> GetParams() 
    {
        return new List<object>();
    }

}
