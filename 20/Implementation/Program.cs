// See https://aka.ms/new-console-template for more information
string Input =
    """""
    broadcaster -> a, b, c
    %a -> b
    %b -> c
    %c -> inv
    &inv -> a
    """"";

Input = File.ReadAllText("input.txt");
ModuleNetwork network = ModuleNetwork.Parse(Input);
foreach (Module m in network.Nodes.Values)
{
    foreach (string destination in network.OutEdges[m.Name])
    {
        
        Console.WriteLine($"{m.Name} -> {destination}");
    }
}
