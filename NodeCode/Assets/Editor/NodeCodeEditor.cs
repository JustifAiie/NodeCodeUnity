using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

public class NodeCodeEditor : EditorWindow
{
    [MenuItem("NodeCode/NodeCodeEditor")]

    [OnOpenAsset()]
    public static bool OpenWindow(int instanceID, int line)
    {
        if (EditorUtility.InstanceIDToObject(instanceID) is NodeCode)
        {
            NodeCodeEditor wnd = GetWindow<NodeCodeEditor>();
            wnd.titleContent = new GUIContent("NodeCodeEditor");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeCodeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeCodeEditor.uss");
        root.styleSheets.Add(styleSheet);
    }
}