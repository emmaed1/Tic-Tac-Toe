using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCell : MonoBehaviour
{
    public int id;
    public GameObject cam;

    public void OnMouseDown()
    {
        cam.GetComponent<Game>().Spawn(this.gameObject, id);
    }
}
