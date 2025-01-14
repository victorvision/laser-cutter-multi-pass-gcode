if (args.Length < 1)
{
    Console.WriteLine("ERROR! Tried to run without file path. Run this by dragging and dropping a file .gc into the .exe file.");
    Console.ReadKey();
    return;
}

var filePath = args[0];

var fileExtension = Path.GetExtension(filePath);

if (fileExtension != ".gc")
{
    Console.WriteLine("ERROR! Only GCode (.gc) files are accepted.");
    Console.ReadKey();
    return;
}

var numberOfCopies = 0;
while (true)
{
    Console.Write("Type the desired number of passes (2 to 99): ");
    var parseSuccess = int.TryParse(Console.ReadLine(), out numberOfCopies);
    Console.WriteLine();

    if (parseSuccess == false)
    {
        Console.WriteLine("ERROR! Number must be an integer.");
        continue;
    }

    if (numberOfCopies > 1 & numberOfCopies < 99) break;
    Console.WriteLine("ERROR! Accepted number of passes: 2 to 99.");
}

Console.WriteLine($"Converting file {filePath}");

var fileName = Path.GetFileNameWithoutExtension(filePath);
var directoryName = Path.GetDirectoryName(filePath);

var outputFilePath = Path.Combine(directoryName, fileName + "_output.gc");

var lines = File.ReadAllLines(filePath).ToList();

// Manipule lines
var duplicateLines = new List<string>();
var initialIndex = lines.FindIndex(x => x == "M8");
var finallIndex = lines.FindIndex(x => x == "M9");

for(var j = 0; j < numberOfCopies - 1; j++)
{
    for (var i = initialIndex + 1; i < finallIndex; i++)
    {
        duplicateLines.Add(lines[i]);
    }
}

lines.InsertRange(initialIndex + 1, duplicateLines);

File.WriteAllLines(outputFilePath, lines);

Console.WriteLine("Conversion finished!");
Console.WriteLine($"Converted file output: {outputFilePath}");

Console.WriteLine("Press any key to exit...");

Console.ReadKey();
