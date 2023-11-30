using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mn : MonoBehaviour
{
    public int stageNum;
    public static Mn mn;
    public Transform ballPrefab;
    Transform ball;
    Transform ballSpawnPoint;
    public int starCount;
    public int starNum;
    public List<GameObject> disabledObj;
    public delegate void ResetFunc();
    public event ResetFunc resetFunc;

    void Awake()
    {
        mn = this;
        ballSpawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
        ballSpawnPoint.position = new Vector3(ballSpawnPoint.position.x, ballSpawnPoint.position.y, 0);
        disabledObj = new List<GameObject>();
        starNum = GameObject.FindGameObjectsWithTag("Star").Length;
        
    }

    void Start()
    {
        ball = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        Respawn();
    }

    public void GetStar()
    {
        starCount++;
        if(starCount == starNum)
        {
            SceneManager.LoadScene("Stage"+(stageNum+1).ToString());
        }
    }

    public void BallDie()
    {
        ball.gameObject.SetActive(false);
        Invoke("Respawn", 1);
    }

    public void DisableObj(GameObject obj)
    {
        obj.SetActive(false);
        disabledObj.Add(obj);
    }

    public void Respawn()
    {
        starCount = 0;
        ball.position = ballSpawnPoint.position;
        ball.gameObject.SetActive(true);
        foreach (GameObject item in disabledObj)
        {
            item.SetActive(true);
        }
        disabledObj = new List<GameObject>();
        if(resetFunc != null) resetFunc.Invoke();
    }
}
