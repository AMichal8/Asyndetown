using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;
	public int borderSize = 5;
	//Need to redo...
	//Need to get color info from texture, and generate map based on if color matches wall (ex. black = wall = 1, vs. white = no wall = 0)

	//public colorToPrefab[] colorMappings;
	public Color wallColor;
	int [,] level;

	// Use this for initialization
	void Start () 
	{

		GenerateLevel ();
		
	}
	void GenerateLevel()
	{
		level = new int[map.width, map.height];
		//Debug.Log ("Map.width = " + map.width + ". Map.Height = " + map.height + ".");
		FillMap ();


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

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (borderedMap, 1f);

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
