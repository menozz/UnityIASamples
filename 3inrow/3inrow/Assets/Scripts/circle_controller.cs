using System.Collections;
using UnityEngine;

public class circle_controller : MonoBehaviour
{
    public Animator animator;
    public float transitionDuration = 0.0f;
    private int idle = 0;
    public int durationToChange = 0;
    public int durationThreshold = 200;
    public string idleString = "units_idle_01";
    public string mousemoveString = "units_moved_01";
    public string selectString = "units_select_01";
    public string Inverted = "unit_mouse_inverted_01";
    private bool mouseOn = false;
    private bool busy = false;

    void Start()
    {
        if (animator == null) animator = this.gameObject.GetComponent<Animator>();
    }

    //void OnEnable()
    //{
    //   // animator.Play(Inverted);
    //}

    void OnMouseEnter()
    {
        mouseOn = true;
    }

    void OnMouseOver()
    {
        mouseOn = true;
        //if (!mouseOn)
        //{
        //   // mouseOn = true;
        //    animator.Play(Inverted);
        // //   mouseOn = false;
        //}
    }

    void OnMouseExit()
    {
        mouseOn = false;
        //animator.Play(idleString);
        //   mouseOn = false;
    }

    void Update()
    {
        if (!mouseOn && !busy)
        {
            StartCoroutine(IdlePlay(idleString));
           // IdlePlay(idleString);
        }
        if(mouseOn && !busy)
        {
            StartCoroutine(IdlePlay(Inverted));
            //IdlePlay(Inverted);
         //   StartCoroutine(IdlePlay(idleString));
            mouseOn = false;
        }
        //if (Player.S.isGrounded)
        //{
        //    if (Actions.S.forward > 0 || Actions.S.backward < 0)
        //    {
        //        if (Actions.S.run > 0)
        //        {
        //            //  animator.CrossFade("Run", transitionDuration);
        //        }
        //        else
        //        {
        //            //  animator.CrossFade("Walk", transitionDuration);
        //        }
        //    }
        //    else
        //    {
        //        //animator.CrossFade(idleString, transitionDuration);
        //    }
        //}
        //else
        //{
        //    //animator.CrossFade("Jump", transitionDuration);
        //}

        //durationToChange++;
        //if (durationToChange >= durationThreshold)
        //{
        // //   idleString = chooseIdle();
        //    durationToChange = 0;
        //}
    }

    private IEnumerator IdlePlay(string name)
    {
        busy = true;
        animator.Play(name);
        yield return new WaitForSeconds(0.0f);
        busy = false;

    }

    //string chooseIdle()
    //{
    //    idle = (idle + 1) % 3;
    //    return "Idle" + idle;
    //}
    //Animator anim;
    //   public AnimationClip animationClip;
    //   public Animation animation;

    ////   GameObject g;

    //   void Start ()
    //   {

    //       anim = GetComponent<Animator>();
    //       //g = new GameObject();
    //       gameObject.AddComponent<Animation>();
    //       animation = gameObject.GetComponent<Animation>();

    //       animationClip = GetAnimationClipFromAnimatorByName(anim, "units_moved_01");

    //       animation.AddClip(animationClip, "units_moved_01");
    //       animation.Play();

    //       // var anumation = GetComponent<Animation>();
    //       //_rigidbody.drag = -0.5f;
    //       // _rigidbody.inertia = 0f;
    //       //        anumation.Play("units_moved_01");
    //       //  a
    //       //var y = animation["units_idle_01"];
    //       var u = 0;
    //   }

    //   internal AnimationClip GetAnimationClipFromAnimatorByName(Animator anim, string name)
    //   {
    //       //can't get data if no animator
    //       if (anim == null)
    //           return null;

    //       //favor for above foreach due to performance issues
    //       for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
    //       {
    //           if (anim.runtimeAnimatorController.animationClips[i].name == name)
    //               return anim.runtimeAnimatorController.animationClips[i];
    //       }

    //       Debug.LogError("Animation clip: " + name + " not found");
    //       return null;
    //   }


    //   //void FixedUpdate()
    //   //{
    //   //    //_rigidbody.AddForce(new Vector2(0, -0.05f));
    //   //    //_rigidbody.velocity = Vector2.zero;

    //   //    //GetComponent<Rigidbody2D>().velocity = Vector2.down * 1f;
    //   //    //GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.11f, ForceMode2D.Force);
    //   //    // GetComponent<Rigidbody>().velocity = Vector3.down * jumpVelocity;

    //   //}

    //   //void OnCollisionEnter2D(Collision2D other)
    //   //{
    //   //   // GetComponent<Rigidbody2D>().isKinematic = true;
    //   //}

    //   //void OnTriggerEnter2D(Collider2D other)
    //   //{
    //   //    var t = 0;
    //   //}

    //   public void MouseMove(bool flag)
    //   {
    //      // animator.SetBool("MouseMove", true);
    //   }

    //   void OnMouseEnter()
    //   {
    //       //Debug.Log("I am over something");
    //       //animator.SetBool("MouseMove", true);
    //   }
    public void MouseMove()
    {
        mouseOn = true;
    }
}
