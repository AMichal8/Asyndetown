using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;
	public Material buildingMat;
	public Material wallMat;
	public int borderSize = 5;
	//Need to redo...
	//Need to get color info from texture, and generate map based on if color matches wall (ex. black = wall = 1, vs. white = no wall = 0)

	public int wallThresholdSize = 50;

	//public colorToPrefab[] colorMappings;
	public Color wallColor;
	int [,] level;

	[SerializeField]
	List<int[,]> regionsList = new List<int[,]> ();

	// Use this for initialization
	void Start () 
	{
		
		regionsList.Clear ();
		GenerateLevel ();
		
	}
	void GenerateLevel()
	{
		level = new int[map.width, map.height];
		//Debug.Log ("Map.width = " + map.width + ". Map.Height = " + map.height + ".");
		FillMap ();

		RemovePocketRegions ();

		//ProcessRegionsIntoMaps ();


	
		//Adds border around existing map
		int[,] borderedMap = new int[map.width + borderSize * 2, map.height + borderSize * 2];

		for (int x = 0; x <borderedMap.GetLength(0); x++) 
		{
			for (int y = 0; y < borderedMap.GetLength(1); y++) 
			{
				if (x >= borderSize && x < map.width + borderSize && y >= borderSize && y < map.height + borderSize) {
					borderedMap [x, y] = level [x - borderSize, y - borderSize];
				}
				else 
				{
					borderedMap [x, y] = 1;
				}
			}
		}

		//CreateRegionObjectsAndMeshes ();
	

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		//foreach region in list of regions, generate mesh
		meshGen.GenerateMesh (borderedMap, 1f);

	}

	void CreateRegionObjectsAndMeshes()
	{
		foreach (int[,] region in regionsList) 
		{
			//create a building gameobject that contains a MeshGenerator, buildingMemory, ColorTimer, and ChangeColor
			GameObject building = new GameObject ();
			building.name = "BuildingObj";
			MeshGenerator buildingMeshGen = building.AddComponent<MeshGenerator> () as MeshGenerator;
			MeshFilter buildingMeshFilter = building.AddComponent<MeshFilter> () as MeshFilter;
			MeshRenderer buildingMeshRenderer = building.AddComponent<MeshRenderer> () as MeshRenderer;
			buildingMeshRenderer.material = buildingMat;
			MeshCollider buildingMeshCollider = building.AddComponent<MeshCollider> () as MeshCollider;
			buildingMemory buildingMemoryScript = building.AddComponent<buildingMemory> () as buildingMemory;
			colorTimer buildingColorTimerScript = building.AddComponent<colorTimer> () as colorTimer;
			changeColor buildingChangeColorScript = building.AddComponent<changeColor> () as changeColor;

			GameObject buildingWalls = new GameObject ();
			buildingWalls.name = "WallMesh";
			MeshFilter wallsMeshFilter = buildingWalls.AddComponent<MeshFilter> () as MeshFilter;
			MeshRenderer wallsMeshRenderer = buildingWalls.AddComponent<MeshRenderer> () as MeshRenderer;
			wallsMeshRenderer.material = wallMat;

			buildingMeshGen.walls = wallsMeshFilter;
			buildingWalls.transform.SetParent (building.transform);

			buildingMeshGen.GenerateMesh (region, 1f);

		}
	}

	void RemovePocketRegions()
	{
		List<List<Coord>> unFilledRegions = GetRegions (0);

		foreach (List<Coord> unfillRegion in unFilledRegions) {

			if (unfillRegion.Count < wallThresholdSize) 
			{
				foreach (Coord tile in unfillRegion) {

					level [tile.tileX, tile.tileY] = 1;	
				}
			}
		}
		List<List<Coord>> FilledRegions = GetRegions (1);

		foreach (List<Coord> fillRegion in FilledRegions) {

			if (fillRegion.Count < wallThresholdSize) 
			{
				foreach (Coord tile in fillRegion) {

					level [tile.tileX, tile.tileY] = 0;	
				}
			}
		}

	}

	void ProcessRegionsIntoMaps()
	{
		List<List<Coord>> FilledRegions = GetRegions (1);

		int count = 0;

		foreach (List<Coord> fillRegion in FilledRegions) 
		{

			int[,] buildingRegion= new int[map.width, map.height];
			count++;
			foreach (Coord tile in fillRegion) 
			{
				buildingRegion [tile.tileX, tile.tileY] = 1;

				level [tile.tileX, tile.tileY] = 0;				
			}
			//Debug.Log ("BuildingRegion contains: " + buildingRegion);
			regionsList.Add (buildingRegion);

		}
		//Debug.Log ("The region count should be : " + count);
		//Debug.Log ("RegionList contains: " + regionsList);
	}

	List<List<Coord>> GetRegions(int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>> ();
		// video timestamp 9:47 out of 16:52
		int[,] mapFlags =new int[map.width, map.height];


		for (int x = 0; x < map.width; x++) {
			for (int y = 0; y < map.height; y++) {

				if (mapFlags [x, y] == 0 && level [x, y] == tileType) {
					List<Coord> newRegion = GetRegionTiles (x, y);
					regions.Add (newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags [tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}

	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}

	List<Coord> GetRegionTiles(int startX, int startY)
	{
		List<Coord> tiles = new List<Coord> ();

		int[,] mapFlags = new int[map.width,map.height];
		int tileType = level [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) 
		{
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) 
			{
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) 
				{
					if(isInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
					{
						if (mapFlags [x, y] == 0 && level [x, y] == tileType) 
						{
							mapFlags [x, y] = 1;
							queue.Enqueue (new Coord (x, y));
						}	
					}
				}
			}
		}

		return tiles;
	}

	bool isInMapRange(int x, int y)
	{
		return x >= 0 && x < map.width && y >= 0 && y < map.height;
	}
	void FillMap()
	{
		for (int x = 0; x < map.width; x++) 
		{
			for (int y = 0; y < map.height; y++) 
			{
				Color pixelColor = map.GetPixel (x, y);
				if (pixelColor.Equals (wallColor))
					level [x, y] = 1;
				else
					level [x, y] = 0;
			}
		}
	}
}
