using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public GameObject Shooter { get; set; } = null;

	private void Start()
	{
		Destroy(gameObject, 10.0f);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject != Shooter)
			Destroy(gameObject);
	}
}
