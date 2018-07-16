using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [ReadOnly]
    public int Id;
    [ReadOnly]
    public Vector3 RespawnPosition;
    [ReadOnly]
    public Vector3 FinalPosition;
    [ReadOnly]
    public int MainScrollSpeed;
    [ReadOnly]
    public int UnitReturnSpeed;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private bool IsDragging = false;
    private Vector3 _beforeDragPosition;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform.position = RespawnPosition;
        StartCoroutine(Movement(_transform, FinalPosition, MainScrollSpeed));
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
    }

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!IsDragging)
        {
            _beforeDragPosition = transform.position;
            IsDragging = true;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        curPosition.z = -1f;
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        if (IsDragging)
        {
            IsDragging = false;
            StartCoroutine(Movement(_transform, _beforeDragPosition, UnitReturnSpeed));
        }
    }
}
