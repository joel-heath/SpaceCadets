namespace SpaceCadets;
internal class Challenge2
{
    static void Main(string[] args)
    {
        string sourceCode = @"clear X;
incr X;
incr X;
incr X;
while X not 0 do;
decr X;
end;";

        sourceCode = @"clear X;
incr X;
incr X;
clear Y;
incr Y;
incr Y;
incr Y;
clear Z;
while X not 0 do;
   clear W;
   while Y not 0 do;
      incr Z;
      incr W;
      decr Y;
   end;
   while W not 0 do;
      incr Y;
      decr W;
   end;
   decr X;
end;";

        Dictionary<string, int> variables = [];
        Stack<int> loopStack = [];

        var instructions = sourceCode.Split(';').Select(i => i.Trim()).ToList();
        for (int i = 0; i < instructions.Count; i++)
        {
            var instruction = instructions[i];
            var tokens = instruction.Split(' ');
            bool printVars = true;
            
            switch (tokens[0])
            {
                case "clear":
                    variables[tokens[1]] = 0;
                    break;
                case "incr":
                    if (variables.TryGetValue(tokens[1], out int val1))
                        variables[tokens[1]] = val1 + 1;
                    else
                        throw new Exception("Name '" + tokens[1] + "' does not exist in the current context");
                    break;
                case "decr":
                    if (variables.TryGetValue(tokens[1], out int val2))
                        variables[tokens[1]] = val2 - 1;
                    else
                        throw new Exception("Name '" + tokens[1] + "' does not exist in the current context");
                    break;
                case "while":
                    printVars = false;
                    var conditional = tokens[1..^1];
                    bool result = variables.TryGetValue(conditional[0], out int val3)
                        ? !((conditional[1] == "is") ^ val3 == (variables.TryGetValue(conditional[2], out int val4) ? val4 : int.Parse(conditional[2])))
                        : throw new Exception("Name '" + conditional[0] + "' does not exist in the current context");
                    if (result)
                        loopStack.Push(i);
                    else
                    {
                        i = instructions.FindIndex(i + 1, s => s == "end");
                        if (i == -1)
                            throw new Exception("Untermiated loop: 'end' expected");
                    }
                    break;
                case "end":
                    printVars = false;
                    if (loopStack.Count == 0)
                        throw new Exception("End-of-file expected");

                    var whileLine = loopStack.Peek();
                    var conditional1 = instructions[whileLine].Split(' ')[1..^1];
                    bool result1 = variables.TryGetValue(conditional1[0], out int val5)
                        ? !((conditional1[1] == "is") ^ val5 == (variables.TryGetValue(conditional1[2], out int val6) ? val6 : int.Parse(conditional1[2])))
                        : throw new Exception("Name '" + conditional1[0] + "' does not exist in the current context");

                    if (result1)
                        i = whileLine;
                    else
                        loopStack.Pop();

                    break;
                default:
                    printVars = false;
                    //throw new Exception("Invalid instruction: " + instruction);
                    break;
            }

            if (printVars)
            {
                foreach (var variable in variables)
                {
                    Console.WriteLine(variable.Key + " = " + variable.Value);
                }
            }
        }
    }
}