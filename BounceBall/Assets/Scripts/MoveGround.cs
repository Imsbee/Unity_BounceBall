using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    int n = 1;
    Vector2 pos;

    void Awake()
    {
        pos = transform.position;
    }
    
    void Update()
    {
        pos.x += n * Time.deltaTime;

        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Broken")
        {
            n = n * -1;
        }
    }
}
