using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public string gameMode;
    public GameObject cross, nought;

    public TextMeshProUGUI Instructions;

    public enum Seed { EMPTY, CROSS, NOUGHT };

    Seed Turn;

    public GameObject[] allSpawns = new GameObject[9];

    public Seed[] player = new Seed[9];
    private void Awake()
    {
        //get the game mode from the previous screen
        /*GameObject peristantObj = GameObject.FindGameObjectWithTag("PersistantObj") as GameObject;
        gameMode = peristantObj.GetComponent<PersistantObject>().gameMode;
        Destroy(peristantObj);*/

        Turn = Seed.CROSS;

        Instructions.text = "Turn: Player 1";

        for(int i = 0; i < 9; i++)
        {
            player[i] = Seed.EMPTY;
        }
    }

    public void Spawn(GameObject emptycell, int id)
    {
        if (Turn == Seed.CROSS)
        {
            allSpawns[id] = Instantiate(cross, emptycell.transform.position, Quaternion.identity);
            player[id] = Turn;

            if (Won(Turn))
            {
                Turn = Seed.EMPTY;
                Instructions.text = "Player 1 has won!";
            }
            else
            {
                Turn = Seed.NOUGHT;
                Instructions.text = "Turn: Player 2";
            }
        }
        else if (Turn == Seed.NOUGHT)
        {
            allSpawns[id] = Instantiate(nought, emptycell.transform.position, Quaternion.identity);
            player[id] = Turn;

            if (Won(Turn))
            {
                Turn = Seed.EMPTY;
                Instructions.text = "Player 2 has won!";
            }
            else
            {
                Turn = Seed.CROSS;
                Instructions.text = "Turn: Player 1";
            }
        }
        if (IsDraw())
        {
            Turn = Seed.EMPTY;
            Instructions.text = "It's a draw!";
        }
        Destroy(emptycell);
    }

    bool IsAnyEmpty()
    {
        bool empty = false;
        for(int i = 0; i < 9; i++)
        {
            if(player[i] == Seed.EMPTY)
            {
                empty = true;
                break;
            }
        }
        return empty;
    }

    bool Won(Seed currPlayer)
    {
        bool hasWon = false;
        int[,] allConditions = new int[8, 3] { {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
                                                {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
                                                {0, 4, 8}, {2, 4, 6} };

        for (int i = 0; i < 8; i++)
        {
            if (player[allConditions[i, 0]] == currPlayer & player[allConditions[i,1]] == currPlayer & player[allConditions[i,2]] == currPlayer)
            {
                hasWon = true;
                break;
            }
        }
        return hasWon;
    }
    bool IsDraw()
    {
        bool player1Won, player2Won, anyEmpty;

        player1Won = Won(Seed.CROSS);
        player2Won = Won(Seed.NOUGHT);
        anyEmpty = IsAnyEmpty();

        bool isDraw = false;

        if(player1Won == false & player2Won == false & anyEmpty == false)
        {
            isDraw = true;
        }
        return isDraw;
    }
}

