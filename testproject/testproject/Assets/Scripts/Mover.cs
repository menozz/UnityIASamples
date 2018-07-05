
using System;
using UnityEngine;

namespace Assets.Scripts 
{
    public class Mover : MonoBehaviour
    {
        [NonSerialized]
        public float Speed = 2f;

        public Vector3 Direction = Vector3.zero;
        public AnimationCurve AnimCurve;

        void Start()
        {
        }
        void Update()
        {
         //   Transform ThisTransform = GetComponent<Transform>();
            transform.position += Direction.normalized * AnimCurve.Evaluate(Time.time) * Speed * Time.deltaTime;
           
        }
    }
}
