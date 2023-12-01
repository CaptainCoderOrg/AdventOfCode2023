// See https://aka.ms/new-console-template for more information
using System.Text;

Dictionary<string, string> Translation = new()
{
    {"one", "one1one"},
    {"two", "two2two"},
    {"three", "three3three"},
    {"four", "four4four"},
    {"five", "five5five"},
    {"six", "six6six"},
    {"seven", "seven7seven"},
    {"eight", "eight8eight"},
    {"nine", "nine9nine"},

};

string[] lines = File.ReadAllLines("input.txt");
Part2();
void Part1()
{
    int result = lines.Select(OnlyDigits).Select(FirstAndLastDigit).Sum();
    Console.WriteLine(result);
}

void Part2()
{
    var justReplaced = lines.Select(ReplaceNumbers);
    // Console.WriteLine(string.Join("\n", justReplaced));
    var onlyDigits = justReplaced.Select(OnlyDigits);
    // Console.WriteLine(string.Join("\n", onlyDigits));
    int result = onlyDigits.Select(FirstAndLastDigit).Sum();
    Console.WriteLine(result);
}

string ReplaceNumbers(string original)
{
    foreach (string key in Translation.Keys)
    {
        original = original.Replace(key, Translation[key]);
    }
    return original;
}

int FirstAndLastDigit(string digitString)
{
    int first = int.Parse(digitString[0].ToString());
    //digitString[^1] === digitString[digitString.Length - 1];
    int last = int.Parse(digitString[^1].ToString());
    return first * 10 + last;
}

string OnlyDigits(string original)
{
    StringBuilder builder = new StringBuilder();
    foreach (char ch in original)
    {
        if (char.IsDigit(ch))
        {
            builder.Append(ch);
        }
    }
    return builder.ToString();
}