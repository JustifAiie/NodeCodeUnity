using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;

public class NodeCodeEditor : EditorWindow
{
    public static NodeCodeEditor Instance;

    public Button CreateButton;
    public NodeCodeView CodeView;

    static NodeCode currentNC;
    
    [OnOpenAsset()]
    public static bool OpenWindowFromAsset(int instanceID, int line)
    {
        Object opened = EditorUtility.InstanceIDToObject(instanceID);
        if (opened is NodeCode)
        {
            NodeCodeEditor wnd = GetWindow<NodeCodeEditor>();
            wnd.titleContent = new GUIContent("NodeCodeEditor - " + opened.name);

            currentNC = opened as NodeCode;

            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        Instance = this;

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeCodeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeCodeEditor.uss");
        root.styleSheets.Add(styleSheet);

        /*CreateButton = root.Q<Button>("createButton");
        CreateButton.clicked += CreateButtonPressed;*/

        CodeView = root.Q<NodeCodeView>();

        CodeView.graphViewChanged = OnGraphChange;

        /*foreach (Node node in currentNC.Nodes)
        {
            RefreshNodeAfterClose(node.GetPosition());
        }*/
    }   

    public void CreateNode()
    {
        CustomNode newNode = new NodeDebug();
        
        currentNC.Nodes.Add(newNode);
        Debug.Log(currentNC.Nodes.Count);
        

        CodeView.AddElement(newNode);

    }

    /*public void RefreshNodeAfterClose(Rect position)
    {
        Node newNode = new NodeInstantiate();
        newNode.SetPosition(position);

        Debug.Log(currentNC.Nodes.Count);


        CodeView.AddElement(newNode);
    }*/

    private GraphViewChange OnGraphChange(GraphViewChange change)
    {
        if (change.elementsToRemove != null)
        {
            foreach (var item in change.elementsToRemove)
            {
                CustomNode node = item as CustomNode;
                if (node != null)
                {
                    currentNC.Nodes.Remove(node);
                    Debug.Log(currentNC.Nodes.Count);
                }
            }
        }

        return change;
    }
}