using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}
	// Update is called once per frame
	private void LateUpdate()
	{
		var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		if (mousePos.x > 0.999f)
			transform.position += Vector3.right * 0.2f;
		else if (mousePos.x < 0.001f)
			transform.position += Vector3.left * 0.2f;
	}
}
