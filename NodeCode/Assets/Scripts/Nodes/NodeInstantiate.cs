using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeInstantiate : CustomNode
{
    public NodeInstantiate()
    {
        title = "Instantiate";
        GUID = System.Guid.NewGuid().ToString();
    }
}
