﻿using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public enum UnitState
{
    Idle,
    Selected,
    Dragged,
    //   MouseDown,
    //  Moved,
    //  MoveEnded
}

public class UnitController : MonoBehaviour
{

    public delegate void CallBack(UnitController obj);
    private CallBack onMouseDown;
    private CallBack onMouseDrag;
    private CallBack onMouseUP;
    private CallBack onMouseEnter;
    //public Vector3 offset;

    public Animator animator;
    private string _name = Guid.NewGuid().ToString("N");
    private string OnIdleAnim = "units_idle_01";
    private string OnMouseMoveAnim = "units_moved_01";
    private string OnMouseDragAnim = "units_select_01";
    private string OnSelectedAnim = "unit_mouse_inverted_01";
    private UnitState State = UnitState.Idle;

    private RaycastHit2D srcDrag;
    //private Vector3 newPos;
    private Vector3 screenPoint;
    //private Vector3 offset;
    private Collider currentCollider;

    public void addMouseDown(CallBack callback)
    {
        onMouseDown += callback;
    }
    public void addMouseDrag(CallBack callback)
    {
        onMouseDrag += callback;
    }
    public void addMouseUp(CallBack callback)
    {
        onMouseUP += callback;
    }

    public void addMouseEnter(CallBack callback)
    {
        onMouseEnter += callback;
    }


    void Start()
    {
        if (animator == null)
        {
            animator = this.gameObject.GetComponent<Animator>();
        }
        if (currentCollider == null)
        {
            currentCollider = this.gameObject.GetComponent<Collider>();
        }
    }

    void OnMouseEnter()
    {
        //animator.SetBool("MouseMove", true);
        State = UnitState.Selected;
        onMouseEnter(this);
        //animator.SetBool("Drag", false);

        //if (State != UnitState.MoveEnded)
        //{
        //    State = UnitState.Selected;
        //}

        //Debug.Log(string.Format("mouse enter {0}",_name));
    }

    void OnMouseDown()
    {
        //animator.SetBool("Drag", true);
        //animator.SetBool("MouseMove", false);
        State = UnitState.Dragged;

       // offset = transform.position; //- Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        onMouseDown(this);

        //  animator.SetBool("MouseMove", false);

        //Debug.Log(string.Format("mouse down {0}", _name));

        ////Vector2 srcworldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);


        //offset = screenPoint - Camera.main.ScreenToWorldPoint(
        //             new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //Debug.Log(string.Format("offset {0} {1}", _name, offset));
        //if (State == UnitState.Selected)
        //{
        //    State = UnitState.MouseDown;

        //    //Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        srcDrag = hit;
        //    }
        //}

    }


    void OnMouseUp()
    {
        //animator.SetBool("Drag", false);
        //animator.SetBool("MouseMove", false);
        State = UnitState.Idle;
        onMouseUP(this);

        //animator.SetBool("Drag", false);
        //animator.SetBool("MouseMove", true);

        //    if (State != UnitState.Selected)
        //    {
        //        State = UnitState.Idle;
        //    }

        //  Debug.Log(string.Format("mouse up {0}", _name));
    }

    void OnMouseOver()
    {
        //animator.SetBool("Drag", false);
        //animator.SetBool("MouseMove", true);
        if (State == UnitState.Idle)
        {
            State = UnitState.Selected;
        }
        //   animator.SetBool("Drag", false);
        //Debug.Log(string.Format("mouse over {0}", _name));

        //State = UnitState.Selected;
        //if (!mouseOn)
        //{
        //   // mouseOn = true;
        //    animator.Play(OnSelectedAnim);
        // //   mouseOn = false;
        //}
        //   State = UnitState.Idle;
        //  Debug.Log(string.Format("mouse over {0}", _name));
    }

    void OnMouseExit()
    {
        // animator.SetBool("MouseMove", false);
        if (State == UnitState.Selected)
        {
            State = UnitState.Idle;
        }
        //animator.SetBool("Drag", false);
        //animator.SetBool("MouseMove", false);

        //if (State != UnitState.MoveEnded)
        //{
        //    State = UnitState.Idle;
        //}

        //Debug.Log(string.Format("mouse exit {0}", _name));
    }


