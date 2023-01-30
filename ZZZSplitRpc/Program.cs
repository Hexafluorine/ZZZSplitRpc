namespace ZZZSplitRPC
{
    class Program
    {

        public static void Main(string[] args)
        {
            int count = 0;
            int files = 0;
            string? input = null;
            string? output = null;
            string? name = "";
            List<string> rpc = new List<string>();
            #if DEBUG
            input = @"D:\Decompile\ZZZ\ZZZRpcDumper\rpcs.json";
            output = @"D:\Decompile\ZZZ\ZZZRpcDumper\RPC Proto\Rpc";
            #else
            if (args.Length <= 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: SplitRPC <Input Path> <Output Path>");
            }
            else
            {
                input = args[0];
                output = args[1];
            }
            #endif
            if (input != null && output != null)
            {
                Console.WriteLine("Processing...");
                string[] read = File.ReadAllLines(input);
                rpc.Add("{");
                for (int i = 1; i < read.Length; i++)
                {
                    string line = read[i];
                    if (line.Contains('{')) count++;
                    else if (line.Contains('}')) count--;
                    if (line.Contains("\"Name\": "))
                    {
                        string stringName = "\"Name\": ";
                        name = line.Substring(line.IndexOf(stringName) + stringName.Length);
                        name = name.Remove(name.Length - 2, 2);
                        name = name.Remove(0, 1);
                        Console.WriteLine(name);
                    }
                    if (line.Contains("},") && count == 0) rpc.Add("  " + line.Substring(line.IndexOf("}")).Remove(1, 1));
                    else rpc.Add("  " + line);
                    if (count == 0)
                    {
                        rpc.Add("}");
                        File.WriteAllLines(output + @"\" + name + ".json", rpc);
                        files++;
                        rpc.Clear();
                        rpc.Add("{");
                    }
                }
                Console.WriteLine($"Done, {files} Files Processed");
                Console.WriteLine("Press Any Key To Exit...");
                Console.ReadKey();

            }
        }
    }
}