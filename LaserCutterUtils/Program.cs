if (args.Length < 1)
{
    Console.WriteLine("ERRO! Você tentou executar o programa sem caminho. Execute arrastando um arquivo .gc para este executável.");
    Console.ReadKey();
    return;
}

var filePath = args[0];

var fileExtension = Path.GetExtension(filePath);

if (fileExtension != ".gc")
{
    Console.WriteLine("ERRO! Só arquivos GCODE (.gc)");
    Console.ReadKey();
    return;
}

var numberOfCopies = 0;
while (true)
{
    Console.Write("Insira o número de passes do laser (de 2 a 99): ");
    var parseSuccess = Int32.TryParse(Console.ReadLine(), out numberOfCopies);
    Console.WriteLine();

    if (parseSuccess == false)
    {
        Console.WriteLine("ERRO! O valor deve ser um inteiro.");
        continue;
    }

    if (numberOfCopies > 1 & numberOfCopies < 99) break;
    Console.WriteLine("ERRO! O valor deve estar entre 2 e 99.");
}

Console.WriteLine($"Convertendo o arquivo {filePath}");

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

Console.WriteLine("Conversão concluída!");
Console.WriteLine($"Arquivo convertido: {outputFilePath}");

Console.ReadKey();
