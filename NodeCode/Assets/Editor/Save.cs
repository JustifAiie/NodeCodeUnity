using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Save
{
    private NodeCodeView _target;
    private NodeCode _nodeCodeCache;

    private List<Edge> Edges => _target.edges.ToList();
    private List<CustomNode> Nodes => _target.nodes.ToList().Cast<CustomNode>().ToList();

    public static Save GetInstance(NodeCodeView target)
    {
        Save Instance = new Save
        {
            _target = target
        };

        return Instance;
    }

    public void SaveNodeCode(string fileName)
    {
        if (Edges.Count == 0)
            return;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        /*AssetDatabase.CreateFolder("Assets/Resources", $"{fileName}Dir");
        AssetDatabase.CreateFolder($"Assets/Resources/{fileName}Dir", "XML");*/

        NodeCode nodeCode = ScriptableObject.CreateInstance<NodeCode>();

        List<Edge> connectedPorts = Edges.Where(x => x.input.node != null).ToList();
        foreach (Edge edge in connectedPorts)
        {
            var outputNode = edge.output.node as CustomNode;
            var inputNode = edge.input.node as CustomNode;

            LinkData linkData = new LinkData();
            linkData.BaseNodeGuid = outputNode.GUID;
            linkData.PortName = edge.output.portName;
            linkData.TargetNodeGuid = inputNode.GUID;

            nodeCode.LinkData.Add(linkData);
        }

        foreach (CustomNode node in Nodes.Where(node => !node.EntryPoint))
        {
            NodeCodeData nodeCodeData = new NodeCodeData();
            nodeCodeData.Guid = node.GUID;
            nodeCodeData.type = node.GetType().FullName;
            nodeCodeData.title = node.title;
            nodeCodeData.Position = node.GetPosition().position;
            nodeCodeData.Parameters = node.GetParams();

            /*XmlSerializer xs = new XmlSerializer(typeof(NodeCodeData));
            TextWriter txtWriter = new StreamWriter($"Assets/Resources/{fileName}Dir/XML/{fileName}.xml");
            xs.Serialize(txtWriter, nodeCodeData);
            txtWriter.Close();*/

            nodeCode.NodeCodeData.Add(nodeCodeData);
        }

        AssetDatabase.CreateAsset(nodeCode, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadNodeCode(string fileName)
    {
        _nodeCodeCache = Resources.Load<NodeCode>(fileName);

        if (_nodeCodeCache == null)
        {
            EditorUtility.DisplayDialog("File not found !", $"Node Code file : {fileName} does not exist", "OK");
            return;
        }

        ClearGraph();
        GenerateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _nodeCodeCache.LinkData[0].BaseNodeGuid;

        foreach (CustomNode node in Nodes)
        {
            if (node.EntryPoint)
                continue;

            foreach (Edge edge in Edges)
            {
                if (edge.input.node == node)
                {
                    _target.RemoveElement(edge);
                }
            }

            _target.RemoveElement(node);
        }
    }

    private void GenerateNodes()
    {
        foreach (NodeCodeData data in _nodeCodeCache.NodeCodeData)
        {
            Type type = GetTypeFix(data.type);
            CustomNode tmp = _target.CreateNode(type, data.Parameters);
            tmp.GUID = data.Guid;
            _target.AddElement(tmp);
        }
    }

    private void ConnectNodes()
    {
        LinkData nodeData = new LinkData();

        foreach (CustomNode node in Nodes)
        {

            foreach (LinkData data in _nodeCodeCache.LinkData)
            {
                if (data.BaseNodeGuid == node.GUID)
                    nodeData = data;
            }

            string targetGuid = nodeData.TargetNodeGuid;
            CustomNode target = Nodes.First(x => x.GUID == targetGuid);

            if (node == target)
                break;

            if (node.outputContainer.childCount != 0)
            {            
                LinkNodes(node.outputContainer[0].Q<Port>(), (Port)target.inputContainer[0]);
            }

            target.SetPosition(new Rect(
                _nodeCodeCache.NodeCodeData.First(x => x.Guid == targetGuid).Position,
                _target.DefaultNodeSize
            ));
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        Edge tmp = new Edge
        {
            output = output,
            input = input
        };

        tmp?.input.Connect(tmp);
        tmp?.output.Connect(tmp);
        _target.Add(tmp);
    }

    public static Type GetTypeFix(string TypeName)
    {
        var type = Type.GetType(TypeName);

        if (type != null)
            return type;

        if (TypeName.Contains("."))
        {
            var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

            var assembly = Assembly.Load(assemblyName);
            if (assembly == null)
                return null;

            type = assembly.GetType(TypeName);
            if (type != null)
                return type;

        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if (assembly != null)
            {
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }
        }

        return null;
    }
}
