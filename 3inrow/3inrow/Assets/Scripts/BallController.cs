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

    private List<BallController> _vertical;
    private List<BallController> _horizontal;

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

    void OnMouseDrag()
    {
        if (!_isDragging)
        {

            //_defaultPosition = transform.position;
            //_selectedPosition = transform.position;
            _isDragging = true;
            _collider.enabled = false;
            var up = Physics2D.RaycastAll(FinalPosition, Vector2.up);
            var down = Physics2D.RaycastAll(FinalPosition, Vector2.down);
            var left = Physics2D.RaycastAll(FinalPosition, Vector2.left);
            var right = Physics2D.RaycastAll(FinalPosition, Vector2.right);
            _horizontal = left.Union(right).Select(g => g.collider.gameObject.GetComponent<BallController>()).ToList();
            _vertical = up.Union(down).Select(g => g.collider.gameObject.GetComponent<BallController>()).ToList();
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
                if (_horizontal.Exists(g => g.Id == controller.Id) && !controller.IsMoving)
                {
                    _vertical.ForEach(g => g.Revert());
                    SetBufferPosition(controller, hit);

                }
                if (_vertical.Exists(g => g.Id == controller.Id) && !controller.IsMoving)
                {
                    _horizontal.ForEach(g => g.Revert());
                    SetBufferPosition(controller, hit);
                }
            }
        }
    }

    private void Revert()
    {
        if (!IsMoving)
        {
            BufferPositon = FinalPosition;
            IsMoving = true;
        }
    }

    private void SetBufferPosition(BallController controller, RaycastHit2D hit)
    {
        var tmp = controller.BufferPositon;
        Debug.Log(string.Format(" tmp {0} / {1} / {2}", tmp, hit.collider.gameObject.transform, BufferPositon));
        controller.MoveToPosition(BufferPositon);
        BufferPositon = tmp;
    }

    private void MoveToPosition(Vector3 position)
    {
        if (!IsMoving)
        {
            BufferPositon = position;
            IsMoving = true;
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
