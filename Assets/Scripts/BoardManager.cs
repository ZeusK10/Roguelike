using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
		[SerializeField] private PlayerController player;
		[SerializeField] private List<FoodObject> foodPrefabs;
		private Tilemap _tilemap;
		private CellData[,] _boardData;
		private Grid _grid;
		private List<Vector2Int> _emptyCells;

		public void Init()
		{
			_tilemap = GetComponentInChildren<Tilemap>();
			_grid = GetComponentInChildren<Grid>();
			_boardData = new CellData[width, height];
			_emptyCells = new List<Vector2Int>();
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
						_emptyCells.Add(new Vector2Int(j, i));
					}
					_tilemap.SetTile(new Vector3Int(j,i,0),tile);
				}
			}

			_emptyCells.Remove(new Vector2Int(1, 1));
			GenerateFood();
		}

		private void GenerateFood()
		{
			int foodCount = Random.Range(0,_emptyCells.Count/2);
			for (int i = 0; i < foodCount; ++i)
			{
				int randomIndex = Random.Range(0, _emptyCells.Count);
				Vector2Int coord = _emptyCells[randomIndex];
				_emptyCells.RemoveAt(randomIndex);
				CellData data = _boardData[coord.x, coord.y];
				if (data.passable && data.containedObject == null)
				{
					int foodIndex = Random.Range(0, foodPrefabs.Count);
					FoodObject newFood = Instantiate(foodPrefabs[foodIndex]);
					newFood.transform.position = CellToWorld(coord);
					data.containedObject = newFood;
				}
			}
		}

		public Vector3 CellToWorld(Vector2Int cellIndex)
		{
			return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
		}
		
		public CellData GetCellData(Vector2Int cellIndex)
		{
			if (cellIndex.x < 0 || cellIndex.x >= width
			                    || cellIndex.y < 0 || cellIndex.y >= height)
			{
				return null;
			}

			return _boardData[cellIndex.x, cellIndex.y];
		}
	}

	public class CellData
	{
		public bool passable;
		public CellObject containedObject;
	}
}

