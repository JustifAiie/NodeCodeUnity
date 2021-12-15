using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeCodeData
{
    public string title;
    public string type;
    public string Guid;
    public Vector2 Position;
    public List<string> Parameters = new List<string>();
}
