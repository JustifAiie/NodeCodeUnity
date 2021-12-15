using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class NodeInstantiate : CustomNode
{
    ObjectField prefab = new ObjectField();
    Vector3Field position = new Vector3Field();
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

        prefab.objectType = typeof(GameObject);
        extensionContainer.Add(prefab);
        extensionContainer.Add(position);

        FullRefresh();
    }

    public NodeInstantiate(List<string> parameters)
    {
        title = "Instantiate";
        GUID = System.Guid.NewGuid().ToString();

        Port newInputPort = GeneratePort(Direction.Input, typeof(string));
        newInputPort.portName = "Input";
        inputContainer.Add(newInputPort);

        Port newOutputPort = GeneratePort(Direction.Output, typeof(string));
        newOutputPort.portName = "Output";
        outputContainer.Add(newOutputPort);

        prefab.value = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/{parameters[0]}.prefab", typeof(GameObject));
        prefab.objectType = typeof(GameObject);
        extensionContainer.Add(prefab);

        position.value = new Vector3(float.Parse(parameters[1]), float.Parse(parameters[2]), float.Parse(parameters[3]));
        extensionContainer.Add(position);

        FullRefresh();
    }

    public void Play(List<string> parameters)
    {
        GameObject.Instantiate(AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/{parameters[0]}.prefab", typeof(GameObject)),
                               new Vector3(float.Parse(parameters[1]), float.Parse(parameters[2]), float.Parse(parameters[3])),
                               Quaternion.identity);
        SetCanGoNext(true);
    }

    public override List<string> GetParams()
    {
        List<string> tmp = new List<string>();
        tmp.Add(prefab.value.name);
        tmp.Add(position.value.x.ToString());
        tmp.Add(position.value.y.ToString());
        tmp.Add(position.value.z.ToString());
        return tmp;
    }
}
