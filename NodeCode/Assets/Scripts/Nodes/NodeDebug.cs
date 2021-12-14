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

        Port newPort = GeneratePort(Direction.Input, typeof(string));
        newPort.portName = "Input";
        inputContainer.Add(newPort);

        
        extensionContainer.Add(text);

        FullRefresh();
    }

    public static void Play(List<object> parameters)
    {
        Debug.Log((string)parameters[0]);
    }

    public override List<object> GetParams()
    {
        List<object> tmp = new List<object>();
        tmp.Add(text.value);
        return tmp;
    }
}
