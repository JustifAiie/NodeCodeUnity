using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeInstantiate : CustomNode
{
    public NodeInstantiate()
    {
        title = "Instantiate";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        FullRefresh();
    }

    public static void Play()
    {
        GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }
}
