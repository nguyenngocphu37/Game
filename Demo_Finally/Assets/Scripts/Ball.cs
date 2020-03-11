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
    }


    private void ActiveSuperBall(int amount)
    {
        if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_X)
        {
            Instantiate(superBall_X, tempBall.transform.position, Quaternion.identity);
        }
        else if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_Y)
        {
            Instantiate(superBall_Y, tempBall.transform.position, Quaternion.identity);
        }
        else if (amount == Constant.AMOUNT_BALL_CREATE_SBALL_CRICLE)
        {
            Instantiate(superBall_Cricle, transform.transform.position, Quaternion.identity);
        }
        else if (amount >= Constant.AMOUNT_BALL_CREATE_SBALL_THUNDER)
        {
            Instantiate(superBall_Thunder, transform.transform.position, Quaternion.identity);
        }
    }

    private void InActiveNearBall(Transform tr, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tr.position, radius * transform.localScale.x, 1 << LayerMask.NameToLayer("ball"));
        int i = 0;
        while (i < hitColliders.Length)
        {
            var isActive = hitColliders[i].gameObject.activeInHierarchy;
            if (hitColliders[i].transform.tag == tr.tag && isActive && hitColliders[i].gameObject != tr.gameObject)
            {
                GameObject nearBall = hitColliders[i].gameObject;
                tr.gameObject.SetActive(false);
                nearBall.SetActive(false);
                InActiveNearBall(nearBall.transform, radius);
                countDetroyBall++;
            }
            i++;
        }
    }
}
