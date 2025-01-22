using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CapsuleCollider cd;
    Rigidbody rb;

    [SerializeField] public float GroundMovementSpeed = 5f;
    [SerializeField] public float WaterMovementSpeed = 2.5f;

    // Start is called before the first frame update
    void Awake()
    {
        cd = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
