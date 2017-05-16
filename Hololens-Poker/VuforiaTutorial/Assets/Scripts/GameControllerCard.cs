using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldemHand;
using System.Threading;
using System.Diagnostics;
using System.Linq;

public class GameControllerCard : MonoBehaviour
{

    public bool preFlop;

    public TextMesh WinRateText;
    public TextMesh HandText;
    public TextMesh BoardText;
    public TextMesh OpponentsText;

    public TextMesh StrategyLabelRaises;
    public TextMesh StrategyRow1;
    public TextMesh StrategyRow2;
    public TextMesh StrategyRow3;
    public TextMesh StrategyRow4;
    public TextMesh StrategyRow5;

    List<string> deck;
    string[] cards = { "Ac","2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc",
                       "Ad","2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd",
                       "Ah","2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh",
                       "As","2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks"};

    List<string> hand;
    List<string> boardList;
    public int opponents;

    // Use this for initialization
    void Start()
    {
        UnityEngine.Debug.Log("GameControllerCard Started");
        NewRound();
    }

    void Awake() {
        UnityEngine.Debug.Log("GameControllerCard Awake");
    }

    public void UpdateOpponents(int o)
    {
        opponents = o;
        OpponentsText.text = o.ToString();

        if (hand.Count == 2) {
            Calculate();
        }
    }

    public void Calculate()
    {
        UnityEngine.Debug.Log("Calculate Called");

        const double time = 4.0;
        ulong dead = Hand.ParseHand("");

        string handString = "";
        foreach (string c in hand)
        {
            handString += c + " ";
        }

        handString.Remove(handString.Length - 1);

        ulong pocket = Hand.ParseHand(handString);
        ulong board = Hand.ParseHand("");

        if (boardList.Count > 0) {
            string boardString = "";
            foreach (string c in boardList)
            {
                boardString += c + " ";
            }

            boardString.Remove(boardString.Length - 1);
            board = Hand.ParseHand(boardString);
        }

        double winrate = WinOddsMonteCarlo(pocket, board, dead, opponents, time) * 100.0;
        RecommendStrategy(winrate);
        UpdateWinRate(winrate);
    }

