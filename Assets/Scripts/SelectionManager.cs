using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    List<GameObject> selectedObjects = new List<GameObject>();
    MouseBehaviour mm;
    // Start is called before the first frame update
    void Start()
    {
        mm = GameObject.FindObjectOfType<MouseBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
