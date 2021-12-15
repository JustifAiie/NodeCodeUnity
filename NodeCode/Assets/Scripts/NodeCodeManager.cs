using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCodeManager : MonoBehaviour
{
    public static NodeCodeManager Instance;
    public NodeCode nodeCode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nodeCode.Play();
    }
}