    void OnMouseDrag()
    {
        onMouseDrag(this);
        //if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0)
        //if (Input.GetAxis("Mouse X") > 0)
        //{
        //    var srchit = Physics2D.Raycast(transform.position, Vector2.zero);
        //    if (srchit.collider != null)
        //    {
        //        var gobj = testRight(srchit);
        //        Debug.Log(gobj.name);

        //        var tmp = transform.position;
        //        var curSrc = new Vector2(transform.position.x, transform.position.y);
        //        //var newPosit = new Vector2(gobj.transform.position.x, gobj.transform.position.y);
        //        var curTrgt = new Vector2(gobj.transform.position.x, gobj.transform.position.y);
        //        transform.position = Vector2.MoveTowards(curSrc, curTrgt, 0.5f);
        //        gobj.transform.position = Vector2.MoveTowards(curTrgt, curSrc, 0.5f);

        //        //Debug.Log(gobj.transform.position);

        //        //Vector2 dstworldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        //var dsthit = Physics2D.Raycast(dstworldPoint, Vector2.zero);
        //        //if (dsthit.collider != null)
        //        //{

        //        //}
        //    }
        //}
        //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);


        //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        //transform.position = curPosition;
        //animator.SetBool("Drag", true);
        //animator.SetBool("MouseMove", false);

        //if (State == UnitState.MouseDown)
        //{
        //    if (Input.GetAxis("Mouse X") < -0.1)
        //    {
        //        Debug.Log(string.Format("axis  x < 0 {0}", _name));
        //    }

        //    if (Input.GetAxis("Mouse X") > 0.1)
        //    {
        //        State = UnitState.MouseDrag;
        //        Debug.Log(string.Format("right {0}", _name));
        //        var obj = testRight(srcDrag);
        //        if (obj != null)
        //        {
        //            var controller = obj.transform.GetChild(0).gameObject.GetComponent<UnitController>();
        //            controller.MoveTo(srcDrag.transform.position);
        //            MoveTo(obj.transform.position);
        //        }
        //    }

        //    if (Input.GetAxis("Mouse Y") > 0.1)
        //    {
        //        Debug.Log(string.Format("axis y > 0 {0}", _name));
        //    }

        //    if (Input.GetAxis("Mouse Y") < -0.1)
        //    {
        //        Debug.Log(string.Format("axis y < 0 {0}", _name));
        //    }

        //}

    }

    //public void MoveTo(Vector3 transformPosition)
    //{
    //    newPos = transformPosition;
    //    // State = UnitState.Moved;
    //}

    //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
    //    if (hit.collider != null)
    //    {
    //        gameNormal(hit);
    //    }
    GameObject testRight(RaycastHit2D hit)
    {
        var hitNew = RayCast(hit, hit.transform.right);
        //for (int i = 0; i < 4; i++)
        //{
        if (hitNew.collider != null)// && hit.collider.name == hitNew.collider.name)
        {
            return hitNew.collider.gameObject;
        }
        //}
        return null;
    }

    private RaycastHit2D RayCast(RaycastHit2D hit, Vector2 direction)
    {
        hit.collider.enabled = false;
        var hitNew = Physics2D.Raycast(hit.transform.position, direction);
        hit.collider.enabled = true;
        return hitNew;
    }

    void Update()
    {
        switch (State)
        {
            case UnitState.Selected:
                //StartCoroutine(PlayAnimation(OnSelectedAnim));
                animator.SetBool("MouseMove", true);
                animator.SetBool("Drag", false);
                break;
            case UnitState.Dragged:
                animator.SetBool("Drag", true);
                animator.SetBool("MouseMove", false);
                ///          case UnitState.MouseDown:
                //                StartCoroutine(PlayAnimation(OnMouseDragAnim));
                break;
            //case UnitState.Moved:
            //case UnitState.MoveEnded:
            //    StartCoroutine(PlayAnimation(OnMouseMoveAnim));
            //    break;
            default:
                //StartCoroutine(PlayAnimation(OnIdleAnim));
                animator.SetBool("MouseMove", false);
                animator.SetBool("Drag", false);
                break;
        }

        //StartCoroutine(Move());

        //if (!mouseOn)// && !busy)
        //{

        //    // IdlePlay(OnIdleAnim);
        //}
        //if (mouseOn)// && !busy)
        //{
        //    StartCoroutine(IdlePlay(OnSelectedAnim));
        //    //IdlePlay(OnSelectedAnim);
        //    //   StartCoroutine(IdlePlay(OnIdleAnim));
        //    mouseOn = false;
        //}
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
        //        //animator.CrossFade(OnIdleAnim, transitionDuration);
        //    }
        //}
        //else
        //{
        //    //animator.CrossFade("Jump", transitionDuration);
        //}

        //durationToChange++;
        //if (durationToChange >= durationThreshold)
        //{
        // //   OnIdleAnim = chooseIdle();
        //    durationToChange = 0;
        //}
    }

    //private IEnumerator Move()
    //{
    //    if (State == UnitState.Moved)
    //    {
    //        Debug.Log(string.Format("moved {0}", _name));
    //        var newVec = new Vector2(transform.root.position.x, transform.root.position.y);
    //        transform.root.position = Vector2.MoveTowards(newVec, newPos, 1);
    //        State = UnitState.MoveEnded;
    //        //this.transform.root

    //    }
    //    yield return new WaitForSeconds(0);
    //}

    //private void MoveToNewPosition()
    //{
    //    //if (newPos != null)
    //    //{

    //    //}
    //}

    private IEnumerator PlayAnimation(string name)
    {
        animator.Play(name);
        yield return new WaitForSeconds(0.0f);
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


    //public void MouseMove()
    //{
    //    mouseOn = true;
    //}
}
