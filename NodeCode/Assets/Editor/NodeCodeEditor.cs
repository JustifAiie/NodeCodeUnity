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

    private string _fileName;
    
    [MenuItem("Node Code/Editor")]
    public static void OpenWindow()
    {
        NodeCodeEditor wnd = GetWindow<NodeCodeEditor>();
        wnd.Show();
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

        CodeView = root.Q<NodeCodeView>();

        var fileNameText = root.Q<TextField>("FileName");
        fileNameText.MarkDirtyRepaint();
        fileNameText.RegisterValueChangedCallback(evt => _fileName = evt.newValue);

        var saveButton = root.Q<Button>("SaveButton");
        saveButton.clicked += SaveData;
        var loadButton = root.Q<Button>("LoadButton");
        loadButton.clicked += LoadData;
    }   

    private void SaveData()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Empty file name !", "Enter a valid file name", "OK");
            return;
        }

        var save = Save.GetInstance(CodeView);
        save.SaveNodeCode(_fileName);
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Empty file name !", "Enter a valid file name", "OK");
            return;
        }

        var load = Save.GetInstance(CodeView);
        load.LoadNodeCode(_fileName);
    }
}