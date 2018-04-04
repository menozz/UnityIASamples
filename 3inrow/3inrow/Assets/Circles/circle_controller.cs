using UnityEngine;

public class circle_controller : MonoBehaviour
{
	private Animator anim;

    void Start ()
    {
		anim = GetComponent<Animator>();

	}
	
	public void destroyed(){
		anim.SetBool("isClicked", true);
	}
}
