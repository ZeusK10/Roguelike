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
		[SerializeField] private List<WallObject> wallPrefabs;
		[SerializeField] private ExitCellObject exitCellPrefab;
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
			
			Vector2Int endCoord = new Vector2Int(width - 2, height - 2);
			AddObject(Instantiate(exitCellPrefab), endCoord);
			_emptyCells.Remove(endCoord);
			
			GenerateWall();
			GenerateFood();
		}

		private void GenerateWall()
		{
			int wallCount = Random.Range(6, 10);
			for (int i = 0; i < wallCount; ++i)
			{
				int randomIndex = Random.Range(0, _emptyCells.Count);
				Vector2Int coord = _emptyCells[randomIndex];

				_emptyCells.RemoveAt(randomIndex);
				int wallIndex = Random.Range(0, wallPrefabs.Count);
				WallObject newWall = Instantiate(wallPrefabs[wallIndex]);
				AddObject(newWall,coord);
			}
		}
		
		private void GenerateFood()
		{
			int foodCount = Random.Range(0,_emptyCells.Count/2);
			for (int i = 0; i < foodCount; ++i)
			{
				int randomIndex = Random.Range(0, _emptyCells.Count);
				Vector2Int coord = _emptyCells[randomIndex];
				_emptyCells.RemoveAt(randomIndex);
				int foodIndex = Random.Range(0, foodPrefabs.Count);
				FoodObject newFood = Instantiate(foodPrefabs[foodIndex]);
				AddObject(newFood,coord);
			}
		}

		public Tile GetCellTile(Vector2Int cellIndex)
		{
			return _tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x,     cellIndex.y, 0));
		}
		
		private void AddObject(CellObject obj, Vector2Int coord)
		{
			CellData data = _boardData[coord.x, coord.y];
			obj.transform.position = CellToWorld(coord);
			data.containedObject = obj;
			obj.Init(coord);
		}
		
		public void Clean()
		{
			if(_boardData == null)
				return;


			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					var cellData = _boardData[x, y];

					if (cellData.containedObject != null)
					{
						Destroy(cellData.containedObject.gameObject);
					}

					SetCellTile(new Vector2Int(x,y), null);
				}
			}
		}
		
		public void SetCellTile(Vector2Int cellIndex, Tile tile)
		{
			_tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
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

