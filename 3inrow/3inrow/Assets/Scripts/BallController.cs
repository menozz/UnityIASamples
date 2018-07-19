using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public int Id;
    public Vector3 RespawnPosition;
    public Vector3 FinalPosition;
    public float MainScrollSpeed;
    public float UnitReturnSpeed;
    public float SwapSpeed;

    public GameManager GameManager;

    private Transform _transform;
    private Vector3 _screenPoint;
    private Vector3 _offset;

    private bool _isDragging;
    private bool _isSwapEnable;

     public Vector3 BufferPositon;

    public bool IsMoving;

    private float distance = 3f;
    private Collider2D _collider;


    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        _transform.position = RespawnPosition;
        IsMoving = true;
        BufferPositon = FinalPosition;
        StartCoroutine(Movement(_transform, BufferPositon, MainScrollSpeed));
    }

    //void Stop()
    //{
    //    StopCoroutine("Movement");
    //}

    void OnMouseDown()
    {
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        _offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(CurScreenPoint());
    }

    //void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.yellow;
    //    //Gizmos.DrawRay(transform.position, Vector3.forward);
    //}

    //void OnMouseEnter()
    //{
    //    if (!_isDragging)
    //    {
    //        Debug.Log("not draging, mouse enter " + transform);
    //    }
    //}

    //void OnMouseExit()
    //{

    //    if (!_isDragging)

    //    {

    //        Debug.Log("not draging, mouse exit " + transform);

    //    }

    //}

    void FixedUpdate()
    {
      
    }
    void Update()
    {
        if (IsMoving)
        {
            StartCoroutine(Movement(_transform, BufferPositon, SwapSpeed));
        }
    }

    void OnMouseUp()
    {
        if (_isDragging)
        {
            _isDragging = false;
            _collider.enabled = true;
            StartCoroutine(Movement(_transform, FinalPosition, UnitReturnSpeed));
            BufferPositon = FinalPosition;
            GameManager.VerticalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
            GameManager.HorizontalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
        }
    }

    void OnMouseDrag()
    {
        if (!_isDragging)
        {
            _isDragging = true;
            _collider.enabled = false;
            GameManager.MakeVertHorz(FinalPosition);
            //GameManager.VerticalUnits.ForEach(g => g.SetBufferToFinalPosition());
            //GameManager.HorizontalUnits.ForEach(g => g.SetBufferToFinalPosition());
        }

        var curPosition = Camera.main.ScreenToWorldPoint(CurScreenPoint()) + _offset;
        curPosition.z = -1;
        transform.position = curPosition;

        if (_isDragging)
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.zero);
            if (hit.collider != null)
            {
                var controller = hit.collider.gameObject.GetComponent<BallController>();
                //if (!controller.IsMoving)
                {
                    if (GameManager.HorizontalUnits.Exists(g => g.GetComponent<BallController>().Id == controller.Id))
                    {

                        GameManager.VerticalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
                        //GameManager.HorizontalUnits.ForEach(g => g.SetBufferToFinalPosition());
                        GameManager.SwapBufferPosition(this, controller);
                        //   SetBufferPosition(controller, hit);

                    }
                    else if (GameManager.VerticalUnits.Exists(g => g.GetComponent<BallController>().Id == controller.Id))
                    {
                        GameManager.HorizontalUnits.ForEach(g => g.GetComponent<BallController>().SetBufferToFinalPosition());
                        //GameManager.VerticalUnits.ForEach(g => g.SetBufferToFinalPosition());
                        GameManager.SwapBufferPosition(this, controller);
                        //   SetBufferPosition(controller, hit);
                    }
                    else
                    {
                        //GameManager.CleanPositions();
                    }
                }
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        if (!IsMoving)
        {
            BufferPositon = position;
            IsMoving = true;
        }
    }

    public void Initialize(GameManager gameManager, int id, Vector2 final, Vector2 spawn)
    {
        GameManager = gameManager;
        Id = id;
        FinalPosition = final;
        RespawnPosition = spawn;
        MainScrollSpeed = GameManager.MainScrollSpeed;
        UnitReturnSpeed = GameManager.UnitReturnSpeed;
        SwapSpeed = GameManager.SwapSpeed;
        GameManager.Units.Add(id, gameObject);
    }

    public void SetBufferToFinalPosition()
    {
        if (!_isDragging) 
        {
            if (!(Vector3.SqrMagnitude(BufferPositon - FinalPosition) < 0.0001))
            {
                BufferPositon = FinalPosition;
                IsMoving = true;
            }
        }
    }

    public IEnumerator Movement(Transform currentTransform, Vector3 finalPosition, float speed)
    {
        float sqrRemainingDistance = (currentTransform.position - finalPosition).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon && IsMoving)
        {
            Vector2 towards = Vector2.MoveTowards(currentTransform.position, finalPosition, speed * Time.deltaTime);
            currentTransform.position = towards;
            sqrRemainingDistance = (currentTransform.position - finalPosition).sqrMagnitude;
            yield return null;
        }

        IsMoving = false;
    }

    private Vector3 CurScreenPoint()
    {
        // return Input.mousePosition;
        return new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
    }
}

public static class Helper
{
    public static void Controller(this GameObject gameObject, Action<BallController> action)
    {
        action(gameObject.GetComponent<BallController>());
    }
}
