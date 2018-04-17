using UnityEngine;

public class circle_controller : MonoBehaviour
{
	private Animator animator;

    void Start ()
    {
		animator = GetComponent<Animator>();
        var anum = GetComponent<Animation>();
        //_rigidbody.drag = -0.5f;
        // _rigidbody.inertia = 0f;
        //var animation = GetAnimationClipFromAnimatorByName(animator, "units_idle_01");
        //var y = animation["units_idle_01"];
        var u = 0;
    }

    internal AnimationClip GetAnimationClipFromAnimatorByName(Animator anim, string name)
    {
        //can't get data if no animator
        if (anim == null)
            return null;

        //favor for above foreach due to performance issues
        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (anim.runtimeAnimatorController.animationClips[i].name == name)
                return anim.runtimeAnimatorController.animationClips[i];
        }

        Debug.LogError("Animation clip: " + name + " not found");
        return null;
    }


    //void FixedUpdate()
    //{
    //    //_rigidbody.AddForce(new Vector2(0, -0.05f));
    //    //_rigidbody.velocity = Vector2.zero;

    //    //GetComponent<Rigidbody2D>().velocity = Vector2.down * 1f;
    //    //GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.11f, ForceMode2D.Force);
    //    // GetComponent<Rigidbody>().velocity = Vector3.down * jumpVelocity;

    //}

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //   // GetComponent<Rigidbody2D>().isKinematic = true;
    //}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    var t = 0;
    //}

    public void MouseMove(bool flag)
    {
        animator.SetBool("MouseMove", true);
    }

    void OnMouseEnter()
    {
        //Debug.Log("I am over something");
        //animator.SetBool("MouseMove", true);
    }
}
