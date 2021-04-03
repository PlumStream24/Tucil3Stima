using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tucil3Stima
{
    class Graph
    {
        // Attributes
        public int numOfVertices;      //number of vertices
        public List<String> vertices;  //list of vertices
        public List<String[]> edges;   //list of edges
        public Dictionary<int, String> nodeName;

        // Constructor
        public Graph()
        {
            numOfVertices = 0;
            vertices = new List<String>();
            edges = new List<String[]>();
            nodeName = new Dictionary<int, string>();
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
            if (!vertices.Contains(B))
            {
                vertices.Add(B);
                numOfVertices++;
            }
            // add edge
            String[] temp = { A, B, weight };
            edges.Add(temp);
            vertices.Sort();

        }

        //Return the adjacent vertices of vertex A
        public List<String> AdjVertices(String A)
        {
            List<String> adjA = new List<String>();
            foreach (String[] v in edges)
            {
                if (v[0] == A && !adjA.Contains(v[1]))
                {
                    adjA.Add(v[1]);
                }
                if (v[1] == A && !adjA.Contains(v[0]))
                {
                    adjA.Add(v[0]);
                }
            }

            adjA.Sort();

            return adjA;
        }


        public void ReadMatrix(List<List<String>> matrix)
        {
            for (int i = 0; i < matrix.Count(); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (matrix[i][j] != "0")
                    {
                        this.AddEdge(nodeName[i], nodeName[j], matrix[i][j]);
                    }
                }
            }
        }

    }
}
