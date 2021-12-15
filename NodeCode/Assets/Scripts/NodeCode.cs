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

    [HideInInspector] public bool canGoNext = true;

    private delegate void _playDelegate(List<string> parameters);
    private _playDelegate _playMethod;

    public void Play()
    {
        NodeCodeManager.Instance.StartCoroutine(WaitForNodeCoroutine());

        /*var info = new DirectoryInfo($"Assets/Resources/{name}Dir/XML");
        foreach (var file in info.GetFiles("*xml"))
        {
            NodeCodeData.Add(Deserialize($"Assets/Resources/{name}Dir/XML/{file.Name}"));
        }*/
    }

    private IEnumerator WaitForNodeCoroutine()
    {
        foreach (NodeCodeData node in NodeCodeData)
        {
            canGoNext = false;
            SetPlayMethod(GetAllNodeTypes(), node.title);
            _playMethod(node.Parameters);
            yield return new WaitUntil(() => canGoNext);
        }
    }

    private List<Type> GetAllNodeTypes()
    {
        List<Type> nodeTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where assemblyType.IsSubclassOf(typeof(CustomNode))
                                select assemblyType).ToList();

        return nodeTypes;
    }

    private void SetPlayMethod(List<Type> types, string typeString)
    {
        foreach (Type type in types)
        {
            if (type.ToString().Contains(typeString))
            {
                MethodInfo methodInfo = type.GetMethod("Play");            
                if (methodInfo != null)
                {
                    _playMethod = (_playDelegate)Delegate.CreateDelegate(typeof(_playDelegate), Activator.CreateInstance(type), methodInfo);
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
