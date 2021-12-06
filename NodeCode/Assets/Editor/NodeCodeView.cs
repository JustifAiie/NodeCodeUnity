using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeCodeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<NodeCodeView, UxmlTraits> { }

    public NodeCodeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        /*this.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
        {
            Debug.Log("Right click !");
        }));*/

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeCodeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {   

        if (evt.target is GraphView)
        {
            evt.menu.AppendAction("Create", (e) => { NodeCodeEditor.Instance.CreateNode(); });
        }
        else
        {
            base.BuildContextualMenu(evt);
        }
    }
}
