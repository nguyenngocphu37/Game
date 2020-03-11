using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperBall : MonoBehaviour
{
    public GameObject superBall_X;
    public GameObject superBall_Y;
    public GameObject superBall_XY;
    public GameObject superBall_Cricle;
    public GameObject superBall_Cricle_X;
    public GameObject superBall_Cricle_Y;
    public GameObject superBall_Cricle_XY;
    public GameObject superBall_Thunder;
    public GameObject superBall_Bum_Small;
    public GameObject superBall_Bum_Big;
    private Board board;
    private float distance_Scale_change;
    private float distance_Radius_change;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        distance_Scale_change = Constant.DISTANCE_SCALE_CHANGE;
        distance_Radius_change = Constant.DISTANCE_RADIUS_CHANGE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Kết hợp
    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((transform.tag == Constant.TAG_SUPER_BALL_X || transform.tag == Constant.TAG_SUPER_BALL_Y) &&
            (col.transform.tag == Constant.TAG_SUPER_BALL_X || col.transform.tag == Constant.TAG_SUPER_BALL_Y))
        {
            ActiveSuperBallLevelHigher(superBall_XY, transform, col.transform);
        }
        else if (transform.tag == col.transform.tag && transform.tag == Constant.TAG_SUPER_BALL_XY)
        {
            ActiveSuperBallLevelHigher(superBall_Cricle, transform, col.transform);
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_Y) ||
        (transform.tag == Constant.TAG_SUPER_BALL_Y && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_Y, transform, col.transform);
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_X) ||
        (transform.tag == Constant.TAG_SUPER_BALL_X && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_X, transform, col.transform);
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE_X || transform.tag == Constant.TAG_SUPER_BALL_CRICLE_Y) &&
        (col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE_X || col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE_Y))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_XY, transform, col.transform);
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_XY) ||
        (transform.tag == Constant.TAG_SUPER_BALL_XY && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_XY, transform, col.transform);
        }
        else if (transform.tag == col.transform.tag && transform.tag == Constant.TAG_SUPER_BALL_CRICLE)
        {
            ActiveSuperBallLevelHigher(superBall_Bum_Small, transform, col.transform);
        }
        else if (transform.tag == col.transform.tag && transform.tag == Constant.TAG_SUPER_BALL_CRICLE_XY)
        {
            ActiveSuperBallLevelHigher(superBall_Bum_Big, transform, col.transform);
        }
    }


    private void ActiveSuperBallLevelHigher(GameObject superBall, Transform tfOrigin, Transform tfTarget)
    {
        if (tfOrigin.gameObject.activeInHierarchy)
        {
            Instantiate(superBall, transform.position, Quaternion.identity);
            tfTarget.gameObject.SetActive(false);
            tfOrigin.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        switch (transform.tag)
        {
            case Constant.TAG_SUPER_BALL_X:
                InActiveBallFromX(transform, distance_Scale_change, true);
                break;
            case Constant.TAG_SUPER_BALL_Y:
                InActiveBallFromY(transform, distance_Scale_change, true);
                break;
            case Constant.TAG_SUPER_BALL_XY:
                InActiveBallFromX(transform, distance_Scale_change, true);
                InActiveBallFromY(transform, distance_Scale_change, true);
                break;
            case Constant.TAG_SUPER_BALL_CRICLE:
                InActiveBallFromXY(transform, distance_Radius_change, true);
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_X:
                StartCoroutine(InActiveBallFromCricle_X());
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_Y:
                StartCoroutine(InActiveBallFromCricle_Y());
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_XY:
                StartCoroutine(InActiveBallFromCricle_XY());
                break;
            case Constant.TAG_SUPER_BALL_BUM_SMALL:
                break;
            case Constant.TAG_SUPER_BALL_BUM_BIG:
                break;
            case Constant.TAG_SUPER_BALL_THUNDER:
                break;
            default:
                break;
        }
    }

    // Nổ
    private void InActiveBallFromX(Transform tf, float distance, bool isDetroy)
    {
        float radius = GetComponent<CircleCollider2D>().radius;
        float dis = Constant.DISTANCE_RAY_POINT * (tf.localScale.x * radius);
        Vector2 rayTop = new Vector2(tf.position.x, tf.position.y + dis);
        Vector2 rayBottom = new Vector2(tf.position.x, tf.position.y - dis);
        DisibaleBall(Physics2D.RaycastAll(rayTop, new Vector3(1f, 0, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayTop, new Vector3(-1f, 0, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayBottom, new Vector3(1f, 0, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayBottom, new Vector3(-1f, 0, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        if (isDetroy) tf.gameObject.SetActive(false);
    }

    private void InActiveBallFromY(Transform tf, float distance, bool isDetroy)
    {
        float radius = GetComponent<CircleCollider2D>().radius;
        float dis = Constant.DISTANCE_RAY_POINT * (tf.localScale.x * radius);
        Vector2 rayLeft = new Vector2(transform.position.x - dis, transform.position.y);
        Vector2 rayRight = new Vector2(transform.position.x + dis, transform.position.y);
        DisibaleBall(Physics2D.RaycastAll(rayLeft, new Vector3(0, 1f, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayLeft, new Vector3(0, -1f, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayRight, new Vector3(0, 1f, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        DisibaleBall(Physics2D.RaycastAll(rayRight, new Vector3(0, -1f, 0), distance, 1 << LayerMask.NameToLayer("ball")));
        if (isDetroy) tf.gameObject.SetActive(false);
    }

    private void InActiveBallFromXY(Transform tf, float distance, bool isDetroy)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tf.position, distance_Radius_change, 1 << LayerMask.NameToLayer("ball"));
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].gameObject.SetActive(false);
            i++;
        }
        if (isDetroy) tf.gameObject.SetActive(false);

    }

    private void InActiveBallFromCircle()
    {

    }

    private void InActiveBallFromThunder()
    {
        List<GameObject> ballsActive = board.pooledObjects.Where(x => x.activeInHierarchy).ToList();
        int[] index = new int[Constant.AMOUNT_DETROY_THUNDER];

    }

    IEnumerator InActiveBallFromCricle_X()
    {
        InActiveBallFromXY(transform, distance_Radius_change, false);
        yield return new WaitForSeconds(Constant.DELAY_TO_INACTIVE);
        InActiveBallFromX(transform, distance_Scale_change, true);
    }

    IEnumerator InActiveBallFromCricle_Y()
    {
        InActiveBallFromXY(transform, distance_Radius_change, false);
        yield return new WaitForSeconds(Constant.DELAY_TO_INACTIVE);
        InActiveBallFromY(transform, distance_Scale_change, true);
    }

    IEnumerator InActiveBallFromCricle_XY()
    {
        InActiveBallFromXY(transform, distance_Radius_change, false);
        yield return new WaitForSeconds(Constant.DELAY_TO_INACTIVE);
        InActiveBallFromX(transform, distance_Scale_change, false);
        InActiveBallFromY(transform, distance_Scale_change, true);
    }

    private void DisibaleBall(RaycastHit2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.gameObject.SetActive(false);
        }
    }
    // Nổ dây chuyển
}
