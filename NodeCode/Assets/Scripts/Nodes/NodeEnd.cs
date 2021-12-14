using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeEnd : CustomNode
{
    public NodeEnd()
    {
        title = "End";
        GUID = System.Guid.NewGuid().ToString();
        ExitPoint = true;

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        FullRefresh();
    }
}