    public void RecommendStrategy(double winrate)
    {
        UnityEngine.Debug.Log("RecommendStrategy Called");
        UnityEngine.Debug.Log("preflop?" + preFlop);

        if (preFlop)
        {
            UnityEngine.Debug.Log("RecommendStrategy preflop");

            hand.Sort();
            StrategyLabelRaises.text = "Raises before you            Strategy";
            // AA, KK, QQ
            if ((hand[0].StartsWith("A") && hand[1].StartsWith("A"))
                || (hand[0].StartsWith("K") && hand[1].StartsWith("K"))
                || (hand[0].StartsWith("Q") && hand[1].StartsWith("Q")))
            {
                StrategyRow1.text = "None                                Raise";
                StrategyRow2.text = "Exactly one                       Raise";
                StrategyRow3.text = "More than one                   All-In";
                StrategyRow4.text = "";
                StrategyRow5.text = "";
            }
            // AK, JJ, TT
            else if ((hand[0].StartsWith("A") && hand[1].StartsWith("K"))
                || (hand[0].StartsWith("T") && hand[1].StartsWith("T"))
                || (hand[0].StartsWith("J") && hand[1].StartsWith("J")))
            {
                StrategyRow1.text = "None                                Raise";
                StrategyRow2.text = "One, from early pos.          Call";
                StrategyRow3.text = "One, from mid pos.            Raise";
                StrategyRow4.text = "More than one                   Fold";
                StrategyRow5.text = "(!) Raises after you            Fold";
            }
            // AQ, AJ, KQ
            else if ((hand[0].StartsWith("A") && hand[1].StartsWith("Q"))
                || (hand[0].StartsWith("A") && hand[1].StartsWith("J"))
                || (hand[0].StartsWith("K") && hand[1].StartsWith("Q")))
            {
                StrategyRow1.text = "None                                Raise";
                StrategyRow2.text = "One, from early pos.          Fold";
                StrategyRow3.text = "One, from mid pos.            Call";
                StrategyRow4.text = "More than one                   Fold";
                StrategyRow5.text = "(!) Raises after you            Fold";
            }
            // QJs-65s
            else if ((hand[0].StartsWith("J") && hand[1].StartsWith("Q"))
                || (hand[0].StartsWith("J") && hand[1].StartsWith("T"))
                || (hand[0].StartsWith("9") && hand[1].StartsWith("T"))
                || (hand[0].StartsWith("8") && hand[1].StartsWith("9"))
                || (hand[0].StartsWith("7") && hand[1].StartsWith("8"))
                || (hand[0].StartsWith("6") && hand[1].StartsWith("7"))
                || (hand[0].StartsWith("5") && hand[1].StartsWith("6")))
            {
                StrategyRow1.text = "None                                Raise";
                StrategyRow2.text = "One, from early pos.          Fold";
                StrategyRow3.text = "One, from mid pos.            Fold";
                StrategyRow4.text = "More than one                   Call";
                StrategyRow5.text = "(!) Raises after you            Fold";
            }
            // 99-22
            else if (hand[0][0].Equals(hand[1][0]) && (hand[0][0] >= 2 || hand[0][0] <= 9))
            {
                StrategyRow1.text = "None                                Raise";
                StrategyRow2.text = "One, from early pos.          Call";
                StrategyRow3.text = "One, from mid pos.            Call";
                StrategyRow4.text = "More than one                   Fold";
                StrategyRow5.text = "(!) Raises after you            Fold";
            }
            else
            {
                StrategyLabelRaises.text = "            Fold";
                StrategyRow1.text = " ";
                StrategyRow2.text = " ";
                StrategyRow3.text = " ";
                StrategyRow4.text = " ";
                StrategyRow5.text = " ";
            }
        }
        else
        {
            StrategyRow1.text = " ";
            StrategyRow2.text = " ";
            StrategyRow3.text = " ";
            StrategyRow4.text = " ";
            StrategyRow5.text = " ";

            if (winrate > 75)
            {
                StrategyLabelRaises.text = "            Raise";
            }
            else if (winrate > 50)
            {
                StrategyLabelRaises.text = "            Call";
            }
            else
            {
                StrategyLabelRaises.text = "If Winrate > Pot Odds, then Check, else Fold";
            }
        }
    }

    void UpdateWinRate(double w)
    {
        UnityEngine.Debug.Log("UpdateWinRate Called:  " + w);

        WinRateText.text = string.Format("{0:0.00}", w);
    }

    void UpdateHand(string hand)
    {
        UnityEngine.Debug.Log("UpdateHand Called");

        HandText.text = hand;
    }

    void UpdateBoard()
    {
        UnityEngine.Debug.Log("UpdateBoard Called");

        if (boardList.Count > 0)
        {
            string boardString = "";
            foreach (string c in boardList)
            {
                boardString += c + " ";
            }
            boardString.Remove(boardString.Length - 1);

            BoardText.text = boardString;
        }
        else
        {
            BoardText.text = " ";
        }
    }


