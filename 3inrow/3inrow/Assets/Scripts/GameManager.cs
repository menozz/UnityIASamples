using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	// Скрипт, который хранит некоторые общие переменные для всей игры
	public static GameManager Instance { get; private set; }

	// показать или нет новый рекорд на экране рекордов
	public bool showNewScrorec;

	// переменная управляет возможностью нажимать или нет по шарикам
	public bool activeGame;

	// переменная, которая устанавливается в true, чтобы при запуске новой игры запускался режим обучения
	public bool firstGame=true;

	// координаты, необходимые чтобы основной игровой скрипт GameGenField мог понять, какой шарик выбран как обучающий
	public float fX;
	public float fY;

	public void Awake()
	{
		Instance = this;

	}


}
