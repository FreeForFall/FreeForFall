using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
	private int _loadedCount;
	private int _lostCount;
	private string _mapName;

	public GameEngine (string name)
	{
		_loadedCount = 0;
		_lostCount = 0;
		_mapName = name;
	}
}
