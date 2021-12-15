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
    public bool ExitPoint = false;

    public Port GeneratePort(Direction portDirection, Type type, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, portDirection, capacity, type);
    }

    public void FullRefresh()
    {
        RefreshExpandedState();
        RefreshPorts();
    }

    public void SetCanGoNext(bool state)
    {
        NodeCodeManager.Instance.nodeCode.canGoNext = state;
    }

    public virtual List<string> GetParams() 
    {
        return new List<string>();
    }

}
