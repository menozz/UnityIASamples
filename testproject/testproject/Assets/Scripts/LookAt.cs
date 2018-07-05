//--------------------------------------------
using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
//--------------------------------------------
public class LookAt : MonoBehaviour
{
    //Cached transform
 //   private Transform ThisTransform = null;
    //Target to look at
    public Transform Target = null;
    //Rotate speed
    public float RotateSpeed = 1f;
    //--------------------------------------------
    // Use this for initialization
    void Awake()
    {
       
        //Get transform for this object
       // ThisTransform = GetComponent<Transform>();
    }
    //--------------------------------------------
    void Start()
    {
        //Start tracking target
        StartCoroutine(TrackRotation(Target));
    }
    //--------------------------------------------
    //Coroutine for turning to face target
    IEnumerator TrackRotation(Transform Target)
    {
        //Loop forever and track target
        while (true)
        {
            if (Target != null)
            {
                //Get direction to target
                Vector3 relativePos = Target.position - transform.position;
                //Calculate rotation to target
                Quaternion NewRotation = Quaternion.LookRotation(relativePos);
                //Rotate to target by speed
                transform.rotation = Quaternion.RotateTowards(transform.rotation, NewRotation, RotateSpeed * Time.deltaTime);
            }
            //wait for next frame
            yield return null;
        }
    }
    //--------------------------------------------
    //Function to draw look direction in viewport
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.up.normalized );
    }
    //--------------------------------------------
}
//--------------------------------------------