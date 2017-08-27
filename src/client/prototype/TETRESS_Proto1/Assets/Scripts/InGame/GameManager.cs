using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager Instance { get { if (_instance == null) _instance = GameObject.FindObjectOfType<GameManager>(); return _instance; } }

	private void Awake()
	{

	}

	public HashSet<Unit> Units { get; set; } = new HashSet<Unit>();

	private void Update()
	{
	}
}