using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NodeDebug : CustomNode
{
    TextField text = new TextField();

    public NodeDebug()
    {
        title = "Debug";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        extensionContainer.Add(text);

        FullRefresh();
    }

    public NodeDebug(List<string> parameters)
    {
        title = "Debug";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        text.value = parameters[0];

        extensionContainer.Add(text);

        FullRefresh();
    }

    public void Play(List<string> parameters)
    {
        Debug.Log(parameters[0]);
        SetCanGoNext(true);
    }

    public override List<string> GetParams()
    {
        List<string> tmp = new List<string>();
        tmp.Add(text.value);
        return tmp;
    }
}
