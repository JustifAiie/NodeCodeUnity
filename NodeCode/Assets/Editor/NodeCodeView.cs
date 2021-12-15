using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeCodeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<NodeCodeView, UxmlTraits> { }

    public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

    private Vector2 _mousePos = new Vector2();

    public NodeCodeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeCodeEditor.uss");
        styleSheets.Add(styleSheet);

        AddElement(GenerateStartNode());
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        List<Type> typeList = GetAllNodeTypes();

        if (evt.target is GraphView)
        {
            _mousePos = evt.localMousePosition;
            foreach (Type type in typeList)
            {
                evt.menu.AppendAction(type.Name.Substring(4), (e) => { CreateNode(type); });
            }
        }
        else
        {
            base.BuildContextualMenu(evt);
        }
    }

    public CustomNode CreateNode(Type type, List<string> parameters = null)
    {
        CustomNode newNode;
        if (parameters == null)
            newNode = (CustomNode)Activator.CreateInstance(type);
        else
            newNode = (CustomNode)Activator.CreateInstance(type, parameters);

        newNode.SetPosition(new Rect(
            _mousePos,
            DefaultNodeSize
        ));

        AddElement(newNode);

        return newNode;
    }

    private CustomNode GenerateStartNode()
    {
        CustomNode startNode = new CustomNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            EntryPoint = true,
        };

        Port newPort = startNode.GeneratePort(Direction.Output, typeof(string));
        newPort.portName = "Next";
        startNode.outputContainer.Add(newPort);

        startNode.FullRefresh();

        return startNode;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node && startPort.portType == port.portType)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    private List<Type> GetAllNodeTypes()
    {
        List<Type> nodeTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where assemblyType.IsSubclassOf(typeof(CustomNode))
                                select assemblyType).ToList();

        return nodeTypes;
    }
}
