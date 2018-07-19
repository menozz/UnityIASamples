using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    public GameObject red;
    public GameObject attack;
    public GameObject defence;

    //    public float startX = 0f;
    public const int ballCount = 8;
    private readonly float startY = ballCount + startHeight;
    //private readonly float startY = 0;
    private const float startHeight = 1f;
    private const float Precision = 0.0001f;
    public float MainScrollSpeed = 10;
    public float UnitReturnSpeed = 20;
    public float SwapSpeed = 1;

    float nextUsage;
    public float firstDelay = 1.8f;
    public float delay = 0.35f;

    GameObject deletedObj;

        // private List<BallController> _rayCasted;

    int goal = 0;
    public int shags = 15;

    GameObject goalLabel, shagLabel;

    
    public Dictionary<int, GameObject> Units = new Dictionary<int, GameObject>();

    public List<GameObject> VerticalUnits;
    public List<GameObject> HorizontalUnits;


    List<Vector2> lst = new List<Vector2>();
    private Collider2D srchit;
    

    void Start()
    {
        nextUsage = Time.time + firstDelay;
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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        srchit = hit.collider;
        //    }
        //}

    }

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
            Units.Remove(toRemove.GetInstanceID());
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
            Units.Remove(toRemove.GetInstanceID());
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
                setRNDCircle(0, new Vector2(x, y), new Vector2(x, y));
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
            setRNDCircle(0, new Vector2(x, y), new Vector2(x, y));
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

    //private RaycastHit2D RayCastCollider(Collider2D collider, Vector2 direction)
    //{
    //    collider.enabled = false;
    //    var hitNew = Physics2D.Raycast(collider.transform.position, direction);
    //    collider.enabled = true;
    //    return hitNew;
    //}

    void initGameField()
    {
        int id = 0;
        for (int y = 0; y < ballCount; y++)
        {
            for (int x = 0; x < ballCount; x++)
            {
                setRNDCircle(id, new Vector2(x, y), new Vector2(x, startY + y));
                id++;
            }
        }
    }

    void setRNDCircle(int id, Vector2 final, Vector2 spawn)
    {
        int rndN = Random.Range(1, 7);
        //var vector = new Vector2(x, y);
        var start = new Vector2(100, 100);
        GameObject ball = null;
        switch (rndN)
        {
            case 1:
                ball = Instantiate(blue, start, Quaternion.identity);
                break;
            case 2:
                ball = Instantiate(yellow, start, Quaternion.identity);
                break;
            case 3:
                ball = Instantiate(green, start, Quaternion.identity);
                break;
            case 4:
                ball = Instantiate(red, start, Quaternion.identity);
                break;
            case 5:
                ball = Instantiate(attack, start, Quaternion.identity);
                break;
            case 6:
                ball = Instantiate(defence, start, Quaternion.identity);
                break;
        }

        if (ball != null)
        {
            ball.GetComponent<BallController>().Initialize(this, id, final, spawn);
        }
    }

    public void MakeVertHorz(Vector3 finalPosition)
    {
        VerticalUnits = Units.Values.Where(g => g.transform.position.x == finalPosition.x).ToList();
        HorizontalUnits = Units.Values.Where(g => g.transform.position.y == finalPosition.y).ToList();
    }

    public void SwapBufferPosition(BallController source, BallController target)
    {
        var direction = (target.BufferPositon - source.FinalPosition).normalized;
        var distance = Vector2.Distance(target.BufferPositon, source.FinalPosition);
        if (distance < 0.001)
        {
            return;

        }
        Debug.Log("_________________");
        Debug.Log("direction " + direction);
        Debug.Log("distance " + distance);
        Debug.Log("source " + source.FinalPosition);
        Debug.Log("target " + target.BufferPositon);

        var controllers = Physics2D.RaycastAll(source.FinalPosition, direction, distance).Select(g => g.collider.gameObject.GetComponent<BallController>()).ToList();

        //if (_rayCasted != null)
        //{
        //    var u = _rayCasted.Except(controllers);
        //    foreach (var collection in u)
        //    {
        //        collection.SetBufferToFinalPosition();
        //    }
        //}

        var stackPosition = source.FinalPosition;
        foreach (var controller in controllers)
        {
        
            var tmp = controller.FinalPosition;
            controller.MoveToPosition(stackPosition);
            stackPosition = tmp;
        }

      //  _rayCasted = controllers;

        source.BufferPositon = target.BufferPositon;


        //var tmp = source.BufferPositon;
        //source.BufferPositon = target.BufferPositon;
        //target.MoveToPosition(tmp);
    }

    public void CleanPositions()
    {
        foreach (var unit in Units.Values)
        {
            unit.GetComponent<BallController>().IsMoving = false;
        }
        HorizontalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
        VerticalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
        foreach (var unit in Units.Values)
        {
            unit.GetComponent<BallController>().IsMoving = true;
        }
    }
}

