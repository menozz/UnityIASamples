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

    public float startX = 0f;
    public const int ballCount = 8;
    private readonly float startY = ballCount + 1f;

    //private float startY = ballCount + 1f;

    float nextUsage;
    public float firstDelay = 1.8f;
    public float delay = 0.35f;

    GameObject deletedObj;

    int goal = 0;
    public int shags = 15;

    GameObject goalLabel, shagLabel;

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

    void Update()
    {
        //transform.position.x = (Input.mousePosition.x - halfW) / halfW;
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(worldPoint);

        if (Input.GetMouseButtonUp(0) && Time.time > nextUsage)
        {
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                gameNormal(hit);
            }
        }
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
        hit.transform.parent = deletedObj.transform;
        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            deletedObj.transform.GetChild(i).GetComponent<circle_controller>().destroyed();
        }
        yield return new WaitForSeconds(delay);
        //  deletedObj.transform.position = new Vector2(-100, -100);
        addPoints((deletedObj.transform.childCount) * 10);
        calcShags((deletedObj.transform.childCount - 1));
        var lst = new List<Vector2>();
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
        hit.transform.GetComponent<circle_controller>().destroyed();
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

    void respawnNewCircles(IEnumerable<Vector2> hits)
    {

        foreach (var vector2 in hits)
        {
            float x = vector2.x, y = this.startY + vector2.y;
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
        int rndN = Random.Range(1, 5);
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
        }
    }
}
