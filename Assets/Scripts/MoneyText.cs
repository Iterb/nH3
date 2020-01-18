using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{

    public int money;
    [SerializeField]
    Vector3 startOffset, endOffset;
    [SerializeField]
    float duration;
    [SerializeField]
    Color startColor, endColor;
    
    Vector3 startingPoint;
    Text text;
    float timer;
    private void Awake()
    {
        text = GetComponentInChildren<Text>(true);
        var canvas = GameObject.FindWithTag("World Canvas");
        if (canvas) transform.SetParent(canvas.transform);
        startingPoint = transform.position;
        timer = duration;
    }
    private void Update()
    {
        text.color = Color.white;
        timer -= Time.deltaTime;
        float percentDone = 1 - timer / duration;
        transform.position = Vector3.Lerp(
            startingPoint + startOffset,
            startingPoint + endOffset,
            percentDone);
        //text.color = Color.Lerp(startColor, startColor, percentDone);
        text.text = money + " $";
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
