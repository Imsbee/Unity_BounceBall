using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOnOff : MonoBehaviour
{
    public int count;
    SpriteRenderer sr;
    public Color offColor;
    public Color onColor;
    GameObject[] stars;

    void Awake()
    {
        stars = GameObject.FindGameObjectsWithTag("Star");
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
    }

    public void Reset()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
        }
        sr.color = offColor;
    }

    void Start()
    {
        Mn.mn.resetFunc += Reset;
        Reset();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        if(count == 1)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(true);
            }
            sr.color = onColor;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        count--;
        if (count == 0)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(false);
            }
            sr.color = offColor;
        }
    }
}
