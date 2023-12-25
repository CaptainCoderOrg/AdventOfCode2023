string input = File.ReadAllText("input.txt");
Graph g = Graph.Parse(input);
Console.WriteLine(g.ToDot());