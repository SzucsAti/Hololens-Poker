using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOpponents : MonoBehaviour, IInputClickHandler
{

    private GameControllerCard gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameControllerCard");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameControllerCard>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        gameController.UpdateOpponents(Int32.Parse(gameObject.name));
    }
}
