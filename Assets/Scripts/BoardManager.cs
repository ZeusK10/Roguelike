using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace ZeuskGames
{
	public class BoardManager : MonoBehaviour
	{
		[SerializeField] private int width;
		[SerializeField] private int height;
		[SerializeField] private Tile[] groundTiles;
		[SerializeField] private Tile[] wallTiles;
		private Tilemap _tilemap;
		private CellData[,] _boardData;

		private void Start()
		{
			_tilemap = GetComponentInChildren<Tilemap>();
			_boardData = new CellData[width, height];
			for (int i = 0; i < height; ++i)
			{
				for (int j = 0; j < width; ++j)
				{
					Tile tile;
					_boardData[j,i] = new CellData();
					if (i == 0 || j == 0 || j == width - 1 || i == height - 1)
					{
						tile = wallTiles[Random.Range(0, wallTiles.Length)];
						_boardData[j, i].passable = false;
					}
					else
					{
						tile = groundTiles[Random.Range(0, groundTiles.Length)];
						_boardData[j, i].passable = true;
					}
					_tilemap.SetTile(new Vector3Int(j,i,0),tile);
				}
			}
		}
	}

	public class CellData
	{
		public bool passable;
	}
}

