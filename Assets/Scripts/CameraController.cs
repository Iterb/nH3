using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    public float cameraSpeed, zoomSpeed, grondHeight;
    public Vector2 cameraHeightMinMax;
    public Vector2 cameraRotationMinMax;
    [Range(0, 1)]//atrybut ograniczający poniższe pole (tylko w inspektorze)
    public float zoomLerp = .1f;
    [Range(0, 0.2f)]
    public float cursorTreshold;

    RectTransform selectionBox; //wariant Transforma używany na UI
    new Camera camera;
    Vector2 mousePos, mousePosOnScreen, keyboardInput, mouseScroll;
    bool isCursorInGameScreen;
    Rect selectionRect, boxRect;

    private void Awake()
    {
        selectionBox = GetComponentInChildren<Image>(true).transform as RectTransform;//znajdzie obiekt zawierający pierwszy image 
        //przeszukając całą hierarchię dzieci i w tym obiekcie szuka RectTransform     (true) include inactive
        camera = GetComponent<Camera>();

    }
    private void Update()
    {
        UpdateMovement();
        UpdateZoom();
        //UpdateClicks();
    }
    
    void UpdateMovement()
    {
        keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mousePos = Input.mousePosition; //pozycja kursora w pikslach
        mousePosOnScreen = camera.ScreenToViewportPoint(mousePos); //zwraca Vector2 mówiący o tym w jakim procencie
        //na ekranie sie znajdujemy. Zakres od 0 do 1. Poza ekranem wartosci przekraczaja 1 lub sa mniejsze od 0
        isCursorInGameScreen = mousePosOnScreen.x >= 0 && mousePosOnScreen.x <= 1 && 
            mousePosOnScreen.y >= 0 && mousePosOnScreen.y <= 1;
        Vector2 movementDirection = keyboardInput;

        if (isCursorInGameScreen)
        {
            if (mousePosOnScreen.x < cursorTreshold) movementDirection.x -= 1 - mousePosOnScreen.x / cursorTreshold;
            if (mousePosOnScreen.x > 1 - cursorTreshold) movementDirection.x += 1 - (1 - mousePosOnScreen.x) / cursorTreshold;
            if (mousePosOnScreen.y < cursorTreshold) movementDirection.y -= 1 - mousePosOnScreen.y / cursorTreshold;
            if (mousePosOnScreen.y >1 - cursorTreshold) movementDirection.y += 1 - (1 - mousePosOnScreen.y) / cursorTreshold;
            
           
        }
        var deltaPosition = new Vector3(movementDirection.x, 0, movementDirection.y);
        deltaPosition *= cameraSpeed * Time.deltaTime; //deltaTime = czas jaki trwała ostatnia klatka w sekundach
        transform.position += deltaPosition;
    }
    void UpdateZoom()
    {
        mouseScroll = Input.mouseScrollDelta;
        float zoomDelta = mouseScroll.y * zoomSpeed * Time.deltaTime;
        zoomLerp = Mathf.Clamp01(zoomLerp + zoomDelta);//ograniczenie wyniku miedzy 01

        var position = transform.localPosition;
        position.y = Mathf.Lerp(cameraHeightMinMax.y, cameraHeightMinMax.x, zoomLerp)
            + grondHeight; //interpolacja linowa
        transform.localPosition = position; //update pozycji


        var rotation = transform.localEulerAngles;
        rotation.x = Mathf.Lerp(cameraRotationMinMax.y, cameraRotationMinMax.x, zoomLerp);
        transform.localEulerAngles = rotation; //update rotacji
    }

    void UpdateClicks()
    {

        if (Input.GetMouseButtonDown(0))
        {
            selectionRect.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {

        }
        if (Input.GetMouseButton(0))
        {
            selectionRect.size = mousePos - selectionRect.position;
            boxRect = AbsRect(selectionRect);
            selectionBox.anchoredPosition = boxRect.position; // anchoredPosition ważne przy UI
            selectionBox.sizeDelta = selectionRect.size;
        }

        
    }

    Rect AbsRect(Rect rect)
    {
        if (rect.width < 0)
        {
            rect.x += rect.width;
            rect.width *= -1;
        }

        if (rect.height < 0)
        {
            rect.y += rect.height;
            rect.height *= -1;
        }
        return rect;
    }
}
