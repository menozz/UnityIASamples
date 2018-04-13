using UnityEngine;

public class circle_controller : MonoBehaviour
{
	private Animator anim;
    private Rigidbody2D _rigidbody;

    void Start ()
    {
		anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.drag = -0.5f;
       // _rigidbody.inertia = 0f;
    }

    void FixedUpdate()
    {
        //_rigidbody.AddForce(new Vector2(0, -0.05f));
        //_rigidbody.velocity = Vector2.zero;

        GetComponent<Rigidbody2D>().velocity = Vector2.down * 1f;
        //GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.11f, ForceMode2D.Force);
        // GetComponent<Rigidbody>().velocity = Vector3.down * jumpVelocity;

    }

    void OnCollisionEnter2D(Collision2D other)
    {
       // GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var t = 0;
    }


    public void destroyed(){
		anim.SetBool("isClicked", true);
	}

}
