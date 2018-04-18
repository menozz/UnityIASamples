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

    List<Vector2> lst = new List<Vector2>();


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

    void Update()
    {
        //if (Input.GetMouseButtonUp(0) && Time.time > nextUsage)
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        gameNormal(hit);
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0)
            {
                Vector2 srcworldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var srchit = Physics2D.Raycast(srcworldPoint, Vector2.zero);
                if (srchit.collider != null)
                {
                    {
                        Debug.Log("drag");
                        //Vector2 dstworldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //var dsthit = Physics2D.Raycast(dstworldPoint, Vector2.zero);
                        //if (dsthit.collider != null)
                        //{

                        //}
                    }
                }
            }


        }

        //if (Input.GetAxis("Mouse X") < 0 || (Input.GetAxis("Mouse X") > 0))
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            Destroy(deletedObj.transform.GetChild(i).gameObject);
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
            Destroy(deletedObj.transform.GetChild(i).gameObject);
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
        switch (rndN)
        {
            case 1:
                Instantiate(blue, vector, Quaternion.identity);
                break;
            case 2:
                Instantiate(yellow, vector, Quaternion.identity);
                break;
            case 3:
                Instantiate(green, vector, Quaternion.identity);
                break;
            case 4:
                Instantiate(red, vector, Quaternion.identity);
                break;
            case 5:
                Instantiate(attack, vector, Quaternion.identity);
                break;
            case 6:
                Instantiate(defence, vector, Quaternion.identity);
                break;
        }
    }
}
