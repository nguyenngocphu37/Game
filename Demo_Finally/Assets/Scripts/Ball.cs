using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Board board;
    private int touchCount;

    private GameObject tempBall;
    private int countDetroyBall;


    public GameObject superBall_X;
    public GameObject superBall_Y;
    public GameObject superBall_Cricle;
    public GameObject superBall_Thunder;

    private int COUNT_BALL = 0;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        countDetroyBall = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        float radius = GetComponent<CircleCollider2D>().radius;
        tempBall = transform.gameObject;
        InActiveNearBall(transform, radius);
        ActiveSuperBall(countDetroyBall + 1);
        countDetroyBall = 0;
        board.ActiveBall();
    }


    private void ActiveSuperBall(int amount)
    {
        if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_X)
        {
            GameObject obj = Instantiate(superBall_X, tempBall.transform.position, Quaternion.identity);
            board.superBalls.Add(obj);
        }
        else if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_Y)
        {
            GameObject obj = Instantiate(superBall_Y, tempBall.transform.position, Quaternion.identity);
            board.superBalls.Add(obj);
        }
        else if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_CRICLE)
        {
            GameObject obj = Instantiate(superBall_Cricle, transform.transform.position, Quaternion.identity);
            board.superBalls.Add(obj);
        }
        else if (amount >= Constant.AMOUNT_BALL_CREATE_SBALL_THUNDER)
        {
            GameObject obj = Instantiate(superBall_Thunder, transform.transform.position, Quaternion.identity);
            board.superBalls.Add(obj);
        }
    }

    private void InActiveNearBall(Transform tr, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tr.position, radius * transform.localScale.x, 1 << LayerMask.NameToLayer(Constant.LAYER_BALL));
        int i = 0;
        while (i < hitColliders.Length)
        {
            var isActive = hitColliders[i].gameObject.activeInHierarchy;
            if (hitColliders[i].transform.tag == tr.tag && isActive && hitColliders[i].gameObject != tr.gameObject)
            {
                if (!isSuperBall(hitColliders[i].tag))
                {
                    GameObject nearBall = hitColliders[i].gameObject;
                    tr.gameObject.SetActive(false);
                    nearBall.SetActive(false);
                    InActiveNearBall(nearBall.transform, radius);
                    countDetroyBall++;
                }
            }
            i++;
        }
    }

    private bool isSuperBall(string tag)
    {
        if (tag == Constant.TAG_SUPER_BALL_X ||
            tag == Constant.TAG_SUPER_BALL_Y ||
            tag == Constant.TAG_SUPER_BALL_XY ||
            tag == Constant.TAG_SUPER_BALL_BUM_BIG ||
            tag == Constant.TAG_SUPER_BALL_BUM_SMALL ||
            tag == Constant.TAG_SUPER_BALL_CRICLE ||
            tag == Constant.TAG_SUPER_BALL_CRICLE_X ||
            tag == Constant.TAG_SUPER_BALL_CRICLE_Y ||
            tag == Constant.TAG_SUPER_BALL_CRICLE_XY ||
            tag == Constant.TAG_SUPER_BALL_THUNDER
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
