using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCL 
{
public float movementSpeed;
public float attack;
public float defense;
public float maxDmg;
public float minDmg;
public float maxHP;
public float currentHP;
public float cost;
public float creationSpeed;
public string city;




    public UnitCL()
    {
    }

    public UnitCL(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;       
    }

    public UnitCL(float movementSpeed, float attack, float defense, float maxDmg,
        float minDmg, float maxHP, float currentHP, float cost, float creationSpeed, string city)
    {
        this.movementSpeed = movementSpeed;
        this.attack = attack;
        this.defense = defense;
        this.maxDmg = maxDmg;
        this.minDmg = minDmg;
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.cost = cost;
        this.creationSpeed = creationSpeed;
        this.city = city;
    }
}
