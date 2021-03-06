﻿using System.Text.RegularExpressions;

namespace LexicalAnalyzer_JANA
{
    public class Analyzer
    {
        private char[] input { get; set; }
        private string output { get; set; }
        private string current { get; set; }
        private string delimiter { get; set; }
        private bool hasError { get; set; }
        private int column { get; set; }

        private string[] errorCodes = {"[No Errors]",
            "[LEXICAL ERROR] Improper use of reserved symbol \"{0}\"." };

        private string[] RW = { "get", "out", "clean", "tolower", "toupper", "exit", "main",
            "false", "stop", "true", "choice", "fall", "attempt", "handle", "do", "else", "elseif",
            "test", "then", "if", "iterate", "strlen", "until", "boolean", "char", "class", "int",
            "inherits", "new", "public", "private", "real", "return", "static", "string", "struct",
            "void" };

        private char[] separators = { ' ', '\n', ','};

        // Regular Expressions
        private string space = "\\s";
        private string paren = "\\(";
        private string hyphen = "\\-";
        private string semic = ";";
        private string newline = "\\n";
        private string zero = "0";
        private string und = "_";
        private string colon = ":";
        private string lowalpha = "[a-z]";
        private string upalpha = "[A-Z]";
        private string digit = "[1-9]";
        private string ASCII = ".";
        private string otherSY1 = "\\+";
        private string otherSY2 = "\\-|>";
        private string relop = "=|!";
        private string delRW_1 = ";|\\s";
        private string delRW_2 = ":|\\s|\\n";
        private string delRW_3 = "\\{|\\s|\\n";
        private string delRW_4 = "(|\\s";
        private string delSY_1 = "\\s|[1-9]";
        private string delSY_2 = "\\s|[A-Z]|[a-z]";
        private string delSY_3 = "\\s|[A-Z]|[a-z]|[1-9]";
        private string delSY_4 = "\\s|.";
        private string delSY_5 = ";|\\n";
        private string delSY_6 = "\\s|[A-Z]|[a-z]|[1-9]|.";
        private string delSY_7 = "\\s|[A-Z]|[a-z]|[1-9]|.|;|\\n";
        private string delSY_8 = "\\n|";
        private string delSY_9 = "\\s|.|;|\\n|";
        private string delSY_10 = "\\s|.|;";
        private string delSY_11 = ":|\\s|\\n||\"";
        private string delid = ";|\\s|=|\\[|";
        private string delit = ";|\\s|\\.";
        private string delch = ";|\\s|'";
        private string delstr = ";|\\s|\"";

        // Identifiers
        private string id = "^([a-z]|[A-Z])([a-z]|[A-Z]|[1-9]|[0]|_){0,9}";
        private string comment = "^(/\\*).*(\\*/)$";

        // Data Values
        private string intType = "^(~)*[1-9][0-9]{0,9}$";
        private string floatType = "^(~|[1-9])[0-9]{0,9}\\.[0-9]{0,10}";
        private string charType = "'.'";
        private string stringType = "^(\").*(\")$";

        // Arithmetic Operators
        private string add = "\\+";
        private string subtract = "\\-";
        private string divide = "/";
        private string wut = "\\";
        private string modulo = "%";
        private string multiply = "\\*";
        private string power = "\\^";
        private string negate = "~";

        // Logical Operators
        private string logical_and = "@@";
        private string logical_or = "\\|\\|";

        // Relational Operators
        private string equal = "==";
        private string not_equal = "==!";
        private string gt = ">>";
        private string lt = "<<";
        private string gt_equal = ">>=";
        private string lt_equal = "<<=";

