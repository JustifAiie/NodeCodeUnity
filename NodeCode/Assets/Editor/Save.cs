using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            nodeCodeData.title = node.title;
            nodeCodeData.Position = node.GetPosition().position;

            nodeCode.NodeCodeData.Add(nodeCodeData);
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

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
            CustomNode tmp = _target.CreateNode();
            tmp.GUID = data.Guid;
            _target.AddElement(tmp);


        }
    }

    private void ConnectNodes()
    {
        List<LinkData> dataList = new List<LinkData>();

        foreach (CustomNode node in Nodes)
        {
            foreach (LinkData data in _nodeCodeCache.LinkData)
            {
                if (data.BaseNodeGuid == node.GUID)
                    dataList.Add(data);
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                string targetGuid = dataList[i].TargetNodeGuid;
                CustomNode target = Nodes.First(x => x.GUID == targetGuid);

                if (node.outputContainer.childCount != 0)
                    LinkNodes(node.outputContainer[i].Q<Port>(), (Port)target.inputContainer[0]);

                target.SetPosition(new Rect(
                    _nodeCodeCache.NodeCodeData.First(x => x.Guid == targetGuid).Position,
                    _target.DefaultNodeSize
                ));
            }
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
}
