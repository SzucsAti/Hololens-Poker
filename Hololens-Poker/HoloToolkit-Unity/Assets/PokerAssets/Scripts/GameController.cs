using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public TextMesh numberOfPlayersText;
    public int numberOfPlayer;

    void Start()
    {
        numberOfPlayer = 2;
        numberOfPlayersText.text = numberOfPlayer.ToString();
    }

    public void AddPlayer()
    {
        if (numberOfPlayer != 12)
        {
            numberOfPlayer++;
            UpdatePlayer();
        }
    }

    public void RemovePlayer()
    {
        if (numberOfPlayer != 2)
        {
            numberOfPlayer--;
            UpdatePlayer();
        }
    }

    void UpdatePlayer()
    {
        numberOfPlayersText.text = numberOfPlayer.ToString();
    }

}

