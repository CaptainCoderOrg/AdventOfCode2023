public class Day12
{

    public static long Part1(string input)
    {
        
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(ProcessSprings)
                    .Sum();
    }

    public static long ProcessSprings(string input)
    {
        string[] parts = input.Split(" ");
        string[] nums = parts[1].Split(",", 2, StringSplitOptions.RemoveEmptyEntries);
        string rest = nums.Length == 1 ? string.Empty : nums[1];
        return ProcessSprings(parts[0], int.Parse(nums[0]), rest);
    }

    public static long ProcessSprings(string springs, int broken, string remaining)
    {
        // ???.### 1,1,3

        // If we have examined all of the springs
        if (springs is "")
        {
            // And there are no remaining broken springs
            // this is a valid configuration
            if (remaining is "" && broken == 0) { return 1; }
            // Otherwise it is invalid
            else { return 0; }
        }

        // If we have no more broken springs in the config
        if (broken == 0 && remaining is "")
        {
            // And there are still broken springs in the input
            // this is not a valid configuration
            if (springs.Contains('#')) { return 0; }
            // Otherwise it is
            else { return 1; }
        }

        // Examine next symbol
        // .
        // ?
        // #


        // We found the end of a broken spring group
        if (springs.StartsWith("#."))
        {
            // "#.*****", 1, ""
            if (broken == 1 && remaining is "")
            {
                return ProcessSprings(springs[2..], 0, "");
            }
            else if (broken == 1)
            {
                // Find the count for the next group of broken springs
                string[] nums = remaining.Split(",", 2, StringSplitOptions.RemoveEmptyEntries);
                string rest = nums.Length == 1 ? string.Empty : nums[1];
                return ProcessSprings(springs[2..], int.Parse(nums[0]), rest); 
            }
            else
            {
                return 0;
            }
        }

        if (springs.StartsWith("#?"))
        {
            long validWithBroken = ProcessSprings("##" + springs[2..], broken, remaining);
            long validWithWorking = ProcessSprings("#." + springs[2..], broken, remaining);
            return validWithBroken + validWithWorking;
        }

        if (springs.StartsWith(".?"))
        {
            long validWithBroken = ProcessSprings(".#" + springs[2..], broken, remaining);
            long validWithWorking = ProcessSprings(".." + springs[2..], broken, remaining);
            return validWithBroken + validWithWorking;
        }


        char nextSpring = springs[0];

        // We found a broken spring
        if (nextSpring == '#')
        {
            

            // If there are no more broken springs in this group
            // We are in an invalid configuration
            if (broken == 0) { return 0; }

            // TODO: Optimize by pulling ALL '#' off.
            return ProcessSprings(springs[1..], broken - 1, remaining);
        }

        // We found a working spring, so we just continue
        if (nextSpring == '.')
        {
            // TODO: Optimize by pulling ALL '.' off.
            return ProcessSprings(springs[1..], broken, remaining);
        }

        if (nextSpring == '?')
        {
            long validWithBroken = ProcessSprings('#' + springs[1..], broken, remaining);
            long validWithWorking = ProcessSprings('.' + springs[1..], broken, remaining);
            return validWithBroken + validWithWorking;
        }
        
        throw new Exception($"Encountered invalid character {nextSpring}.");
    }

}