        public string analyze(string statement)
        {
            // Set-up the Regular Expression objects.
            Regex rgxFloat = new Regex(floatType);
            Regex rgxInt = new Regex(intType);
            Regex rgxChar = new Regex(charType);
            Regex rgxString = new Regex(stringType);
            Regex rgxId = new Regex(id);

            bool isRW = false;
            bool hasError = false;
            output = "COMPLETE.\r\n";

            // Trim whitespaces from start and split string into a CharArray.
            input = statement.TrimStart().ToCharArray();
            current = "";

            // TODO: Refactor the code.
            // Code beyond this point is extremely redundant.

            // Iterate through the entire CharArray.
            foreach (var c in input)
            {
                if (c == ' ' && current == "") continue; // Ignore the whitespaces. Causes problems.
                else if (c == ' ' && current != "")
                {
                    if (isRW)
                    {
                        if (hasLookAhead(current, c.ToString()))
                            frmMain.Self.dGridResults.Rows.Add(current, current);
                        else
                        {
                            frmMain.Self.dGridResults.Rows.Add(current, "invalid");
                            hasError = true;
                            output += "[ERROR] Incorrect use of reserved word \"" + current + "\".\r\n";
                        }

                        current = "";
                        isRW = false;
                        continue;
                    } else
                    {
                        // Check the lexeme for types.
                        if (rgxFloat.IsMatch(current))
                            frmMain.Self.dGridResults.Rows.Add(current, "floattype");
                        else if (rgxInt.IsMatch(current))
                            frmMain.Self.dGridResults.Rows.Add(current, "inttype");
                        else if (rgxChar.IsMatch(current))
                            frmMain.Self.dGridResults.Rows.Add(current, "chartype");
                        else if (rgxString.IsMatch(current))
                            frmMain.Self.dGridResults.Rows.Add(current, "stringtype");
                        else if (rgxId.IsMatch(current))
                            frmMain.Self.dGridResults.Rows.Add(current, "identifier");
                        else
                        {
                            frmMain.Self.dGridResults.Rows.Add(current, "invalid");
                            hasError = true;
                            output += "[ERROR] Unknown symbol \"" + current + "\".\r\n";
                        }
                    }

                    current = "";
                } else
                {
                    if (isRW)
                    {
                        if (hasLookAhead(current, c.ToString()))
                            frmMain.Self.dGridResults.Rows.Add(current, current);
                        else
                        {
                            frmMain.Self.dGridResults.Rows.Add(current, "invalid");
                            hasError = true;
                            output += "[ERROR] Incorrect use of reserved word \"" + current + "\".\r\n";
                        }

                        current = "";
                        isRW = false;
                        continue;
                    }

                    current += c.ToString();
                    isRW = checkRW(current);
                }
            }

            if (isRW)
            {
                if (hasLookAhead(current, ""))
                    frmMain.Self.dGridResults.Rows.Add(current, current);
                else
                {
                    frmMain.Self.dGridResults.Rows.Add(current, "invalid");
                    hasError = true;
                    output += "[ERROR] Incorrect use of reserved word \"" + current + "\".\r\n";
                }

                current = "";
                isRW = false;
            }
            else
            {
                if (rgxFloat.IsMatch(current))
                    frmMain.Self.dGridResults.Rows.Add(current, "floattype");
                else if (rgxInt.IsMatch(current))
                    frmMain.Self.dGridResults.Rows.Add(current, "inttype");
                else if (rgxChar.IsMatch(current))
                    frmMain.Self.dGridResults.Rows.Add(current, "chartype");
                else if (rgxString.IsMatch(current))
                    frmMain.Self.dGridResults.Rows.Add(current, "stringtype");
                else if (rgxId.IsMatch(current))
                    frmMain.Self.dGridResults.Rows.Add(current, "identifier");
                else
                {
                    frmMain.Self.dGridResults.Rows.Add(current, "invalid");
                    hasError = true;
                    output += "[ERROR] Unknown symbol \"" + current + "\".\r\n";
                }
            }

            current = "";

            if (!hasError) output = "COMPLETE.\r\nNo Errors.";

            return output;
        }

        // Method checks for the lookaheads of the reserved words.
        private bool hasLookAhead(string current, string c)
        {
            switch (current)
            {
                case "get":
                    Regex rgx = new Regex(hyphen);
                    return (rgx.IsMatch(c) ? true : false);
                case "out":
                    rgx = new Regex(hyphen);
                    return (rgx.IsMatch(c) ? true : false);
                case "clean":
                    rgx = new Regex(paren);
                    return (rgx.IsMatch(c) ? true : false);
                case "tolower":
                    rgx = new Regex(paren);
                    return (rgx.IsMatch(c) ? true : false);
                case "toupper":
                    rgx = new Regex(paren);
                    return (rgx.IsMatch(c) ? true : false);
                case "exit":
                    rgx = new Regex(paren);
                    return (rgx.IsMatch(c) ? true : false);
                case "main":
                    rgx = new Regex(paren);
                    return (rgx.IsMatch(c) ? true : false);
                case "false":
                    rgx = new Regex(delRW_1);
                    return (rgx.IsMatch(c) ? true : false);
                case "stop":
                    rgx = new Regex(delRW_1);
                    return (rgx.IsMatch(c) ? true : false);
                case "true":
                    rgx = new Regex(delRW_1);
                    return (rgx.IsMatch(c) ? true : false);
                case "choice":
                    rgx = new Regex(delRW_2);
                    return (rgx.IsMatch(c) ? true : false);
                case "fall":
                    rgx = new Regex(delRW_2);
                    return (rgx.IsMatch(c) ? true : false);
                case "attempt":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "handle":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "do":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "else":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "elseif":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "test":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "then":
                    rgx = new Regex(delRW_3);
                    return (rgx.IsMatch(c) ? true : false);
                case "iterate":
                    rgx = new Regex(delRW_4);
                    return (rgx.IsMatch(c) ? true : false);
                case "strlen":
                    rgx = new Regex(delRW_4);
                    return (rgx.IsMatch(c) ? true : false);
                case "until":
                    rgx = new Regex(delRW_4);
                    return (rgx.IsMatch(c) ? true : false);
                case "boolean":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "char":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "class":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "int":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "inherits":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "new":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "public":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "if":
                    rgx = new Regex(delRW_4);
                    return (rgx.IsMatch(c) ? true : false);
                case "private":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "real":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "return":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "static":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "string":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "struct":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
                case "void":
                    rgx = new Regex(space);
                    return (rgx.IsMatch(c) ? true : false);
            }

            return false;
        }

        // Method checks if the lexeme "current" is a reserved word.
        private bool checkRW(string current)
        {
            foreach (var s in RW)
            {
                if (current == s) return true;
            }

            return false;
        }
    }
}
