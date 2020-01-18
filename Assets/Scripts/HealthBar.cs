using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    const string WORLD_CANVAS = "World Canvas";
    [SerializeField]
    Vector3 offset;

    Slider slider;
    Unit unit;
    Transform parent;
    Transform cameraTransform;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        parent = transform.parent;
        unit = GetComponentInParent<Unit>();
        cameraTransform = Camera.main.transform;
        var canvas = GameObject.FindWithTag(WORLD_CANVAS);
        if (canvas) transform.SetParent(canvas.transform); //zmiana rodzica, ponieważ elementy UI są widoczne tylko na canvasie

    }
    private void Update()
    {
        if (!parent)
        {
            Destroy(this.gameObject);
            return;
        }
        if (unit) slider.value = unit.HealthPercent;
        transform.position = parent.transform.position + offset;
    }
}
