using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameGenField : MonoBehaviour
{
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    public GameObject red;

    public float startX = 0f;
    public float startY = 0f;

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
        if (Input.GetMouseButtonUp(0) && Time.time > nextUsage)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        deletedObj.transform.position = new Vector2(-100, -100);
        respawnNewCircles();
        addPoints((deletedObj.transform.childCount) * 10);
        calcShags((deletedObj.transform.childCount - 1));
        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            Destroy(deletedObj.transform.GetChild(i).gameObject);
        }
        deletedObj.transform.position = new Vector2(-20, -20);
    }

    IEnumerator deleteOne(RaycastHit2D hit)
    {
        deletedObj.transform.DetachChildren();
        hit.transform.parent = deletedObj.transform;
        hit.transform.GetComponent<circle_controller>().destroyed();
        yield return new WaitForSeconds(delay);
        deletedObj.transform.position = new Vector2(-100, -100);
        respawnNewCircles();
        addPoints(10);
        calcShags(-1);
        for (int i = 0; i < deletedObj.transform.childCount; i++)
        {
            Destroy(deletedObj.transform.GetChild(i).gameObject);
        }
        deletedObj.transform.position = new Vector2(-20, -20);
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

    void respawnNewCircles()
    {
        float x = 0f, y = 4f;
        for (int i = 0; i < 5; i++)
        for (int j = 0; j < 5; j++)
        {
                Vector2 worldPoint = new Vector2(x + i, y + j);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider == null)
            {
                setRNDCircle(x + i, y + j);
            }
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

    private static RaycastHit2D RayCast(RaycastHit2D hit, Vector2 direction)
    {
        hit.collider.enabled = false;
        var hitNew = Physics2D.Raycast(hit.transform.position, direction);
        hit.collider.enabled = true;
        return hitNew;
    }

    void initGameField()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                setRNDCircle(startX + x, startY + y);
            }
        }
    }

    void setRNDCircle(float x, float y)
    {
        int rndN = Random.Range(1, 5);

        switch (rndN)
        {
            case 1:
                Instantiate(blue, new Vector2(x, y), Quaternion.identity);
                break;
            case 2:
                Instantiate(yellow, new Vector2(x, y), Quaternion.identity);
                break;
            case 3:
                Instantiate(green, new Vector2(x, y), Quaternion.identity);
                break;
            case 4:
                Instantiate(red, new Vector2(x, y), Quaternion.identity);
                break;
        }
    }
}
