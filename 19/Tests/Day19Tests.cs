using System.Runtime.InteropServices;
using Shouldly;

namespace Tests;

public class Day19Tests
{

    public static readonly string SampleInput =
    """
    px{a<2006:qkq,m>2090:A,rfg}
    pv{a>1716:R,A}
    lnx{m>1548:A,A}
    rfg{s<537:gd,x>2440:R,A}
    qs{s>3448:A,lnx}
    qkq{x<1416:A,crn}
    crn{x>2662:A,R}
    in{s<1351:px,qqz}
    qqz{s>2770:qs,m<1801:hdj,R}
    gd{a>3333:R,R}
    hdj{m>838:A,pv}

    {x=787,m=2655,a=1222,s=2876}
    {x=1679,m=44,a=2067,s=496}
    {x=2036,m=264,a=79,s=2244}
    {x=2461,m=1339,a=466,s=291}
    {x=2127,m=1623,a=2188,s=1013}
    """.ReplaceLineEndings().Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day19.Part1(SampleInput).ShouldBe(19114);
    }

    [Fact]
    public void test_parse_workflow()
    {
        string input = "px{a<2006:qkq,m>2090:A,rfg}";
        Workflow parsed = Workflow.Parse(input);
        parsed.Label.ShouldBe("px");
        parsed.Conditions.Count().ShouldBe(3);
        ICondition[] conditions = [.. parsed.Conditions];
        conditions[0].ShouldBe(new IfCondition(Rating.A, Comparator.LT, 2006, "qkq"));
        conditions[1].ShouldBe(new IfCondition(Rating.M, Comparator.GT, 2090, "A"));
        conditions[2].ShouldBe(new ElseCondition("rfg"));
    }

    [Theory]
    [InlineData("a<2006:qkq", Rating.A, Comparator.LT, 2006, "qkq")]
    [InlineData("m>1548:A", Rating.M, Comparator.GT, 1548, "A")]
    public void test_parse_if_condition(string input, Rating rating, Comparator comparator, int value, string label)
    {
        ICondition.Parse(input).ShouldBe(new IfCondition(rating, comparator, value, label));
    }

    [Theory]
    [InlineData("qtq")]
    [InlineData("A")]
    [InlineData("R")]
    public void test_parse_else_condition(string label)
    {
        ICondition.Parse(label).ShouldBe(new ElseCondition(label));
    }

    [Theory]
    [InlineData("{x=787,m=2655,a=1222,s=2876}", 787, 2655, 1222, 2876)]
    [InlineData("{x=1679,m=44,a=2067,s=496}", 1679, 44, 2067, 496)]
    public void test_parse_machine_part(string input, int x, int m, int a, int s)
    {
        MachinePart.Parse(input).ShouldBe(new MachinePart(x, m, a, s));
    }
}