using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int stageNum;
    public static GameManager gameManager;
    public Transform ballPrefab;
    Transform ball;
    Transform ballSpawnPoint;
    public int starCount;
    public int starNum;
    public List<GameObject> disabledObj;

    // 프리팹이 인스턴스화 된 직후에 호출
    void Awake()
    {
        gameManager = this;
        ballSpawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
        ballSpawnPoint.position = new Vector3(ballSpawnPoint.position.x, ballSpawnPoint.position.y, 0);
        disabledObj = new List<GameObject>();
        starNum = GameObject.FindGameObjectsWithTag("Star").Length;
    }

    // 인스턴스가 활성화 된 상태에서 첫 프레임 업데이트 전에 호출 (Awake() 호출 이후에 호출)
    void Start()
    {
        ball = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        Respawn();
    }

    public void GetStar()
    {
        starCount++;

        if (starCount == starNum)
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
    }
}
