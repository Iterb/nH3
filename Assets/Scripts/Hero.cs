using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit, ISelectable
{
    public void SetSelected(bool selected)
    {
        healthBar.gameObject.SetActive(selected);
        selectionIndicator.gameObject.SetActive(selected);
    }
}
