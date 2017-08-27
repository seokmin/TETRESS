using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour
{
	private Rigidbody2D _rigidBody = null;

	[SerializeField]
	private SpriteRenderer _spriteRenderer = null;

	[SerializeField]
	private Transform _firePoint = null;
	[SerializeField]
	private GameObject _selectedMarker = null;

	[SerializeField]
	private Bullet _bulletSource = null;

	private bool _isSelected = false;
	public bool IsSelected
	{
		get { return _isSelected; }
		set { _isSelected = value; _selectedMarker.SetActive(value); }
	}

	public enum DirectionType
	{
		kLeft = 0,
		kRight = 1,

	}

	public DirectionType Direction { get; set; } = DirectionType.kLeft;

	public float Sight { get; set; } = 6.0f;

	// Use this for initialization
	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		GameManager.Instance.Units.Add(this);
	}

	// Update is called once per frame
	private void Update()
	{
		if (false == IsSelected)
			return;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			_rigidBody.MovePosition(_rigidBody.position + Vector2.left * 0.1f);
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			Direction = DirectionType.kLeft;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			_rigidBody.MovePosition(_rigidBody.position + Vector2.right * 0.1f);
			transform.localRotation = Quaternion.Euler(0, 180, 0);
			Direction = DirectionType.kRight;
		}

		var directionVec = _spriteRenderer.transform.rotation * new Vector3(-1, 0, 0);
		var upperRay = Quaternion.Euler(0, 0, +Sight / 2.0f) * directionVec * 100.0f;
		var downRay = Quaternion.Euler(0, 0, -Sight / 2.0f) * directionVec * 100.0f;

		Debug.DrawRay(_firePoint.position, upperRay);
		Debug.DrawRay(_firePoint.position, downRay);

		var spriteTransform = _spriteRenderer.transform;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			spriteTransform.Rotate(new Vector3(0, 0, -0.5f));
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			spriteTransform.Rotate(new Vector3(0, 0, +0.5f));
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(shoot(upperRay, downRay));
		}
	}

	private IEnumerator shoot(Vector3 upper, Vector3 lower)
	{
		Action shootAction = () =>
		{

			var bullet = GameObject.Instantiate(_bulletSource);
			bullet.Shooter = gameObject;
			bullet.transform.position = _firePoint.position;

			var direction = new Vector3(UnityEngine.Random.Range(lower.x, upper.x), UnityEngine.Random.Range(lower.y, upper.y), 0);
			bullet.transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, direction));
			var bulletBody = bullet.GetComponent<Rigidbody2D>();
			bulletBody.AddForce(direction.normalized * 1000);
		};

		for(int i=0; i<8;++i)
		{
			shootAction();
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void OnMouseDown()
	{
		Debug.Log("Unit clicked!");
		GameManager.Instance.Units.Foreach(eachUnit =>
		{
			eachUnit.IsSelected = eachUnit.GetInstanceID() == this.GetInstanceID();
		});
	}
}


public static class IEnumerableExtension
{
	public static void Foreach<T>(this IEnumerable<T> me, Action<T> action)
	{
		foreach(T instance in me)
		{
			action(instance);
		}
	}
}