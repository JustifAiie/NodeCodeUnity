using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class NodeCodeEditor : EditorWindow
{
    [MenuItem("NodeCode/NodeCodeEditor")]
    public static void OpenWindow()
    {
        NodeCodeEditor wnd = GetWindow<NodeCodeEditor>();
        wnd.titleContent = new GUIContent("NodeCodeEditor");
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