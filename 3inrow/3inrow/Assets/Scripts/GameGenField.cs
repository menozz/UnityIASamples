using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameGenField : MonoBehaviour
{
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    public GameObject red;
    public GameObject attack;
    public GameObject defence;

    public float startX = 0f;
    public const int ballCount = 8;
    private readonly float startY = ballCount + startHeight;
    private const float startHeight = 1f;
    private const float Precision = 0.0001f;

    float nextUsage;
    public float firstDelay = 1.8f;
    public float delay = 0.35f;

    GameObject deletedObj;

    int goal = 0;
    public int shags = 15;

    GameObject goalLabel, shagLabel;
    private Dictionary<int, GameObject> balls = new Dictionary<int, GameObject>();

    List<Vector2> lst = new List<Vector2>();
    private Collider2D srchit;

    private GameObject srcObj;
    private GameObject dstObj;

    private Vector3 fixSrcPos;
    private Vector3 fixDstPos;

    private bool _move;

    void Start()
    {
        nextUsage = Time.time + firstDelay;
        deletedObj = GameObject.Find("deleteObj");
        initGameField();

        goalLabel = GameObject.Find("GoalLabel") as GameObject;
        shagLabel = GameObject.Find("ShagLabel") as GameObject;
        goalLabel.GetComponent<Text>().text = goal + "";
        shagLabel.GetComponent<Text>().text = shags + "";
    }

    void FixedUpdate()
    {

    }

    void OnMouseDown()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            srchit = hit.collider;
        }
    }

    void OnMouseDrag()
    {
        //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        //transform.position = curPosition;
    }

    bool MoveTo(GameObject obj, Vector3 destination)
    {
        if (obj.transform.position != destination)
        {
            // Calculate the next position
            //float delta = 15 * Time.deltaTime;
          //  float speed = Vector3.Distance(destination, obj.transform.position) / 1f;
            Vector3 currentPosition = obj.transform.position;
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, 2f * Time.deltaTime);

            // Move the object to the next position
            obj.transform.position = nextPosition;
            return true;
        }

        return false;
    }

    void Update()
    {
        if (_move)
        {
            // var dstPos = dstObj.transform.position;
            // var srcPos = srcObj.transform.position;
            // var curSrc = new Vector2(srcPos.x, srcPos.y);
            // var curTrgt = new Vector2(dstPos.x, dstPos.y);
            //// float Speed = Vector3.Distance(curSrc, curTrgt) / 2f;
            // srcObj.transform.position = Vector2.Lerp(curSrc, curTrgt, 30 * Time.deltaTime);
            // dstObj.transform.position = Vector2.Lerp(curTrgt, curSrc, 30 * Time.deltaTime);
            if (srcObj != null && fixDstPos != fixSrcPos)
            {
                var u = MoveTo(dstObj, fixSrcPos);
                u &= MoveTo(dstObj, fixSrcPos);
                if (!u)
                {
                    Clean();
                    _move = false;
                }
            }
        }
        //if (Input.GetMouseButtonUp(0) && Time.time > nextUsage)
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        gameNormal(hit);
        //    }
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit= Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        srchit = hit.collider;
        //    }
        //}

        //if (Input.GetAxis("Mouse X") > 0)
        //{
        //    //Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    //var srchit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //    //var srchit = Physics2D.Raycast(transform.position, Vector2.zero);
        //    if (srchit!=null)
        //    {
        //        var gobj = testRightGet(srchit);
        //        Debug.Log(gobj.name);

        //        //    var tmp = srchit.transform.position;
        //        var curSrc = new Vector2(srchit.transform.position.x, srchit.transform.position.y);
        //        //var newPosit = new Vector2(gobj.transform.position.x, gobj.transform.position.y);
        //        var curTrgt = new Vector2(gobj.transform.position.x, gobj.transform.position.y);
        //        srchit.transform.position = Vector2.MoveTowards(curSrc, curTrgt, 0.5f);
        //        gobj.transform.position = Vector2.MoveTowards(curTrgt, curSrc, 0.5f);

        //        //Debug.Log(gobj.transform.position);

        //        //Vector2 dstworldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        //var dsthit = Physics2D.Raycast(dstworldPoint, Vector2.zero);
        //        //if (dsthit.collider != null)
        //        //{

        //        //}
        //    }
        //}

        //if (Input.GetAxis("Mouse X") < 0 || (Input.GetAxis("Mouse X") > 0))
        //{
        //   Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null && hit.transform.childCount > 0 && hit.transform.GetChild(0).tag == "unit")
        //    {
        //        var children = hit.transform.GetChild(0);
        //        //   Debug.Log("animation");
        //        // var t = hit.collider.gameObject;
        //        var u = children.GetComponent<circle_controller>();
        //        //  var h = (circle_controller)FindObjectOfType(typeof(circle_controller));
        //        if (u != null)
        //        {
        //            //    u.MouseMove();

        //        }
        //        //    hit.collider.gameObject.transform.GetComponent<circle_controller>().MouseMove(true);
        //    }
        //}


        //if (Input.M(0) && Time.time > nextUsage)
        //{
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //    if (hit.collider != null)
        //    {
        //        gameNormal(hit);
        //    }
        //}

        //   this.DoSomethingInAWhile();

    }

    private void Clean()
    {
        srcObj = null;
        dstObj = null;
        fixDstPos = Vector3.zero;
        fixSrcPos = Vector3.zero;
    }

    GameObject testRightGet(Collider2D hit)
    {
        var hitNew = RayCastCollider(hit, hit.transform.right);
        //for (int i = 0; i < 4; i++)
        //{
        if (hitNew.collider != null)// && hit.collider.name == hitNew.collider.name)
        {
            return hitNew.collider.gameObject;
        }
        //}
        return null;
    }


    //private void DoSomethingInAWhile()
    //{
    //    Vector2 prev = Vector2.zero;
    //    foreach (var vector21 in this.lst)
    //    {
    //        if (prev == Vector2.zero)
    //        {
    //            prev = vector21;
    //        }
    //        Debug.DrawLine(prev, vector21);
    //        //Debug.DrawLine(Vector2.zero, vector21);
    //    }
    //    //    Debug.DrawLine(Vector2.up, vector2);
    //}


    void gameNormal(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            testRight(hit);
            testLeft(hit);

            if (deletedObj.transform.childCount > 1)
            {
                StartCoroutine(deleteAll(hit));
            }

            else
            {
                this.deletedObj.transform.DetachChildren();
                testTop(hit);
                testBottom(hit);

                if (deletedObj.transform.childCount > 1)
                {
                    StartCoroutine(deleteAll(hit));
                }
                else
                {
                    StartCoroutine(deleteOne(hit));
                }
            }
            nextUsage = Time.time + delay;
        }
    }

    IEnumerator deleteAll(RaycastHit2D hit)
    {
        this.lst = new List<Vector2>();
        hit.transform.parent = deletedObj.transform;
        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            // deletedObj.transform.GetChild(i).GetComponent<circle_controller>().destroyed();
        }
        yield return new WaitForSeconds(delay);
        //  deletedObj.transform.position = new Vector2(-100, -100);
        addPoints((deletedObj.transform.childCount) * 10);
        calcShags((deletedObj.transform.childCount - 1));

        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            lst.Add(deletedObj.transform.GetChild(i).gameObject.transform.position);
            var toRemove = deletedObj.transform.GetChild(i).gameObject;
            balls.Remove(toRemove.GetInstanceID());
            Destroy(toRemove);
        }
        respawnNewCircles(lst);
        //  deletedObj.transform.position = new Vector2(-20, -20);
    }

    IEnumerator deleteOne(RaycastHit2D hit)
    {
        deletedObj.transform.DetachChildren();
        hit.transform.parent = deletedObj.transform;
        //  hit.transform.GetComponent<circle_controller>().destroyed();
        yield return new WaitForSeconds(delay);
        //deletedObj.transform.position = new Vector2(-100, -100);
        addPoints(10);
        calcShags(-1);
        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            var toRemove = deletedObj.transform.GetChild(i).gameObject;
            balls.Remove(toRemove.GetInstanceID());
            Destroy(toRemove);
        }
        respawnNewCircles(hit.transform.position);

        // deletedObj.transform.position = new Vector2(-20, -20);
    }

    void addPoints(int addP)
    {
        goal = goal + addP;
        goalLabel.GetComponent<Text>().text = goal + "";
    }

    void calcShags(int shag)
    {
        shags = shags + shag;
        shagLabel.GetComponent<Text>().text = shags + "";
        if (shags < 1) gameOver();
    }

    void gameOver()
    {
        SceneManager.LoadScene("Game");
    }

    void respawnNewCircles(List<Vector2> hits)
    {
        float y = this.startY;
        Vector2 prev = hits[0];
        for (int i = 0; i < hits.Count; i++)
        {
            var vector2 = hits[i];
            float x = vector2.x;
            if (i != 0)
            {
                var newVector = vector2 - prev;
                newVector.Normalize();
                if (!(Mathf.Abs(Mathf.Round(newVector.y)) < Precision))
                {
                    y += startHeight;
                }
            }

            Vector2 worldPoint = new Vector2(x, y);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider == null)
            {
                setRNDCircle(x, y);
            }

        }
        //float x = 0f, y = this.startY + 1;
        //for (int i = 0; i < 8; i++)
        //    for (int j = 0; j < 8; j++)
        //    {
        //        Vector2 worldPoint = new Vector2(x + i, y + j);
        //        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //        if (hit.collider == null)
        //        {
        //            setRNDCircle(x + i, y + j);
        //        }
        //    }
    }


    void respawnNewCircles(Vector2 hitObj)
    {
        float x = hitObj.x, y = this.startY;
        Vector2 worldPoint = new Vector2(x, y);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider == null)
        {
            setRNDCircle(x, y);
        }
    }

    void testRight(RaycastHit2D hit)
    {
        var hitNew = RayCast(hit, hit.transform.right);
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hitNew = RayCast(hitNew, hit.transform.right);
            }
        }
    }

    void testTop(RaycastHit2D hit)
    {
        var hitNew = RayCast(hit, hit.transform.up);
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hitNew = RayCast(hitNew, hit.transform.up);
            }
        }
    }

    void testBottom(RaycastHit2D hit)
    {
        var hitNew = RayCast(hit, -hit.transform.up);
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hitNew = RayCast(hitNew, -hit.transform.up);
            }
        }
    }

    void testLeft(RaycastHit2D hit)
    {
        var hitNew = RayCast(hit, -hit.transform.right);
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hitNew = RayCast(hitNew, -hit.transform.right);
            }
        }
    }

    private RaycastHit2D RayCast(RaycastHit2D hit, Vector2 direction)
    {
        hit.collider.enabled = false;
        var hitNew = Physics2D.Raycast(hit.transform.position, direction);
        hit.collider.enabled = true;
        return hitNew;
    }

    private RaycastHit2D RayCastCollider(Collider2D collider, Vector2 direction)
    {
        collider.enabled = false;
        var hitNew = Physics2D.Raycast(collider.transform.position, direction);
        collider.enabled = true;
        return hitNew;
    }

    void initGameField()
    {
        for (int y = 0; y < ballCount; y++)
        {
            for (int x = 0; x < ballCount; x++)
            {
                setRNDCircle(startX + x, startY + y);
            }
        }
    }

    void setRNDCircle(float x, float y)
    {
        int rndN = Random.Range(1, 7);
        var vector = new Vector2(x, y);
        GameObject ball = null;
        switch (rndN)
        {
            case 1:
                ball = Instantiate(blue, vector, Quaternion.identity);
                break;
            case 2:
                ball = Instantiate(yellow, vector, Quaternion.identity);
                break;
            case 3:
                ball = Instantiate(green, vector, Quaternion.identity);
                break;
            case 4:
                ball = Instantiate(red, vector, Quaternion.identity);
                break;
            case 5:
                ball = Instantiate(attack, vector, Quaternion.identity);
                break;
            case 6:
                ball = Instantiate(defence, vector, Quaternion.identity);
                break;
        }

        if (ball != null)
        {
            var controller = ball.transform.GetChild(0).GetComponent<UnitController>();
            controller.addMouseDown(addMouseDown);
            controller.addMouseDrag(addMouseDrag);
            controller.addMouseUp(addMouseUp);
            controller.addMouseEnter(addMouseEnter);
            balls.Add(ball.GetInstanceID(), ball);
        }
    }

    private void addMouseEnter(UnitController obj)
    {
        dstObj = obj.gameObject;
        if (srcObj != null)
        {
            fixDstPos = dstObj.transform.position;
            fixSrcPos = srcObj.transform.position;
            if (fixSrcPos != fixDstPos)
            {
                var srcit = Physics2D.Raycast(fixSrcPos, Vector2.zero);
                var dsthit = Physics2D.Raycast(fixDstPos, Vector2.zero);
                if (srcit.collider != null && dsthit.collider != null)
                {
                    _move = true;
                }
            }
        }
    }

    private void addMouseUp(UnitController obj)
    {
        Clean();
        //Debug.Log("mouse up");
        //Debug.Log(obj.transform.position);
    }

    private void addMouseDrag(UnitController obj)
    {
        //Debug.Log("mouse drag");
        //Debug.Log(obj.transform.position);
    }

    private void addMouseDown(UnitController obj)
    {
        //Debug.Log("mouse down");
        //Debug.Log(obj.transform.position);
        srcObj = obj.gameObject;
    }
}
