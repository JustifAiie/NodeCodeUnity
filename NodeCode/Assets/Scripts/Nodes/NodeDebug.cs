using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeDebug : CustomNode
{
    public NodeDebug()
    {
        title = "Debug";
        GUID = System.Guid.NewGuid().ToString();

        Port newPort = GeneratePort(Direction.Input, typeof(string));
        newPort.portName = "Input";
        inputContainer.Add(newPort);

        FullRefresh();
    }

}
