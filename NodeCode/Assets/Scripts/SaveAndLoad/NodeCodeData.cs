using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NodeCodeData
{
    public delegate void PlayDelegate();
    public PlayDelegate PlayMethod;

    public string title;
    public string Guid;
    public Vector2 Position;
}
