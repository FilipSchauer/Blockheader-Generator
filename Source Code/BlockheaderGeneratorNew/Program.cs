/* --------------------------------------------------------------
 *              HTBLA - Leonding / Class: 2DHIF
 * --------------------------------------------------------------
 *              Filip Schauer, 30.09.2022
 * --------------------------------------------------------------
 * Description:
 * Generates C# blockheaders like the one you see here. 
 * --------------------------------------------------------------
*/

public class Program
{
    const string PATH = "C:/ProgramData/BlockheaderGenerator";
    const string NAME_FILE = "C:/ProgramData/BlockheaderGenerator/name.txt";
    const string CLASS_FILE = "C:/ProgramData/BlockheaderGenerator/class.txt";

    private delegate string Format(string input);

    [STAThread]
    public static void Main()
    {
        while (true)
        {
            string arg = GetInput("Type in the description, write your name using \"-[name]\" " +
                "or change the class with @[class].");

            Directory.CreateDirectory(PATH);

            if (arg.Length > 0)
            {
                string Upper(string input) => input.ToUpper();

                if (!SetFile(arg, "name", '-', NAME_FILE, Title) &&
                    !SetFile(arg, "class", '@', CLASS_FILE, Upper))
                {
                    string blockheader = GetBlockheader(arg);

                    WriteLineColor("\n\n" + blockheader + "\n\n", ConsoleColor.Blue);

                    Clipboard.SetText(blockheader);

                    WriteLineColor("\nThe Blockheader has been copied to Clipboard.\n", ConsoleColor.Green);
                }
            }
            else
                return;
        }
    }

    private static bool SetFile(string arg, string name, char prefix, string file, Format format)
    {
        if (arg[0] == prefix)
        {
            arg = arg.Substring(1);
            arg = format(arg);

            if (arg == "")
            {
                File.Delete(file);
                WriteLineColor($"\n{Title(name)} has been reset to default.\n", ConsoleColor.Red);
            }
            else
            {
                File.WriteAllText(file, arg);
                WriteLineColor($"\n\"{arg}\" saved in \"{file}\" as {name}.\n", ConsoleColor.DarkYellow);
            }
            return true;
        }
        return false;
    }

    private static string GetInput(string arg)
    {
        Console.WriteLine(arg);
        WriteColor(">", ConsoleColor.Cyan, false);
        return Console.ReadLine();
    }

    private static void WriteColor(string arg, ConsoleColor color, bool resetColor = true)
    {
        Console.ForegroundColor = color;
        Console.Write(arg);
        if (resetColor) 
            Console.ResetColor();
    }
    private static void WriteLineColor(string arg, ConsoleColor color)
        => WriteColor(arg + '\n', color);

    private static string Title(string input)
    {
        string output = "";
        char lastChar = ' ';

        foreach (char c in input)
        {
            output += lastChar == ' ' ? CharToUpper(c) : c;
            lastChar = c;
        }
        return output;
    }

    static char CharToUpper(char c)
        => (char)(c >= 'a' && c <= 'z' ? c - 32 : c);

    static string GetBlockheader(string description)
    {
        string name = ReadFile(NAME_FILE, "{name}");
        string @class = ReadFile(CLASS_FILE, "{class}");

        string date = DateTime.UtcNow.ToString("dd.MM.yyyy");

        return  "/* --------------------------------------------------------------\n" +
               $" *              HTBLA - Leonding / Class: {@class}\n" +
                " * --------------------------------------------------------------\n" +
               $" *              {name}, {date}\n" +
                " * --------------------------------------------------------------\n" +
                " * Description:\n" +
               $" * {FormatDescription(description)}\n" +
                " * --------------------------------------------------------------\n" +
                "*/";
    }

    private static string ReadFile(string file, string notAvavible)
    {
        return File.Exists(file) ? File.ReadAllText(file) : notAvavible;
    }

    private static string FormatDescription(string input)
    {
        string[] words = BasicFormatting(input).Split(' ');
        string currentLine = "";
        string totalLines = "";

        foreach (string word in words)
        {
            if ((currentLine + word).Length > 62)
            {
                totalLines += currentLine + "\n * ";
                currentLine = "";
            }
            currentLine += word + ' ';
        }

        return totalLines + currentLine;
    }

    static string BasicFormatting(string input)
    {
        string output = "";
        string lastChars = ". ";

        foreach (char c in input)
        {
            if (lastChars == ". ")
            {
                char c_ = CharToUpper(c);
                lastChars = c_.ToString();
                output += c_;
            }
            else
            {
                if (lastChars != " " || c != ' ')
                {
                    output += c;
                }
                
                if (lastChars == "" || lastChars == ".")
                {
                    lastChars += c;
                }
                else
                {
                    lastChars = c.ToString();
                }
            }
        }

        for (int i = output.Length - 1; i >= 0; i--)
        {
            if (output[i] != ' ')
            {
                output = output.Substring(0, i + 1);
                break;
            }
        }

        char[] endLines = { '.', '!', '?' };
        if (!endLines.Contains(output[output.Length - 1]))
        {
            output += '.';
        }
        
        return output;
    }
}
