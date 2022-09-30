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

    private static bool exit = false;

    private delegate string Format(string input);

    [STAThread]
    public static void Main()
    {
        while (!exit)
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            string arg = GetInput("Type in the description, write your name using \"-[name]\" " +
                "or change the class with @[class].");

            if (arg.Length > 0)
            {
                string Upper(string input) => input.ToUpper();

                if (!SetFile(arg, "name", '-', NAME_FILE, Title) &&
                    !SetFile(arg, "class", '@', CLASS_FILE, Upper))
                {
                    Clipboard.SetText(GetBlockheader(arg));

                    Console.WriteLine("\nThe Blockheader has been copied to Clipboard.\n");
                }
            }
            else
            {
                exit = true;
            }
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
                Console.WriteLine($"{Title(name)} has been reset to default.\n");
            }
            else
            {
                File.WriteAllText(file, arg);
                Console.WriteLine($"\"{arg}\" saved in \"{file}\" as {name}.\n");
            }

            Main();
            return true;
        }
        return false;
    }

    private static string GetInput(string arg)
    {
        Console.Write(arg + "\n>");
        return Console.ReadLine();
    }

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
    {
        return (char)(c >= 'a' && c <= 'z' ? c - 32 : c);
    }

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
            if ((currentLine + word).Length > 62 && word.Length <= 62)
            {
                totalLines += currentLine + "\n * ";
                currentLine = word + ' ';
            }
            else
            {
                currentLine += word + ' ';
            }
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

        if (output[output.Length - 1] != '.')
        {
            output += '.';
        }
        
        return output;
    }
}
