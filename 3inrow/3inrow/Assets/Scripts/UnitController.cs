using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public enum UnitState
{
    Idle,
    Selected,
    Dragged,
    Moved
}

public class UnitController : MonoBehaviour
{

    public delegate void CallBack(UnitController obj);
    private CallBack onMouseDown;
    private CallBack onMouseDrag;
    private CallBack onMouseUP;
    private CallBack onMouseEnter;

    //public bool IsDragged
    //{
    //    get
    //    {
    //        return this.State == UnitState.Dragged || this.State == UnitState.Moved;
    //    }
    //}

    public Animator animator;
    public string Name { get; private set; }
    private string OnIdleAnim = "units_idle_01";
    private string OnMouseMoveAnim = "units_moved_01";
    private string OnMouseDragAnim = "units_select_01";
    private string OnSelectedAnim = "unit_mouse_inverted_01";
    public UnitState State = UnitState.Idle;

    private RaycastHit2D srcDrag;
    private Vector3 screenPoint;
    private Collider currentCollider;

    private Vector3 swapPosition;

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

    public bool IsActive;

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

        Name = string.Format("{0}_{1:N}", this.gameObject.name, Guid.NewGuid());
    }

    void OnMouseEnter()
    {
        // State = UnitState.Selected;

        //if (!this.IsDragged)
        //{
        onMouseEnter(this);
        //}
    }

    void OnMouseDown()
    {
        //if (!this.IsDragged)
        //{
        onMouseDown(this);
        //    State = UnitState.Dragged;
        //}
    }


    void OnMouseUp()
    {
        onMouseUP(this);
        //  State = UnitState.Idle;
    }

    void OnMouseOver()
    {
        //if (State == UnitState.Idle)
        //{
        //    State = UnitState.Selected;
        //}
    }

    void OnMouseExit()
    {
        //if (State == UnitState.Selected)
        //{
        //    State = UnitState.Idle;
        //}
    }


    //void OnMouseDrag()
    //{
    //    if (!this.IsDragged)
    //    {
    //        onMouseDrag(this);
    //    }
    //}

    //GameObject testRight(RaycastHit2D hit)
    //{
    //    var hitNew = RayCast(hit, hit.transform.right);
    //    if (hitNew.collider != null)
    //    {
    //        return hitNew.collider.gameObject;
    //    }
    //    return null;
    //}

    //private RaycastHit2D RayCast(RaycastHit2D hit, Vector2 direction)
    //{
    //    hit.collider.enabled = false;
    //    var hitNew = Physics2D.Raycast(hit.transform.position, direction);
    //    hit.collider.enabled = true;
    //    return hitNew;
    //}

    void Update()
    {
        //switch (State)
        //{
        //    case UnitState.Selected:
        //        animator.SetBool("Selected", true);
        //        animator.SetBool("Drag", false);
        //        animator.SetBool("Moved", false);
        //        break;
        //    case UnitState.Dragged:
        //        animator.SetBool("Drag", true);
        //        animator.SetBool("Selected", false);
        //        animator.SetBool("Moved", false);
        //        break;
        //    case UnitState.Moved:
        //        animator.SetBool("Moved", true);
        //        animator.SetBool("Drag", false);
        //        animator.SetBool("Selected", false);
        //        break;
        //    default:
        //        animator.SetBool("Selected", false);
        //        animator.SetBool("Drag", false);
        //        animator.SetBool("Moved", false);
        //        break;
        //}

    }

    public void SetSwapPosition(Vector3 swapPos)
    {
        if (IsActive)
        {
            return;
        }

        swapPosition = swapPos;
        IsActive = true;
    }

    public bool Swap(UnitController destination)
    {
        if (!IsActive)
        {
            if (IsRayCastHit(gameObject, destination.gameObject))
            {
                SetSwapPosition(destination.transform.position);
                destination.SetSwapPosition(transform.position);
            }
            else
            {
                return false;
            }
        }

        MoveTo();
        destination.MoveTo();
        return IsActive;
    }

    private bool IsRayCastHit(GameObject source, GameObject destination)
    {
        var fromPosition = source.transform.position;
        var toPosition = destination.transform.position;
        var direction = toPosition - fromPosition;
        if (Math.Abs(direction.x) > 0.01f && Math.Abs(direction.y) > 0.01f)
        {
            return false;
        }

        if (Math.Abs(direction.x) > 1.5 || Math.Abs(direction.y) > 1.5)
        {
            return false;
        }

        
        return true;
    }

    public void MoveTo()
    {
        var destination = swapPosition;
        if (transform.position != destination)
        {
            var nextPosition = Vector3.MoveTowards(transform.position, destination, 4f * Time.deltaTime);
            transform.position = nextPosition;
            return;
        }

        IsActive = false;
    }
}
