namespace CaptainCoder.MathUtils;

public static class MathUtils {
    public static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);
    public static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    public static long LCM(IEnumerable<long> nums) => nums.Aggregate(LCM);
}