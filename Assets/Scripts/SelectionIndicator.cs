using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    Unit unit;
    Transform cameraTransform;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        if (!unit)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.position = unit.transform.position + offset;
    }
}
