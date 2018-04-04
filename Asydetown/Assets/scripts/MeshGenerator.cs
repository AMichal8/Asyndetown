//Code Acquired through Official Unity Tutorial "Procedural Cave Generation"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour 
{
	public SquareGrid squareGrid;
	public MeshFilter walls;
	public float wallHeight = 5f;
	List<Vector3> vertices;
	List<int> triangles;

	Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
	List<List<int>> outlines = new List<List<int>> ();
	HashSet<int> checkedVertices = new HashSet<int> ();


	public void GenerateMesh(int[,] map, float squareSize)
	{
		triangleDictionary.Clear ();
		outlines.Clear ();
		checkedVertices.Clear ();


		squareGrid = new SquareGrid (map, squareSize);
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		for (int x = 0; x < squareGrid.squares.GetLength (0); x++) 
		{
			for (int y = 0; y < squareGrid.squares.GetLength (1); y++) 
			{
				TriangulateSquare (squareGrid.squares [x, y]);
			}
		}

		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;

		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();

		CreateWallMesh ();

	}
	void CreateWallMesh()
	{
		CalculateMeshOutlines ();

		List<Vector3> wallvertices = new List<Vector3> ();
		List<int> wallTris = new List<int> ();
		Mesh wallMesh = new Mesh ();


		foreach (List<int> outline in outlines) 
		{
			for (int i = 0; i < outline.Count - 1; i++) 
			{
				int startIndex = wallvertices.Count;
				wallvertices.Add (vertices [outline [i]]); //left
				wallvertices.Add (vertices [outline [i+1]]); //right
				wallvertices.Add (vertices [outline [i]] - Vector3.up * wallHeight); //bottomleft
				wallvertices.Add (vertices [outline [i+1]] - Vector3.up * wallHeight); //bottomright

				wallTris.Add (startIndex + 0);
				wallTris.Add (startIndex + 2);
				wallTris.Add (startIndex + 3);

				wallTris.Add (startIndex + 3);
				wallTris.Add (startIndex + 1);
				wallTris.Add (startIndex + 0);

			}
		}
		wallMesh.vertices = wallvertices.ToArray ();
		wallMesh.triangles = wallTris.ToArray ();

		walls.mesh = wallMesh;
	}
	void TriangulateSquare(Square square)
	{
		switch (square.configuration) 
		{
		// no points
		case 0: 
			break;

		// 1 point
		case 1:
			MeshFromPoints(square.centerLeft, square.centerBottom, square.bottomLeft);
			break;

		case 2:
			MeshFromPoints(square.bottomRight, square.centerBottom, square.centerRight);
			break;

		case 4:
			MeshFromPoints(square.topRight, square.centerRight, square.centerTop);
			break;

		case 8:
			MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft);
			break;

		// 2 points
		case 3:
			MeshFromPoints(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;

		case 6:
			MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
			break;

		case 9:
			MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
			break;

		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
			break;

		case 5:
			MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
			break;

		case 10:
			MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

		// 3 points
		case 7:
			MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;

		case 11:
			MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

		// 4 points
		case 15:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVertices.Add (square.topLeft.vertextIndex);
			checkedVertices.Add (square.topRight.vertextIndex);
			checkedVertices.Add (square.bottomRight.vertextIndex);
			checkedVertices.Add (square.bottomLeft.vertextIndex);

			break;

		}
	}
	void MeshFromPoints(params Node[] points)
	{
		AssignVertices (points);

		if (points.Length >= 3)
			createTriangle (points [0], points [1], points [2]);
		if (points.Length >= 4)
			createTriangle (points [0], points [2], points [3]);
		if (points.Length >= 5)
			createTriangle (points [0], points [3], points [4]);
		if (points.Length >= 6)
			createTriangle (points [0], points [4], points [5]);

	}
	void createTriangle (Node a, Node b, Node c)
	{
		triangles.Add (a.vertextIndex);
		triangles.Add (b.vertextIndex);
		triangles.Add (c.vertextIndex);

		Triangle triangle = new Triangle (a.vertextIndex, b.vertextIndex, c.vertextIndex);

		AddTriangleToDictionary (triangle.vertexIndexA, triangle);
		AddTriangleToDictionary (triangle.vertexIndexB, triangle);
		AddTriangleToDictionary (triangle.vertexIndexC, triangle);


	}

	void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
	{
		if (triangleDictionary.ContainsKey (vertexIndexKey)) {
			triangleDictionary [vertexIndexKey].Add (triangle);
		} 
		else 
		{
			List<Triangle> triangleList = new List<Triangle> ();
			triangleList.Add (triangle);
			triangleDictionary.Add (vertexIndexKey, triangleList);
		}

	}

	void CalculateMeshOutlines()
	{
		for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++) 
		{
			if (!checkedVertices.Contains (vertexIndex)) 
			{
				int newOutlineVertex = GetConnectedOutlineVertex (vertexIndex);
				if (newOutlineVertex != -1) 
				{
					checkedVertices.Add (vertexIndex);
					List<int> newOutline = new List<int> ();
					newOutline.Add (vertexIndex);
					outlines.Add (newOutline);
					FollowOutline (newOutlineVertex, outlines.Count - 1);
					outlines [outlines.Count - 1].Add (vertexIndex);
				}
			}
		}
			
	}

	void FollowOutline(int vertexIndex, int outlineIndex)
	{
		outlines[outlineIndex].Add(vertexIndex);
			checkedVertices.Add(vertexIndex);
			int nextvertexIndex = GetConnectedOutlineVertex(vertexIndex);

			if(nextvertexIndex !=-1)
			{
				FollowOutline (nextvertexIndex, outlineIndex);
			}
	}
	void AssignVertices (Node[] points)
	{
		for (int i = 0; i < points.Length; i++) 
		{

			if (points [i].vertextIndex == -1) 
			{
				points [i].vertextIndex = vertices.Count;
				vertices.Add (points [i].pos);
			}
		}
	}

	int GetConnectedOutlineVertex(int vertexIndex)
	{
		List<Triangle> trianglesContainingVertex = triangleDictionary [vertexIndex];
		for (int i = 0; i < trianglesContainingVertex.Count; i++) 
		{
			Triangle triangle = trianglesContainingVertex [i];

			for (int j = 0; j < 3; j++) 
			{
				int vertexB = triangle [j];
				if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB)) 
				{
					if (isOutlineEdge (vertexIndex, vertexB)) 
					{
						return vertexB;
					}
				}

			}
		}
		return -1;
	}

	bool isOutlineEdge(int vertexA, int vertexB)
	{
		List<Triangle> trianglesWithVertA = triangleDictionary [vertexA];
		int sharedTriangleCount = 0;

		for (int i = 0; i < trianglesWithVertA.Count; i++) 
		{
			if (trianglesWithVertA [i].Contains (vertexB)) 
			{
				sharedTriangleCount++;
				if (sharedTriangleCount > 1) 
				{
					break;
				}
			}
		}
		return sharedTriangleCount == 1;
	}

	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int [] vertices;


		public Triangle(int a, int b, int c)
		{
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;
			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}
		public int this[int i]{
			get{
				return vertices [i];
			}
		}
		public bool Contains(int vertexIndex)
		{
			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
		}
	}

	public class SquareGrid
	{
		public Square[,] squares;

		public SquareGrid(int[,] map, float squareSize) 
		{
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);
			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY  * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for(int x =0; x<nodeCountX; x++)
			{
				for(int y =0; y<nodeCountY; y++)
				{
					Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos, map[x,y] == 1, squareSize);
				}
			}

			squares = new Square[nodeCountX-1,nodeCountY-1];
			for(int x =0; x<nodeCountX-1; x++)
			{
				for(int y =0; y<nodeCountY-1; y++)
				{
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1, y+1], controlNodes[x+1, y], controlNodes[x,y]);
				}
			}
		}


	}

	public class Square
	{
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centerTop, centerRight, centerBottom, centerLeft;
		public int configuration;

		public Square(ControlNode _topLeft, ControlNode _topRight,ControlNode _bottomRight,ControlNode _bottomLeft)
		{
			topLeft = _topLeft;
			topRight = _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;


			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;


			if(topLeft.active)
				configuration +=8;
			if(topRight.active)
				configuration +=4;
			if(bottomRight.active)
				configuration +=2;
			if(bottomLeft.active)
				configuration+=1;

		}
	}

	public class Node 
	{
		public Vector3 pos;
		public int vertextIndex = -1;

		public Node(Vector3 _pos)
		{
			pos = _pos;

		}
	}
	public class ControlNode: Node
	{

			public bool active;
			public Node above, right;

			public ControlNode(Vector3 _pos, bool _active, float squareSize): base(_pos) {
				active = _active;
				above = new Node(pos + Vector3.forward * squareSize/2f);
				right = new Node(pos + Vector3.right * squareSize/2f);
			
			}
	}
}
