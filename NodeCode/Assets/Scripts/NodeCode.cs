using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeCode : ScriptableObject
{
    public List<NodeCodeData> NodeCodeData = new List<NodeCodeData>();
    public List<LinkData> LinkData = new List<LinkData>();

    private Action _playAction = () => { };

    public void Play()
    {
        foreach(NodeCodeData node in NodeCodeData)
        {
            Debug.Log(node.title);
            SetPlayMethod(GetAllNodeTypes(), node.title);
            _playAction();
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
                    _playAction = (Action)Delegate.CreateDelegate(typeof(Action), methodInfo);
                }
            }
        }
    }
}
