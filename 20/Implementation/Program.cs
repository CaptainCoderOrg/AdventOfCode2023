// See https://aka.ms/new-console-template for more information
string SampleInput1 =
    """""
    broadcaster -> a, b, c
    %a -> b
    %b -> c
    %c -> inv
    &inv -> a
    """"";
ModuleNetwork network = ModuleNetwork.Parse(SampleInput1);
network.PushButton();