using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeCode : ScriptableObject
{
    public List<NodeCodeData> NodeCodeData = new List<NodeCodeData>();
    public List<LinkData> LinkData = new List<LinkData>();

    private Action _playAction = () => { };

    public void Play()
    {
        /*foreach(NodeCodeData node in NodeCodeData)
        {
            Debug.Log(node.title);
            SetPlayMethod(GetAllNodeTypes(), node.title, node.Parameters);
            _playAction();
        }*/
        
        var info = new DirectoryInfo($"Assets/Resources/{name}Dir/XML");
        foreach (var file in info.GetFiles("*xml"))
        {
            NodeCodeData.Add((NodeCodeData)Deserialize($"Assets/Resources/{name}Dir/XML/{file.Name}"));
        }

        Debug.Log(NodeCodeData[0].Parameters[0].ToString().GetType());
    }

    private List<Type> GetAllNodeTypes()
    {
        List<Type> nodeTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where assemblyType.IsSubclassOf(typeof(CustomNode))
                                select assemblyType).ToList();

        return nodeTypes;
    }

    private void SetPlayMethod(List<Type> types, string typeString, List<object> parameters)
    {
        foreach (Type type in types)
        {
            if (type.ToString().Contains(typeString))
            {
                MethodInfo methodInfo = type.GetMethod("Play"); 
                if (methodInfo != null)
                {
                    _playAction = (Action)Delegate.CreateDelegate(typeof(Action), parameters, methodInfo);
                }
            }
        }
    }

    private NodeCodeData Deserialize(string fileName)
    {
        XmlSerializer xs = new XmlSerializer(typeof(NodeCodeData));

        NodeCodeData data;

        using (Stream reader = new FileStream(fileName, FileMode.Open))
        {
            data = (NodeCodeData)xs.Deserialize(reader);
        }

        return data;
    }
}
