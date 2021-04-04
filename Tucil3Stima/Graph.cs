using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tucil3Stima
{
    class Graph
    {
        // Attributes
        public int numOfVertices;      //number of vertices
        public List<String> vertices;  //list of vertices
        public List<String[]> edges;   //list of edges
        public Dictionary<int, String> nodeName;    //node name

        // Constructor
        public Graph()
        {
            numOfVertices = 0;
            vertices = new List<String>();
            edges = new List<String[]>();
            nodeName = new Dictionary<int, String>();
        }

        // Add edge to the graph
        // Will automatically add the vertices too
        public void AddEdge(String A, String B, String weight)
        {
            // add vertex
            if (!vertices.Contains(A))
            {
                vertices.Add(A);
                numOfVertices++;
            }

            // add edge
            String[] temp = { A, B, weight };
            edges.Add(temp);
            vertices.Sort();

        }

        // Return the adjacent vertices of vertex A with its weight
        // Already sorted by weight
        public Dictionary<String, int> AdjVerticesWithWeight(String A)
        {
            // add vertices
            Dictionary<String, int> adjA = new Dictionary<String, int>();
            foreach (String[] v in edges)
            {
                if (v[0] == A)
                {
                    adjA.Add(v[1], int.Parse(v[2]));
                }
            }

            // sorting by weight, ascending
            Dictionary<String, int> sorted = new Dictionary<String, int>();
            foreach (KeyValuePair<String, int> k in adjA.OrderBy(key => key.Value)) {
                sorted.Add(k.Key, k.Value);
            }

            return sorted;
        }

        // Read an adjacency matrix and add it to graph
        public void ReadMatrix(List<List<String>> matrix)
        {
            for (int i = 0; i < matrix.Count(); i++)
            {
                for (int j = 0; j < matrix[i].Count(); j++)
                {
                    if (matrix[i][j] != "0")
                    {
                        this.AddEdge(nodeName[i], nodeName[j], matrix[i][j]);
                    }
                }
            }
        }

        // Calculating the estimated cost to goal
        public void Heuristic(String A, String B, ref bool[] visited, ref bool found, ref Stack<int> weight)
        {
            // Set visited and get adjacent vertices
            visited[vertices.IndexOf(A)] = true;
            Dictionary<String, int> adjA = AdjVerticesWithWeight(A);
            List<String> adjNodes = adjA.Keys.ToList();

            // Loop adjacent vertices
            foreach (String v in adjNodes)
            {
                if (found) { break; }   //case already found, get out of the recursive call
                if (!visited[vertices.IndexOf(v)])
                {
                    if (v == B) { found = true; }
                    weight.Push(adjA[v]);   //add weight
                    Heuristic(v, B, ref visited, ref found, ref weight);    //recursive call
                }
            }

            // In case dead end, backtrack
            if (!found) { weight.Pop(); }
        }

        // Calculate each F of adjacent vertices then choose the minimum F
        public void AStarRecurse(String A, String B, ref bool[] visitedMain, ref List<String> path)
        {
            if (A == B) { return; } //end recursive call

            // Set visited and get adjacent vertices
            Dictionary<String, int> adjA = AdjVerticesWithWeight(A);
            List<String> adjNodes = adjA.Keys.ToList();
            visitedMain[vertices.IndexOf(A)] = true;

            // Loop adjacent vertices
            foreach (String v in adjNodes)
            {
                bool found = false;
                bool[] visited = (bool[])visitedMain.Clone();
                Stack<int> weight = new Stack<int>();
                weight.Push(adjA[v]);   //add weight

                if (v != B)
                {
                    Heuristic(v, B, ref visited, ref found, ref weight);
                }
                adjA[v] = weight.Sum(); //sum weight
            }

            // Select the node with minimum weight
            String selectedNode = adjA.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            path.Add(selectedNode);
            
            AStarRecurse(selectedNode, B, ref visitedMain, ref path);   //recursive call
        }

        // A Star Algorithm 
        public void AStar(String A, String B)
        {
            // Initiate variables
            List<String> path = new List<String>();
            bool[] visitedMain = new bool[numOfVertices];
            path.Add(A);

            AStarRecurse(A, B, ref visitedMain, ref path);

            // Print path, probably for debugging
            foreach(var v in path)
            {
                Debug.Write(v + " ");
            }
            Debug.WriteLine(" ");
        }

    }
}
