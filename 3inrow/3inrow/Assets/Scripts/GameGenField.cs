using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class GameGenField : MonoBehaviour
{

    /// <summary>
    /// Основной скрипт управляющий игрой
    /// </summary>

    // префабы для загрузки шариков разных цветов
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    public GameObject red;

    /// <summary>
    /// Стартовые координаты для сетки шариков
    /// </summary>
    public float startX = -4.2f;
    public float startY = -0.2f;


    //GameObject newCircle;

    /// <summary>
    /// переменные, куда записываются таймеры, по которым проверяется возможность следующего нажатия по шарику
    /// </summary>
    float nextUsage;
    public float firstDelay = 1.8f;
    public float delay = 0.35f;


    // объект, к которому привязываются удаляемы объекты
    GameObject deletedObj;

    // игровые диалоги
    GameObject panel1;
    GameObject panelConfirmExit;

    // кнока "назад"
    GameObject btnGoBack;

    // хранятся очки и количество ходов
    int goal = 0;
    public int shags = 15;


    GameObject goalLabel, shagLabel;

    void Start()
    {


        // В эту переменную записывается время , после которого возможно следующее нажатие на шарик
        nextUsage = Time.time + firstDelay;


        // создаем объект, через который будет удалять шарики
        //		deletedObj = new GameObject();
        //		deletedObj.transform.position = new Vector2(0,0);
        deletedObj = GameObject.Find("deleteObj");


        //		// запуск метода по создания игрового поля
        initGameField();

        // получаем досту к элементам худа которые выводят количество очков и ходов, и заполняем их
        goalLabel = GameObject.Find("GoalLabel") as GameObject;
        shagLabel = GameObject.Find("ShagLabel") as GameObject;
        goalLabel.GetComponent<Text>().text = goal + "";
        shagLabel.GetComponent<Text>().text = shags + "";

        // пока что отключаем диалог gameover
        //panel1 = GameObject.Find("PanelGameOver") as GameObject;
        //panel1.SetActive(false);




    }

    void Awake()
    {




    }




    void Update()
    {
        // переменные через которые мы будем получать шарик, по которому нажал игрока
        Vector2 worldPoint;
        RaycastHit2D hit;


        // проверяется, первый ли это запуск игрового экрана
        if (GameManager.Instance.firstGame)
        {
            // Проверяется можно ли нажать по полю
            if (Input.GetMouseButtonUp(0) && Time.time > nextUsage && GameManager.Instance.activeGame)
            {

                // получаем объект, по которому нажимал игрок
                worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                // проверяем, обучающий ли это шарик, и если да, отключаем анимацию мигания, выключаем режим обучения и запускаем метод для уничтожения шариков
                if (hit.collider != null && hit.transform.GetComponent<circle_controller>().firsStart == true)
                {
                    hit.transform.GetComponent<circle_controller>().firsStartOff();
                    GameManager.Instance.firstGame = false;
                    StartCoroutine(runNormalGame(hit));
                }
            }


        }
        // проверяется нажатие по полю, если уже идет обычный режим
        else if (Input.GetMouseButtonUp(0) && Time.time > nextUsage && GameManager.Instance.activeGame)
        {

            // получаем объект, по которому нажимал игрок
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // проверяем, существует ли объект и если да, запускаем метод уничтожения
            if (hit.collider != null)
            {
                gameNormal(hit);
            }
        }







    }

    // функция которая запускается при нажатии по обучающему шарику, чтобы дать время на отключения мигания, выключения текста подсказки, и запуск обычной проверки на уничтожения шариков
    IEnumerator runNormalGame(RaycastHit2D hit)
    {
        yield return new WaitForSeconds(0.15f);
        GameObject helptxt = GameObject.Find("HelpTXT") as GameObject;
        helptxt.SetActive(false);
        gameNormal(hit);
    }


    // метод для обычного режима игры
    void gameNormal(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            // если игрок нажал по шарику, то вызываются методы, проверяющие, совпадаюти шарики или нет 
            testRight(hit);
            testLeft(hit);

            // есди 3 и больше шарика, то запускается первый метод
            if (deletedObj.transform.childCount > 1)
            {
                StartCoroutine(deleteAll(hit));
            }

            // если шарик один, запускается второй метод
            else
            {
                StartCoroutine(deleteOne(hit));
            }

            // устанавливается время, после которого можно снова нажать по шарику
            nextUsage = Time.time + delay;
        }
    }



    // метод, удаляющий все шарики в линии, если их 3  и больше
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


    //метод, удаляющий только выбранный шарик, если их набралось меньше трех одинаковых
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

    // подсчет текущих очков в игре
    void addPoints(int addP)
    {
        goal = goal + addP;
        goalLabel.GetComponent<Text>().text = goal + "";
    }

    // подсчет оставшихся ходов в игре
    void calcShags(int shag)
    {
        shags = shags + shag;
        shagLabel.GetComponent<Text>().text = shags + "";
        if (shags < 1) gameOver();
    }


    // метод, который решает, показать таблицу рекордов при новом рекорде, или просто выкинуть в меню в случае проигрыша
    void gameOver()
    {
    //    LoadRecords lrec = new LoadRecords();
    //    lrec.initRecScreen();
    //    List<GoalStr> recList = lrec.getList();
    //    if (goal > recList[0].Goals_)
    //    {

    //        int j = 0;
    //        while (PlayerPrefs.HasKey("game_" + j))
    //        {
    //            j++;
    //        }
    //        PlayerPrefs.SetString("game_" + j, DateTime.Now + ";" + goal);
    //        GameManager.Instance.showNewScrorec = true;
            SceneManager.LoadScene("Game");
        //}
        //else
        //{
        //    GameManager.Instance.activeGame = false;
        //    btnGoBack = GameObject.Find("ButtonGoBack");
        //    btnGoBack.SetActive(false);
        //    panel1.SetActive(true);
        //}

    }

    // создание верхнего (шестого) ряда шариков, которые падают вниз,если какой либо из шариков сгорает
    void respawnNewCircles()
    {
        float x = -4.2f, y = 6f;
    //public float startX = -4.2f;
    //public float startY = -0.2f;
        //float x = this.startX, y = this.startY;

        for (int i = 0; i < 5; i++)
        {
            Vector2 worldPoint = new Vector2(x + i, y);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider == null)
            {
                setRNDCircle(x + i, y);
            }

        }
    }


    // проверка, есть ли справа от выбранного шарика, такие же шарики
    void testRight(RaycastHit2D hit)
    {
        RaycastHit2D hitNew;
        hit.collider.enabled = false;
        hitNew = Physics2D.Raycast(hit.transform.position, hit.transform.right);
        hit.collider.enabled = true;
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hit = hitNew;
                hit.collider.enabled = false;
                hitNew = Physics2D.Raycast(hit.transform.position, hit.transform.right);
                hit.collider.enabled = true;
            }
        }

    }

    // проверка, есть ли слева от выбранного шарика, такие же шарики
    void testLeft(RaycastHit2D hit)
    {
        RaycastHit2D hitNew;
        hit.collider.enabled = false;
        hitNew = Physics2D.Raycast(hit.transform.position, -hit.transform.right);
        hit.collider.enabled = true;
        for (int i = 0; i < 4; i++)
        {
            if (hitNew.collider != null && hit.collider.name == hitNew.collider.name)
            {
                hitNew.transform.parent = deletedObj.transform;
                hit = hitNew;
                hit.collider.enabled = false;
                hitNew = Physics2D.Raycast(hit.transform.position, -hit.transform.right);
                hit.collider.enabled = true;
            }
        }

    }



    // задание координат для генерации первоначальной сетки шариков
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

    // генерация случайного шарика в указанных координатах
    void setRNDCircle(float x, float y)
    {
        int rndN = UnityEngine.Random.Range(1, 5);

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
