using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NodeDebug : CustomNode
{
    //TextField text = new TextField();

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


        //extensionContainer.Add(text);

        FullRefresh();
    }

    public static void Play()
    {
        Debug.Log("test");
    }

    /*public override List<object> GetParams()
    {
        List<object> tmp = new List<object>();
        tmp.Add(text.value);
        return tmp;
    }*/
}
