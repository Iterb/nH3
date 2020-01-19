using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static List<ISelectable> SelectableBuildings { get { return selectableBuildings; } }
    static List<ISelectable> selectableBuildings = new List<ISelectable>();
    [SerializeField]
    float hp, hpMax = 100;
    [SerializeField]
    GameObject hpBarPrefab;
    [SerializeField]
    GameObject selectionIndicatorPrefab;
    [SerializeField]
    protected Transform spawnPoint, flag;
    protected HealthBar healthBar;
    protected SelectionIndicator selectionIndicator;

    protected virtual void Awake()
    {
        hp = hpMax;
        healthBar = Instantiate(hpBarPrefab, transform).GetComponent<HealthBar>();
        selectionIndicator = Instantiate(selectionIndicatorPrefab, transform).GetComponent<SelectionIndicator>();
    }
    protected virtual void Start()
    {
        if (this is ISelectable)
        {
            selectableBuildings.Add(this as ISelectable);
            (this as ISelectable).SetSelected(false);
        }
    }
    protected virtual void Update()
    {
        CalculateCreationDelay();
    }

    protected virtual void OnDestroy()
    {
        if (this is ISelectable) selectableBuildings.Remove(this as ISelectable);
    }

    protected virtual void CalculateCreationDelay()
    {

    }




}
