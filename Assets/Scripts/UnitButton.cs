using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject spawnPrefab;

    Button button;
    Image image;
    Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>(true);
        button = GetComponentInChildren<Button>(true);
        image = GameObject.FindGameObjectWithTag("Image").GetComponent<Image>();
        Buyable buyable;
        if (spawnPrefab && (buyable = spawnPrefab.GetComponent<Buyable>()))
        {
            Debug.Log(buyable.ToString());
            image.sprite = buyable.icon;
        }

    }

    private void Update()
    {
        Buyable buyable;
        if (spawnPrefab && (buyable = spawnPrefab.GetComponent<Buyable>()))
        {
            text.text = buyable.cost + "$";
            button.interactable = Money.HaveEnoughMoney(buyable.cost);
        }
    }
    public virtual void SpawnUnit()
    {
        MouseBehaviour.SpawnUnits(spawnPrefab);
    }
}
