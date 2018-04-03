using UnityEngine;
using System.Collections;

public class circle_controller : MonoBehaviour {

	// вспомогательный скрипт, для управления шариком на игровом поле


	private Animator anim;
	public bool firsStart = false;

	// Use this for initialization
	void Start () {

		// получаем аниматор шарика
		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

	}

	private void FixedUpdate()
	{

	}

	// метод вызывается, когда необходимо визуально отобразить уничтожение(взрыв) шарика
	public void destroyed(){
		anim.SetBool("isClicked", true);

	}

	// метод вызывается, если необходимо чтобы шарик начал мигать при первом запуске игры
	public void firsStartOn(){
		anim.SetBool("firstStart", true);
		firsStart = true;

	}

	// метод вызывается, когда нужно чтобы первый шарик перестал мигать
	public void firsStartOff(){
		anim.SetBool("firstStart", false);
		firsStart = false;
	}
}
