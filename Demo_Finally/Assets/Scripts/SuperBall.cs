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
    public GameObject superBall_Bum_Small;
    public GameObject superBall_Bum_Big;
    public GameObject superBall_Thunder;
    private Board board;
    private float distance_cale_change_X;
    private float distance_cale_change_Y;
    private float distance_radius_change;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        distance_cale_change_X = Constant.DISTANCE_SCALE_CHANGE_X;
        distance_cale_change_Y = Constant.DISTANCE_SCALE_CHANGE_Y;
        distance_radius_change = Constant.DISTANCE_RADIUS_CHANGE;
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
            return;
        }
        else if (transform.tag == col.transform.tag && transform.tag == Constant.TAG_SUPER_BALL_XY)
        {
            ActiveSuperBallLevelHigher(superBall_Cricle, transform, col.transform);
            return;
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_Y) ||
        (transform.tag == Constant.TAG_SUPER_BALL_Y && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_Y, transform, col.transform);
            return;
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_X) ||
        (transform.tag == Constant.TAG_SUPER_BALL_X && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_X, transform, col.transform);
            return;
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE_X || transform.tag == Constant.TAG_SUPER_BALL_CRICLE_Y) &&
        (col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE_X || col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE_Y))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_XY, transform, col.transform);
            return;
        }
        else if ((transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_XY) ||
        (transform.tag == Constant.TAG_SUPER_BALL_XY && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE))
        {
            ActiveSuperBallLevelHigher(superBall_Cricle_XY, transform, col.transform);
            return;
        }
        else if (transform.tag == Constant.TAG_SUPER_BALL_CRICLE && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE)
        {
            ActiveSuperBallLevelHigher(superBall_Bum_Small, transform, col.transform);
            return;
        }
        else if (transform.tag == Constant.TAG_SUPER_BALL_CRICLE_XY && col.transform.tag == Constant.TAG_SUPER_BALL_CRICLE_XY)
        {
            ActiveSuperBallLevelHigher(superBall_Bum_Big, transform, col.transform);
            return;
        }
    }


    private void ActiveSuperBallLevelHigher(GameObject superBall, Transform tfOrigin, Transform tfTarget)
    {
        if (tfOrigin.gameObject.activeInHierarchy && tfTarget.gameObject.activeInHierarchy)
        {
            Instantiate(superBall, tfOrigin.position, Quaternion.identity);
            tfTarget.gameObject.SetActive(false);
            tfOrigin.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        ActionWithSuperBall(transform.tag, transform);
    }

    private void ActionWithSuperBall(string tagObject, Transform tf)
    {
        switch (tagObject)
        {
            case Constant.TAG_SUPER_BALL_X:
                StartCoroutine(InActiveBall_Wait_With_X(tf, 0, distance_cale_change_X));
                break;
            case Constant.TAG_SUPER_BALL_Y:
                StartCoroutine(InActiveBall_Wait_With_Y(tf, 0, distance_cale_change_Y));
                break;
            case Constant.TAG_SUPER_BALL_XY:
                StartCoroutine(InActiveBall_Wait_With_XY(tf, 0, distance_cale_change_X, distance_cale_change_Y));
                break;
            case Constant.TAG_SUPER_BALL_CRICLE:
                StartCoroutine(InActiveBall_Wait_WithCricle(tf, 0, distance_radius_change));
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_X:
                StartCoroutine(InActiveBall_Wait_WithCricle_X(tf, Constant.DELAY_TO_INACTIVE, distance_radius_change, distance_cale_change_X));
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_Y:
                StartCoroutine(InActiveBall_Wait_WithCricle_Y(tf, Constant.DELAY_TO_INACTIVE, distance_radius_change, distance_cale_change_Y));
                break;
            case Constant.TAG_SUPER_BALL_CRICLE_XY:
                StartCoroutine(InActiveBall_Wait_WithCricle_XY(tf, Constant.DELAY_TO_INACTIVE, distance_radius_change, distance_cale_change_X, distance_cale_change_Y));
                break;
            case Constant.TAG_SUPER_BALL_BUM_SMALL:
                break;
            case Constant.TAG_SUPER_BALL_BUM_BIG:
                break;
            case Constant.TAG_SUPER_BALL_THUNDER:
                StartCoroutine(InActiveBall_Wait_WithThunder(tf, Constant.AMOUNT_DESTROY_THUNDER, 0, 0));
                break;
            default:
                break;
        }
    }

    /// Phát hiện bóng
    /// Theo chiều X, chiều Y, theo hình Tròn có bán kính 
    private GameObject[] HitBallFromX(Transform tf, float distance, string layerName)
    {
        List<RaycastHit2D> RaycastHit2DBalls = new List<RaycastHit2D>();
        float radius = GetComponent<CircleCollider2D>().radius;
        float dis = Constant.DISTANCE_RAY_POINT * (tf.localScale.x * radius);
        Vector2 rayTop = new Vector2(tf.position.x, tf.position.y + dis);
        Vector2 rayBottom = new Vector2(tf.position.x, tf.position.y - dis);
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayTop, new Vector3(1f, 0, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayTop, new Vector3(-1f, 0, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayBottom, new Vector3(1f, 0, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayBottom, new Vector3(-1f, 0, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        return RaycastHit2DBalls.Select(x => x.transform.gameObject).Distinct().ToArray();
    }

    private GameObject[] HitBallFromY(Transform tf, float distance, string layerName)
    {
        List<RaycastHit2D> RaycastHit2DBalls = new List<RaycastHit2D>();
        float radius = GetComponent<CircleCollider2D>().radius;
        float dis = Constant.DISTANCE_RAY_POINT * (tf.localScale.x * radius);
        Vector2 rayLeft = new Vector2(transform.position.x - dis, transform.position.y);
        Vector2 rayRight = new Vector2(transform.position.x + dis, transform.position.y);
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayLeft, new Vector3(0, 1f, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayLeft, new Vector3(0, -1f, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayRight, new Vector3(0, 1f, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        RaycastHit2DBalls.AddRange(Physics2D.RaycastAll(rayRight, new Vector3(0, -1f, 0), distance, 1 << LayerMask.NameToLayer(layerName)));
        return RaycastHit2DBalls.Select(x => x.transform.gameObject).Distinct().ToArray();
    }

    private GameObject[] HitBallFromXY(Transform tf, float distance, string layerName)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(tf.position, distance, 1 << LayerMask.NameToLayer(layerName));
        return hitColliders.Select(x => x.gameObject).Distinct().ToArray();
    }

    private void InActiveBallFromCircle()
    {

    }

    private GameObject[] HitBallFromThunder(Transform tf, int amountDestroy)
    {
        GameObject[] objsActive = board.pooledObjects?.ToArray();
        for (int i = 0; i < objsActive.Count(); i++)
        {
            GameObject tmp = objsActive[i];
            int r = Random.Range(0, objsActive.Count() - 1);
            objsActive[i] = objsActive[r];
            objsActive[r] = tmp;
        }
        return objsActive.Take(amountDestroy).ToArray();
    }

    // Nổ
    IEnumerator InActiveBall_Wait_With_X(Transform tf, float? waitFirst, float scaleChange)
    {
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromX(tf, scaleChange, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }
    IEnumerator InActiveBall_Wait_With_Y(Transform tf, float? waitFirst, float scaleChange)
    {
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromY(tf, scaleChange, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }
    IEnumerator InActiveBall_Wait_With_XY(Transform tf, float? waitFirst, float scaleChangeX, float scaleChangeY)
    {
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromX(tf, scaleChangeX, Constant.LAYER_BALL), tf);
        InActiveBall(HitBallFromY(tf, scaleChangeY, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }

    IEnumerator InActiveBall_Wait_WithCricle(Transform tf, float? waitFirst, float distance)
    {
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromXY(tf, distance, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }

    IEnumerator InActiveBall_Wait_WithCricle_X(Transform tf, float? waitFirst, float distance, float scaleChangeX)
    {
        InActiveBall(HitBallFromXY(tf, distance, Constant.LAYER_BALL), tf);
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromX(tf, scaleChangeX, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }

    IEnumerator InActiveBall_Wait_WithCricle_Y(Transform tf, float? waitFirst, float distance, float scaleChangeY)
    {
        InActiveBall(HitBallFromXY(tf, distance, Constant.LAYER_BALL), tf);
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromY(tf, scaleChangeY, Constant.LAYER_BALL), tf);
        ActionFinal(tf);

    }

    IEnumerator InActiveBall_Wait_WithCricle_XY(Transform tf, float? waitFirst, float distance, float scaleChangeX, float scaleChangeY)
    {
        InActiveBall(HitBallFromXY(tf, distance, Constant.LAYER_BALL), tf);
        yield return new WaitForSeconds(waitFirst.Value);
        InActiveBall(HitBallFromX(tf, scaleChangeX, Constant.LAYER_BALL), tf);
        InActiveBall(HitBallFromY(tf, scaleChangeY, Constant.LAYER_BALL), tf);
        ActionFinal(tf);
    }

    IEnumerator InActiveBall_Wait_WithThunder(Transform tf, int amountDestroy, float? waitFirst, float? waitSecond)
    {
        yield return new WaitForSeconds(waitFirst.Value);
        GameObject[] hitBalls = HitBallFromThunder(tf, amountDestroy);
        InActiveBall(hitBalls, tf);
        ActionFinal(tf);
    }

    private void InActiveBall(GameObject[] gameObjs, Transform origin)
    {
        List<GameObject> nearSuperBall = new List<GameObject>();
        if (gameObjs != null)
        {
            for (int i = 0; i < gameObjs.Length; i++)
            {
                if (gameObjs[i].activeInHierarchy)
                {
                    if (!isSuperBall(gameObjs[i].tag))
                    {
                        gameObjs[i].SetActive(false);
                    }
                    else if (origin.gameObject != gameObjs[i])
                    {
                        nearSuperBall.Add(gameObjs[i]);
                    }
                }
            }
        }

        // Nổ dây chuyền
        if (nearSuperBall != null && nearSuperBall.Count() > 0)
        {

            ActionBumSpread(nearSuperBall.ToArray(), 3f);
        }

    }

    private void ActionFinal(Transform tf)
    {
        tf.gameObject.SetActive(false);
        board.ActiveBall();
    }

    // Nổ dây chuyển
    private void ActionBumSpread(GameObject[] nearSuperBalls, float delay)
    {
        //yield return new WaitForSeconds(delay);
        foreach (var item in nearSuperBalls)
        {
            if (item.activeInHierarchy)
            {
                GameObject[] objs = HitBallFromXY(item.transform, distance_radius_change, Constant.LAYER_BALL);
                InActiveBall(objs, item.transform);
                ActionFinal(item.transform);
            }
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

    // Phá xích, Phá gạch đá
}
