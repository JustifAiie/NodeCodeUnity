using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using System.IO;

public class NodeCodeEditor : EditorWindow
{
    public static NodeCodeEditor Window;

    public NodeCodeView CodeView;

    private string _fileName;
    private NodeCodeManager _nodeCodeManager;
    
    [MenuItem("Node Code/Editor")]
    public static void OpenWindow()
    {
        Window = GetWindow<NodeCodeEditor>();
        Window.Show();
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeCodeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeCodeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _nodeCodeManager = GameObject.Find("NodeCodeManager").GetComponent<NodeCodeManager>();

        CodeView = root.Q<NodeCodeView>();

        var fileNameText = root.Q<TextField>("FileName");
        fileNameText.MarkDirtyRepaint();
        fileNameText.RegisterValueChangedCallback(evt => _fileName = evt.newValue);

        var saveButton = root.Q<Button>("SaveButton");
        saveButton.clicked += SaveData;
        var loadButton = root.Q<Button>("LoadButton");
        loadButton.clicked += LoadData;
        var applyButton = root.Q<Button>("ApplyButton");
        applyButton.clicked += ApplyNodeCode;
        var playButton = root.Q<Button>("PlayButton");
        playButton.clicked += PlayFromEditor;
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

    private void ApplyNodeCode()
    {
        NodeCode toApply = (NodeCode)AssetDatabase.LoadAssetAtPath($"Assets/Resources/{_fileName}.asset", typeof(NodeCode));

        if (toApply == null)
            EditorUtility.DisplayDialog("Wrong file name !", "Could not find a NodeCode matching the file name", "OK");
        else
            _nodeCodeManager.nodeCode = toApply;
    }

    private void PlayFromEditor()
    {
        if (_nodeCodeManager.nodeCode == null)
            EditorUtility.DisplayDialog("No NodeCode specified !", "Can't enter Play mode because no NodeCode has been specified in NodeCodeManager", "OK");
        else
            EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}