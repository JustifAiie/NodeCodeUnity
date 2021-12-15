using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeWait : CustomNode
{
    IntegerField seconds = new IntegerField();

    public NodeWait()
    {
        title = "Wait";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        extensionContainer.Add(seconds);

        FullRefresh();
    }

    public NodeWait(List<string> parameters)
    {
        title = "Wait";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        seconds.value = Int32.Parse(parameters[0]);

        extensionContainer.Add(seconds);

        FullRefresh();
    }

    public void Play(List<string> parameters)
    {
        NodeCodeManager.Instance.StartCoroutine(WaitCoroutine(Int32.Parse(parameters[0])));
    }

    private IEnumerator WaitCoroutine(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetCanGoNext(true);
    }

    public override List<string> GetParams()
    {
        List<string> tmp = new List<string>();
        tmp.Add(seconds.value.ToString());
        return tmp;
    }
}
