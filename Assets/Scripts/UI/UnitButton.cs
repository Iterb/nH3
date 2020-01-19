using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitButton : MonoBehaviour
{
    [SerializeField]
    protected GameObject spawnPrefab;

    Button button;
    Image image;
    Text[] texts;
    Text costText;
    Text spawnTimerText;
    private void Awake()
    {
        texts = GetComponentsInChildren<Text>(true);
        costText = texts[0];
        spawnTimerText = texts[1];
        button = GetComponentInChildren<Button>(true);
        image = GameObject.FindGameObjectWithTag("Image").GetComponent<Image>();
        Buyable buyable;
        if (spawnPrefab && (buyable = spawnPrefab.GetComponent<Buyable>()))
        {
            image.sprite = buyable.icon;
        }

    }
    
    private void Update()
    {
        Buyable buyable;
        Tent tent;
        tent = FindObjectOfType<Tent>();
        spawnTimerText.enabled =  tent.spawnTimer > 0 ? true : false;
        spawnTimerText.text = Math.Round(tent.spawnTimer, 2).ToString();
        if (spawnPrefab && (buyable = spawnPrefab.GetComponent<Buyable>()))
        {
            costText.text = buyable.cost + "$";
            button.interactable = Money.HaveEnoughMoney(buyable.cost);
        }
    }
    public virtual void SpawnUnit()
    {
        MouseBehaviour.SpawnUnits(spawnPrefab);
    }
}
