using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [ReadOnly] public int Id;
    [ReadOnly] public Vector3 RespawnPosition;
    [ReadOnly] public Vector3 FinalPosition;
    [ReadOnly] public int MainScrollSpeed;
    [ReadOnly] public int UnitReturnSpeed;

    [ReadOnly] public GameManager GameManager;

    private int _swapSpeed = 4;

    private Transform _transform;
    private Vector3 _screenPoint;
    private Vector3 _offset;

    private bool _isDragging;
    private bool _isSwapEnable;

    //    private Vector3 _defaultPosition;
    [NonSerialized] public Vector3 BufferPositon;

    private bool IsMoving;

    private float distance = 3f;
    private Collider2D _collider;


    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        _transform.position = RespawnPosition;
        StartCoroutine(Movement(_transform, FinalPosition, MainScrollSpeed));
        BufferPositon = FinalPosition;
        //    _defaultPosition = FinalPosition;
    }

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

    void Update()
    {
        if (IsMoving)
        {
            StartCoroutine(Movement(_transform, BufferPositon, _swapSpeed));
        }
    }

    void OnMouseUp()
    {
        if (_isDragging)
        {
            _isDragging = false;
            _collider.enabled = true;
            StartCoroutine(Movement(_transform, FinalPosition, UnitReturnSpeed));
        }
    }

    void OnMouseDrag()
    {
        if (!_isDragging)
        {

            //_defaultPosition = transform.position;
            //_selectedPosition = transform.position;
            _isDragging = true;
            _collider.enabled = false;
            GameManager.MakeVertHorz(FinalPosition);
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
                if (GameManager.HorizontalUnits.Exists(g => g.Id == controller.Id) && !controller.IsMoving)
                {
                    GameManager.VerticalUnits.ForEach(g => g.SetBufferToFinalPosition());
                    GameManager.SwapBufferPosition(this, controller);
                    //   SetBufferPosition(controller, hit);

                }
                if (GameManager.VerticalUnits.Exists(g => g.Id == controller.Id) && !controller.IsMoving)
                {
                    GameManager.HorizontalUnits.ForEach(g => g.SetBufferToFinalPosition());
                      GameManager.SwapBufferPosition(this, controller);
                    //   SetBufferPosition(controller, hit);
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
        GameManager.Units.Add(id, gameObject);
    }

    private void SetBufferToFinalPosition()
    {
        if (!IsMoving && !_isDragging) 
        {
            if (!(Vector3.SqrMagnitude(BufferPositon - FinalPosition) < 0.0001))
            {
                BufferPositon = FinalPosition;
                IsMoving = true;
            }
        }
    }

    private IEnumerator Movement(Transform currentTransform, Vector3 finalPosition, int speed)
    {
        float sqrRemainingDistance = (currentTransform.position - finalPosition).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
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
