using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float panSpeed = 40f;
    public float panBoarderThickness = 10f;
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minZ = -1000f;
    public float maxZ = 1000f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBoarderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBoarderThickness)
        {
            Debug.Log("a");
            pos.x -= panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;

        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        this.transform.position = pos;

    }
}

