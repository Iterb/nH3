using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehaviour : MonoBehaviour
{
    GameObject selectionBox;
    GameObject unitModel;
    private Vector3 modelPosition;
    private Vector3 offset = new Vector3(0f, -1f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        unitModel = this.transform.Find("Model").gameObject;
        selectionBox = this.transform.Find("Selection_Indicator").gameObject.transform.Find("Quad").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        selectionBox.transform.position = unitModel.transform.TransformPoint(offset);
        //selectionBox.transform.rotation = unitModel.transform.rotation;
        //selectionBox.transform.LookAt(unitModel.transform);
    }
}
