using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeCode : ScriptableObject
{
    public List<NodeCodeData> NodeCodeData = new List<NodeCodeData>();
    public List<LinkData> LinkData = new List<LinkData>();

    public void Play()
    {
        foreach(NodeCodeData node in NodeCodeData)
        {
            Debug.Log(node.title);
            
            node.PlayMethod();
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

    private void GetPlayMethod(List<Type> types, string typeString)
    {

    }
}
