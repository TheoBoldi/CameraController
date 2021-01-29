using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float speed = 10.0f;
	public float yaw = 0;
	public float yawSpeed = 100;

	Rigidbody _rigidbody = null;
	protected bool IsActive { get; private set; }

	public void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate()
    {
		Vector3 direction = Vector3.zero;
		direction += Input.GetAxisRaw("Horizontal") * transform.right;
		direction += Input.GetAxisRaw("Vertical") * transform.forward;
		direction.Normalize();
		_rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
	}

    private void Update()
    {
		float rotation = Input.GetAxis("Mouse X");

		yaw += rotation * yawSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, yaw, 0);
	}
}
