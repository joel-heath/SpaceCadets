namespace SpaceCadets;
internal class Challenge1
{
    static readonly HttpClient client = new();

    static async Task Main(string[] args)
    {
        // Cool code examples:
        // pll
        // wh
        // spb
        // Uncool code examples:
        // 5y7mhh
        // 5y7tth
        // 5xswt6

        Console.Write("Enter the ID: ");
        string code = (Console.ReadLine() ?? "").Trim();
        Console.Write("Is it a cool ID? ");
        bool choice = (Console.ReadLine() ?? "").Trim().ToLower().Append('y').First() == 'y';

        string uri = choice ? @$"https://www.ecs.soton.ac.uk/people/{(code == "" ? "dem" : code)}" : @$"https://www.southampton.ac.uk/people/{(code == "" ? "5wyj8j" : code)}";
        string responseBody = await client.GetStringAsync(uri);

        var leader = "<meta property=\"og:title\" content=\"";
        int index = responseBody.IndexOf(leader) + leader.Length;
        var name = string.Concat(responseBody[index..].TakeWhile(c => c != '"'));

        Console.WriteLine(name);
    }
}