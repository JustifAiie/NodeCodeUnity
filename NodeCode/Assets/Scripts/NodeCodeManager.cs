using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCodeManager : MonoBehaviour
{
    public NodeCode nodeCode;

    private void Start()
    {
        nodeCode.Play();
    }
}
