using System;
using UnityEngine;

//public class Card : MonoBehaviour
//{
//    private GameController gameController;

//    public enum Colors { Club, Diamond, Hearth, Spade }

//    public Colors Color { get; set; }
//    public int Rank { get; set; }


//    public Card(Colors color, int rank)
//    {
//        Color = color;
//        Rank = rank;
//    }   

//    public void AddCardToGame()
//    {
//        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
//        if (gameControllerObject != null)
//        {
//            gameController = gameControllerObject.GetComponent<GameController>();
//            gameController.AddCard(this);
//        }
//        if (gameController == null)
//        {
//            Debug.Log("Cannot find 'GameController' script");
//        }

//    }

//}

namespace Vuforia
{
    public class Card : MonoBehaviour
    {
        private GameControllerCard gameController;

        string Id;


        public Card(string id)
        {
            Id = id;
        }

        public void AddCardToGame()
        {
            GameObject gameControllerObject = GameObject.FindWithTag("GameControllerCard");
            if (gameControllerObject != null)
            {
                gameController = gameControllerObject.GetComponent<GameControllerCard>();

            }
            if (gameController == null)
            {
                Debug.Log("Cannot find 'GameControllerCard' script");
            }
            Debug.Log(gameController.ToString());
            gameController.AddCard(Id);
        }

    }
}