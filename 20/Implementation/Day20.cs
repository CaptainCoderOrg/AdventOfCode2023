using CaptainCoder.MathUtils;
public class Day20
{
    public static long Part1(string input)
    {
        ModuleNetwork network = ModuleNetwork.Parse(input);
        (long lows, long highs) = Enumerable.Repeat(0, 1000)
                                            .Select(_ => network.PushButton())
                                            .Aggregate((lh0, lh1) => (lh0.Low + lh1.Low, lh0.High + lh1.High));
        return lows * highs;                                            
    }

    public static long Part2(string input)
    {
        ModuleNetwork network = ModuleNetwork.Parse(input);
        Dictionary<string, long> CycleLength = new();
        string[] targets = [ "db", "lm", "sg", "dh"];
        int pushes = 0;
        while (CycleLength.Count < 4 && pushes < 5_000)
        {
            network.PushButton();
            pushes++;
            foreach (string target in targets)
            {
                Module m = network.Nodes[target];
                if (m.HasSentHigh && !CycleLength.ContainsKey(target))
                {
                    CycleLength[target] = pushes;
                }
            }
        }
        return MathUtils.LCM(CycleLength.Values);
    }
}

public enum ModuleType : ushort
{
    Broadcaster = 'b',
    FlipFlop = '%',
    Conjunction = '&',
    Sink = '*',
}

public record Pulse(bool IsHigh, string Source, string Destination);
public class ModuleNetwork
{
    public Dictionary<string, List<string>> IncomingEdges = new();
    public Dictionary<string, string[]> OutEdges = new();
    public Dictionary<string, Module> Nodes = new ();
    private ModuleNetwork() {}

    public static ModuleNetwork Parse(string input)
    {
        ModuleNetwork network = new ModuleNetwork();
        string[] rows = input.ReplaceLineEndings().Trim().Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (string row in rows)
        {
            //broadcaster -> a, b, c
            string[] parts = row.Split(" -> ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            string[] destinations = parts[1].Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts[0] == "broadcaster")
            {
                network.Nodes["broadcaster"] = new Module(ModuleType.Broadcaster, "broadcaster", network);
                network.OutEdges["broadcaster"] = destinations;
                network.AddIncomingEdges("broadcaster", destinations);
                continue;
            }
            ModuleType type = (ModuleType)parts[0][0];
            string name = parts[0][1..];
            network.Nodes[name] = new Module(type, name, network);
            network.OutEdges[name] = destinations;
            network.AddIncomingEdges(name, destinations);
        }
        return network;
    }

    public (long Low, long High) PushButton()
    {
        long low = 1;
        long high = 0;
        Queue<Pulse> pulses = new Queue<Pulse>();
        Console.WriteLine("button -low-> broadcaster");
        IEnumerable<Pulse> initial = Nodes["broadcaster"].HandlePulse(new Pulse(false, string.Empty, string.Empty));
        pulses.EnqueueAll(initial);
        while (pulses.TryDequeue(out Pulse? outgoing))
        {
            Console.WriteLine($"{outgoing.Source} -{(outgoing.IsHigh ? "high" : "low")}-> {outgoing.Destination}");
            high += outgoing.IsHigh ? 1 : 0;
            low += outgoing.IsHigh ? 0 : 1;
            if (Nodes.TryGetValue(outgoing.Destination, out Module? targetModule))
            {
                pulses.EnqueueAll(targetModule.HandlePulse(outgoing));
            }            
        }
        return (low, high);
    }

    private void AddIncomingEdges(string source, string[] destinations)
    {
        foreach (string destination in destinations)
        {
            if (!IncomingEdges.TryGetValue(destination, out List<string>? edgeList))
            {
                edgeList = new(); 
                IncomingEdges[destination] = edgeList;
            }            
            edgeList.Add(source);
        }
    }
}

public record Module(ModuleType MType, string Name, ModuleNetwork Network)
{

    public bool IsOn { get; private set; } = false;
    public bool HasSentHigh { get; private set; } = false;
    private Dictionary<string, bool> _lastInput = new ();
    public IEnumerable<Pulse> HandlePulse(Pulse incoming)
    {
        Func<Pulse, IEnumerable<Pulse>> handler = MType switch
        {
            ModuleType.Broadcaster => (_) => Network.OutEdges[Name].Select(destination => new Pulse(false, Name, destination)),
            ModuleType.FlipFlop => (pulse) =>
            {
                if (pulse.IsHigh) { return []; }
                IsOn = !IsOn;
                return Network.OutEdges[Name].Select(destination => new Pulse(IsOn, Name, destination));
            },
            ModuleType.Conjunction => (pulse) =>
            {
                _lastInput[pulse.Source] = pulse.IsHigh;
                bool allHigh = Network.IncomingEdges[Name]
                                      .Select(moduleName => _lastInput.GetValueOrDefault(moduleName, false))
                                      .All(x => x);
                HasSentHigh |= !allHigh;
                return Network.OutEdges[Name].Select(destination => new Pulse(!allHigh, Name, destination));
            },
            ModuleType.Sink => (_) => [],
            _ => throw new Exception($"Invalid module type {MType}")

        };
        return handler(incoming);
    }
}

public static class Helpers
{
    public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> toEnqueue)
    {
        foreach (T el in toEnqueue)
        {
            queue.Enqueue(el);
        }
    }
}