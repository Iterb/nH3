using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static List<Solider> SoliderList { get { return gameController.soliderList; } }
    public static List<Dragon> DragonList { get { return gameController.dragonList; } }

    static GameController gameController;
    List<Solider> soliderList = new List<Solider>(); 
    List<Dragon> dragonList = new List<Dragon>();

    [SerializeField]
    GameObject middlePanel, upperPanel, bottomPanel;
    [SerializeField]
    MouseBehaviour mouseBehaviour;

    Text text;
    private void Awake()
    {
        gameController = this;
        text = middlePanel.GetComponentInChildren<Text>(true);
    }

    private void Update()
    {
        TidyList(soliderList);
        TidyList(dragonList);

        if (soliderList.Count <= 0) Lose();
        else if (dragonList.Count <= 0) Win();
    }

    void TidyList<T>(List<T> list) where T: Unit
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null || !list[i].IsAlive)
            {
                list.RemoveAt(i--);
            }
        }
    }

    void Lose()
    {
        EndGame();
        text.text = "You lose!";
    
    }

    void Win()
    {
        EndGame();
        text.text = "You win!";
    }

    void EndGame()
    {
        enabled = false;
        mouseBehaviour.enabled = enabled = false;
        middlePanel.SetActive(true);
        upperPanel.SetActive(false);
        bottomPanel.SetActive(false);
    }
}
