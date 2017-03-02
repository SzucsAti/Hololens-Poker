using UnityEngine;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class AddPlayer : MonoBehaviour, IInputClickHandler{ 

    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        gameController.AddPlayer();
    }
}
