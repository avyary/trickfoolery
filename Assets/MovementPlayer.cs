using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private float _playerInput;
    private float _rotInput;
    private Vector3 _userRot;


    private Rigidbody _rigidbody;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerInput = Input.GetAxis("Vertical");
        _rotInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() 
    {
        _rigidbody.velocity += _transform.forward * _playerInput * 0.25f;
        _userRot = _transform.rotation.eulerAngles;
        _userRot += new Vector3(0, _rotInput, 0);
        _transform.rotation = Quaternion.Euler(_userRot);
    }
}
