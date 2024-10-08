using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public string gameMode;
    public GameObject cross, nought, bar;

    public TextMeshProUGUI Instructions, player2Name;

    public enum Seed { EMPTY, CROSS, NOUGHT };

    Seed Turn;

    public GameObject[] allSpawns = new GameObject[9];

    public Seed[] player = new Seed[9];

    //track cells of winning cells
    Vector2 pos1, pos2;

    public GameOver GameOverScreen;

    private void Awake()
    {
        //get the game mode from the previous screen
        GameObject peristantObj = GameObject.FindGameObjectWithTag("PersistantObj") as GameObject;
        gameMode = peristantObj.GetComponent<PersistantObject>().gameMode;
        Destroy(peristantObj);

        GameManager.Instance.gameMode = gameMode;

        if (gameMode == "1 Player")
        {
            player2Name.text = "AI Player";
        }
        else
        {
            player2Name.text = "Player 2";
        }

        Turn = Seed.CROSS;

        Instructions.text = "Turn: Player 1";

        for (int i = 0; i < 9; i++)
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

                //winning bar
                float slope = calculateSlope();
                Instantiate(bar, calculateCenter(), Quaternion.Euler(0, 0, slope));
                GameOver();
            }
            else
            {
                Turn = Seed.NOUGHT;
                Instructions.text = "Turn: Player 2";
            }
        }
        else if (Turn == Seed.NOUGHT && gameMode == "2 Players")
        {
            allSpawns[id] = Instantiate(nought, emptycell.transform.position, Quaternion.identity);
            player[id] = Turn;

            if (Won(Turn))
            {
                Turn = Seed.EMPTY;
                Instructions.text = "Player 2 has won!";

                //winning bar
                float slope = calculateSlope();
                Instantiate(bar, calculateCenter(), Quaternion.Euler(0, 0, slope));
                GameOver();
            }
            else
            {
                Turn = Seed.CROSS;
                Instructions.text = "Turn: Player 1";
            }
        }

        if (Turn == Seed.NOUGHT && gameMode == "1 Player")
        {
            int bestScore = -1, bestPos = -1, score = 0;
            for (int i = 0; i < 9; i++)
            {
                if (player[i] == Seed.EMPTY)
                {
                    player[i] = Seed.NOUGHT;
                    score = miniMax(Seed.CROSS, player, -1000, +1000);
                    player[i] = Seed.EMPTY;

                    if (bestScore < score)
                    {
                        bestScore = score;
                        bestPos = i;
                    }
                }
            }
            if (bestPos > -1)
            {
                allSpawns[bestPos] = Instantiate(nought, allSpawns[bestPos].transform.position, Quaternion.identity);
                player[bestPos] = Turn;
            }
            if (Won(Turn))
            {
                // change the turn
                Turn = Seed.EMPTY;

                // change the instructions
                GameOver();
                Instructions.text = "Player 2 has won!";

                // Spawn bar
                float slope = calculateSlope();
                Instantiate(bar, calculateCenter(), Quaternion.Euler(0, 0, slope));
            }
            else
            {
                // change the turn
                Turn = Seed.CROSS;

                // change the instructions
                Instructions.text = "Turn: 1st Player";
            }
    }
        if (IsDraw())
        {
            Turn = Seed.EMPTY;
            Instructions.text = "It's a draw!";
            GameOver();
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
            if (player[allConditions[i, 0]] == currPlayer & player[allConditions[i, 1]] == currPlayer & player[allConditions[i, 2]] == currPlayer)
            {
                hasWon = true;

                //winning positions
                pos1 = allSpawns[allConditions[i, 0]].transform.position;
                pos2 = allSpawns[allConditions[i, 2]].transform.position;
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

        if (player1Won == false & player2Won == false & anyEmpty == false)
        {
            isDraw = true;
        }
        return isDraw;
    }

    Vector2 calculateCenter()
    {
        float x = (int)((pos1.x + pos2.x) / 2);
        float y = (int)((pos1.y + pos2.y) / 2);

        return new Vector2 (x, y);
    }

    float calculateSlope()
    {

        if (pos1.x == pos2.x)
        {
            return 0.0f;
        }
        else if (pos1.y == pos2.y)
        {
            return 90.0f;
        }
        else if (pos1.x > 0.0f)
        {
            return -45.0f;
        }
        else {
            return 45.0f;
        }
    }

    int miniMax(Seed currPlayer, Seed[] board, int alpha, int beta)
    {
        if (IsDraw())
        {
            return 0;
        }
        if (Won(Seed.NOUGHT))
        {
            return +1;
        }
        if (Won(Seed.CROSS))
        {
            return -1;
        }

        int score;

        if(currPlayer == Seed.NOUGHT)
        {
            for (int i = 1; i < 9; i++)
            {
                if (board[i] == Seed.EMPTY)
                {
                    board[i] = Seed.NOUGHT;
                    score = miniMax(Seed.CROSS, board, alpha, beta);
                    board[i] = Seed.EMPTY;

                    if(score > alpha)
                    {
                        alpha = score;
                    }
                    if(alpha > beta)
                    {
                        break;
                    }
                }
            }
            return alpha;
        }
        else
        {
            for (int i = 1; i < 9; i++)
            {
                if (board[i] == Seed.EMPTY)
                {
                    board[i] = Seed.CROSS;
                    score = miniMax(Seed.NOUGHT, board, alpha, beta);
                    board[i] = Seed.EMPTY;

                    if (score < beta)
                    {
                        beta = score;
                    }
                    if (alpha > beta)
                    {
                        break;
                    }
                }
            }
            return beta;
        }
    }

    public void GameOver()
    {
        GameOverScreen.Setup(Instructions.text);
    }
}