    // Update is called once per frame
    void Update()
    {
        //UpdateWinRate();
        //if (!preFlop)
        //{
        //    RecommendStrategy();
        //}
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewRound()
    {
        UnityEngine.Debug.Log("NewRound Called");

        hand = new List<string>();
        boardList = new List<string>();
        deck = new List<string>(cards);
        preFlop = true;
        UpdateOpponents(HoloToolkit.Unity.GameSingleton.players - 1);
        UpdateBoard();
        UpdateHand("");
        
        StrategyLabelRaises.text = " ";
        StrategyRow1.text = " ";
        StrategyRow2.text = " ";
        StrategyRow3.text = " ";
        StrategyRow4.text = " ";
        StrategyRow5.text = " ";

        UnityEngine.Debug.Log("After newRound top text: " + StrategyLabelRaises.text);
    }

    public void AddCard(string card)
    {
        UnityEngine.Debug.Log("Card: " + card + " Detected");

        if (hand.Count < 2)
        {
            if (!hand.Contains(card))
            {
                hand.Add(card);
                deck.Remove(card);

                UpdateHand(card);

                if (hand.Count == 2)
                {

                    string handString = "";
                    foreach (string c in hand)
                    {
                        handString += c + " ";
                    }
                    handString.Remove(handString.Length - 1);

                    UpdateHand(handString);

                //task maybe? https://forum.unity3d.com/threads/plugin-async-task-bool-for-windows-store-app.297351/
                 Calculate();
                }
            }
        }
        else if (hand.Count == 2) {
            if (!boardList.Contains(card) && !hand.Contains(card) && boardList.Count < 5)
            {
                boardList.Add(card);
                deck.Remove(card);
                UpdateBoard();

                if (boardList.Count >= 3)
                {
                    preFlop = false;
                    Calculate();
                }
            }
        }
    }


    // An example of how to calculate win odds for multiple players
    double WinOddsMonteCarlo(ulong pocket, ulong board, ulong dead, int nopponents, double duration)
    {
        // Keep track of stats
        double win = 0.0, count = 0.0;

        string boardString = "";
        if (boardList.Count > 0)
        {
            foreach (string c in boardList)
            {
                boardString += c + " ";
            }
            if (boardList.Count == 5)
            {
                boardString.Remove(boardString.Length - 1);
            }
        }

        string[] sub = new string[7];
        sub[0] = hand[0];
        sub[1] = hand[1];
        int k = 2;
        foreach (string c in boardList)
        {
            sub[k] = c;
            k++;
        }

        string[] res = cards.Except(sub).ToArray();

        // Loop for specified duration
        while (count < 100000)
        {

            // Player and board info
            string boa = boardString;
            string str = "";

            List<string> tmp = new List<string>(res);

            for (int j = 0; j < 5-boardList.Count; j++)
            {
                int r = UnityEngine.Random.Range(0, tmp.Count-1);
                str = tmp[r];
                boa +=  str + " ";
                tmp.Remove(str);
            }
            boa.Remove(boa.Length - 1);

            ulong boardmask = Hand.ParseHand(boa);
            uint playerHandVal = Hand.Evaluate(pocket | boardmask);

            // Ensure that dead, board, and pocket cards are not
            // available to opponent hands.
            ulong deadmask = dead | boardmask | pocket;

            // Comparison Results
            bool greaterthan = true;
            bool greaterthanequal = true;

            
            // Get random opponent hand values
            for (int i = 0; i < nopponents; i++)
            {

                // Get Opponent hand info
                string opp ="";
                
                for (int j = 0; j < 2; j++)
                {
                    int r = UnityEngine.Random.Range(0, tmp.Count-1);
                    str = tmp[r];
                    opp += str + " ";
                    tmp.Remove(str);
                }
                opp.Remove(opp.Length - 1);

                ulong oppmask = Hand.ParseHand(opp);
                uint oppHandVal = Hand.Evaluate(oppmask | boardmask);

                // Remove these opponent cards from future opponents
                deadmask |= oppmask;

                // Determine compare status
                if (playerHandVal < oppHandVal)
                {
                    greaterthan = greaterthanequal = false;
                    break;
                }
                else if (playerHandVal <= oppHandVal)
                {
                    greaterthan = false;
                }
            }

            // Calculate stats
            if (greaterthan)
                win += 1.0;
            else if (greaterthanequal)
                win += 0.5;

            count += 1.0;
        }
        
        // Return stats
        return (count == 0.0 ? 0.0 : win / count);
    }
}
