using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public string type;
    Rigidbody2D rb;
    Vector2 pos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;
        switch (type)
        {
            case "Fall":
                Mn.mn.resetFunc += FallReset;
                break;
            default:
                break;
        }
    }

    void FallReset()
    {
        rb.gravityScale = 0;
        rb.isKinematic = true;
        transform.position = pos;
    }
}
