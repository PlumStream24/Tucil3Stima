using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tucil3Stima
{
    public class Graph
    {
        // Attributes
        public int numOfVertices;      //number of vertices
        public List<String> vertices;  //list of vertices
        public List<String[]> edges;   //list of edges
        //[[node A, node B, weight], and so on]
        public Dictionary<(String,String),int> manhattan; //dict of the manhattan distance of each vertices to each other
        //[[(node A, node B),weight], and so on] -> access the weight of the edge by entering the key which is (starting node, destination node)
        public Dictionary<int, String> nodeName;    //node name

        //added comments for reminder

        // Constructor
        public Graph()
        {
            numOfVertices = 0;
            vertices = new List<String>();
            edges = new List<String[]>();
            manhattan = new Dictionary<(string, string), int>();
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
            //vertices.Sort();

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
            //actually initializing manhattan
            //manhattan will consist of manhattan distance of each vert, including to themselves
            foreach (String vert in vertices)
            {
                manhattan.Add((vert, vert), 0);
                makeManhattan(vert, (vert, 0), new Dictionary<string, int>());
            }
        }

        //making manhattan
        public void makeManhattan(String start, (String name, int weight) node, Dictionary<String, int> seen)
        {
            foreach (String[] element in edges)
            {
                if (element[0] == node.name && element[1]!=start)
                {
                    (String, String) temp = (start, element[1]);
                    int tempWeight = node.weight + Convert.ToInt32(element[2]);
                    if (manhattan.ContainsKey(temp))
                    {
                        if (manhattan[temp] > tempWeight)
                        {
                            manhattan.Remove(temp);
                            manhattan.Add(temp, tempWeight);
                            seen.Remove(element[1]);
                            seen.Add(element[1], tempWeight);
                        }
                    }
                    else
                    {
                        manhattan.Add(temp, tempWeight);
                        seen.Add(element[1], tempWeight);
                    }
                }
            }
            while (seen.Count() > 0)
            {
                (String, int) tempPair = (seen.First().Key, seen.First().Value);
                seen.Remove(tempPair.Item1);
                makeManhattan(start, tempPair, seen);
            }
        }

        public (List<String>, int) AllStar((List<String>,int) start, String end, List<(List<String>, int)> data) //joke name, change this later perhaps
            //HEY NOW, YOU'RE AN ALLSTAR, PUT YOUR GAME ON, GO PLAY
            //returns a tuple
            //item 1 is the List of String of the path
            //item 2 is the length
            //if error, item 1 is empty, item 2 is -1
        {
            if (manhattan.ContainsKey((start.Item1.Last(), end)))
            {
                if (start.Item1.Last() == end)
                {
                    return start;   
                }
                foreach (String[] element in edges)
                {
                    // if left edge == active node and adj node unvisited
                    if (start.Item1.Last() == element[0] && !start.Item1.Contains(element[1]))
                    {
                        data.Remove(start);
                        List<String> tempList = start.Item1.ToList();
                        int tempWeight = start.Item2 - manhattan[(start.Item1.Last(), end)] + Convert.ToInt32(element[2]) + manhattan[(element[1], end)];
                        tempList.Add(element[1]);
                        data.Add((tempList, tempWeight));
                    }
                }
                data.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                return AllStar(data.First(), end, data);
            }
            else
            {
                return (new List<String>(), -1);
            }
        }

        /*
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
        }*/

    }
}
