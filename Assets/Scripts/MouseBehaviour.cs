using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseBehaviour : MonoBehaviour
{

    [SerializeField]
    LayerMask commandLayerMask = -1;
    [SerializeField]
    LayerMask buildingLayerMask = 0;
    public static MouseBehaviour mouseBehaviour;
    public GameObject hoveredObject;
    Unit selectedUnit;
    public GameObject selectedObject;
    public List<GameObject> selectedObjects = new List<GameObject>();
    public List<Unit> selectedUnits = new List<Unit>();
    public List<Building> selectedBuildings = new List<Building>();
    Ray ray;
    RaycastHit rayHit;
    bool isSelecting = false;
    Vector3 mousePosition1;
    Vector2 mousePosOnScreen, mousePos;

    BuildingPlacer placer;
    GameObject buildingPrefabToSpawn;


    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
    private void Awake()
    {
        mouseBehaviour = this;
    }
    private void Start()
    {
        placer = GameObject.FindObjectOfType<BuildingPlacer>();
        placer.gameObject.SetActive(false);
    }
    void Update()
    {
        UpdateRay();
        UpdateClicks();
        UpdatePlacer();

    }

    void UpdateClicks()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedObject = hoveredObject;
                isSelecting = true;
                mousePosition1 = Input.mousePosition;
                selectedUnit = hoveredObject.GetComponent<Unit>();
                TryBuild();
                if (Input.GetKey("left ctrl")) GroupControl();
            }

            if (Input.GetMouseButtonUp(0)) isSelecting = false;
            if (isSelecting)
            {
                if (!Input.GetKey("left ctrl")) UpdateSelecting();
            }
            if (Input.GetMouseButtonDown(1))
            {
                GiveCommands();
                buildingPrefabToSpawn = null;
            }
        }     

    }


    void UpdateSelecting()
    {
        selectedUnits.Clear();
        foreach (Unit unit in Unit.SelectableUnits)
        {
            if (!unit || !unit.IsAlive) continue; //to do ||isAlive
            bool isUnit = (selectedUnit == unit);
            bool inRect = IsWithinSelectionBounds(unit);
            (unit as ISelectable).SetSelected(inRect||isUnit);
            if (inRect || isUnit)
            {
                selectedUnits.Add(unit);
            }
        }
        selectedBuildings.Clear();
        foreach (Building building in Building.SelectableBuildings)
        {

            if (!building) continue; //to do ||isAlive
            bool isBuild = (selectedUnit == building);
            bool inRect = IsWithinSelectionBounds(building);
            (building as ISelectable).SetSelected(inRect || isBuild);
            if (inRect || isBuild)
            {
                selectedBuildings.Add(building);
            }
        }
    }

    void GroupControl()
    {
        foreach (Unit unit in Unit.SelectableUnits)
        {
            bool isUnit = (selectedUnit == unit);
            bool inRect = IsWithinSelectionBounds(unit);
            if (inRect || isUnit)
            {
                if (selectedUnits.Contains(unit))
                {
                    selectedUnits.Remove(unit);
                    (unit as ISelectable).SetSelected(false);
                }
                else
                {
                    selectedUnits.Add(unit);
                    (unit as ISelectable).SetSelected(true);
                }
            }
        }
    }


    


    void UpdatePlacer()
    {
        placer.gameObject.SetActive(buildingPrefabToSpawn);
        if (placer.gameObject.activeInHierarchy)
        {
            if (Physics.Raycast(ray, out rayHit, 1000, buildingLayerMask))
            {
                placer.SetPosition(rayHit.point);

            }
        }
    }

    void TryBuild()
    {
        if (buildingPrefabToSpawn && placer && 
            placer.isActiveAndEnabled && placer.CanBuildHere())
        {
            var buyable = buildingPrefabToSpawn.GetComponent<Buyable>();
            if (!buyable || !Money.TrySpendMoney(buyable.cost)) return;
            var building = Instantiate(buildingPrefabToSpawn, placer.transform.position, placer.transform.rotation);
            MoneyEarner.ShowMoneyText(transform.position, -(int)(buyable.cost));
        }
    }

    void GiveCommands()
    {
        if (Physics.Raycast(ray, out rayHit, 1000, commandLayerMask))
        {
            object commandData = null;//taki sposb poniewaz zostanie wyslany jako messa
            if (rayHit.collider is TerrainCollider)
            {
                //Debug.Log("Terrain: " + rayHit.point.ToString());
                commandData = rayHit.point;
            }

            else
            {
                //Debug.Log(rayHit.collider);
                commandData = rayHit.collider.gameObject.GetComponent<Unit>();
            }
            GiveCommands(commandData, "Command");
            
        }
    }

    void GiveCommands(object datacommand, string commandName)
    {
        foreach (Unit unit in selectedUnits)
            unit.SendMessage(commandName, datacommand, SendMessageOptions.DontRequireReceiver);
        foreach (Building building in selectedBuildings)
            building.SendMessage(commandName, datacommand, SendMessageOptions.DontRequireReceiver);

    }

    public static void SpawnUnits(GameObject prefab)
    {
        mouseBehaviour.GiveCommands(prefab, "Spawn");
    }
    public static void SpawnBuilding(GameObject prefab)
    {
        mouseBehaviour.buildingPrefabToSpawn = prefab;
    }


    public bool IsWithinSelectionBounds(Unit unit)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(
            camera.WorldToViewportPoint(unit.transform.position));
    }
    public bool IsWithinSelectionBounds(Building building)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(
            camera.WorldToViewportPoint(building.transform.position));
    }
    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void UpdateRay()
    {
        mousePos = Input.mousePosition;
        mousePosOnScreen = Camera.main.ScreenToViewportPoint(mousePos);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit))
        {
            GameObject hitObject = rayHit.transform.root.gameObject;
            HoverObject(hitObject);

        }
        else
        {
            ClearSelection();
        }
    }
    void HoverObject(GameObject obj)
    {
        if (hoveredObject != null)
        {
            if (obj == hoveredObject)
                return;
        }
        hoveredObject = obj;
    }
    void ClearSelection()
    {
        hoveredObject = null;
    }
}
