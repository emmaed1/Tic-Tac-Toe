using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public string gameMode;
    public GameObject cross, nought;

    public TextMeshProUGUI Instructions;

    enum Seed { EMPTY, CROSS, NOUGHT };

    Seed Turn;

    private void Awake()
    {
        //get the game mode from the previous screen
        /*GameObject peristantObj = GameObject.FindGameObjectWithTag("PersistantObj") as GameObject;
        gameMode = peristantObj.GetComponent<PersistantObject>().gameMode;
        Destroy(peristantObj);*/

        Turn = Seed.CROSS;

        Instructions.text = "Turn: Player 1";
    }

    public void Spawn(GameObject emptycell, int id)
    {
        if (Turn == Seed.CROSS)
        {
            Instantiate(cross, emptycell.transform.position, Quaternion.identity);
            Turn = Seed.NOUGHT;
            Instructions.text = "Turn: Player 2";
        }
        else if (Turn == Seed.NOUGHT)
        {
            Instantiate(nought, emptycell.transform.position, Quaternion.identity);
            Turn = Seed.CROSS;
            Instructions.text = "Turn: Player 1";
        }

        Destroy(emptycell);
    }
}
