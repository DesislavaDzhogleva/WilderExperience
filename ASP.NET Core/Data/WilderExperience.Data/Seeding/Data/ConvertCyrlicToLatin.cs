namespace WilderExperience.Data.Seeding.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConvertCyrlicToLatin
    {
        public static string Convert(string input)
        {
            var table = new Dictionary<char, string>();

            // Define mappings.
            // Uppercase modern Cyrillic characters.
            table.Add('\u0410', "A");
            table.Add('\u0411', "B");
            table.Add('\u0412', "V");
            table.Add('\u0413', "G");
            table.Add('\u0414', "D");
            table.Add('\u0415', "E");
            table.Add('\u0416', "Zh");
            table.Add('\u0417', "Z");
            table.Add('\u0418', "I");
            table.Add('\u0419', "I");
            table.Add('\u041A', "K");
            table.Add('\u041B', "L");
            table.Add('\u041C', "M");
            table.Add('\u041D', "N");
            table.Add('\u041E', "O");
            table.Add('\u041F', "P");
            table.Add('\u0420', "R");
            table.Add('\u0421', "S");
            table.Add('\u0422', "T");
            table.Add('\u0423', "U");
            table.Add('\u0424', "F");
            table.Add('\u0425', "Kh");
            table.Add('\u0426', "Ts");
            table.Add('\u0427', "Ch");
            table.Add('\u0428', "Sh");
            table.Add('\u0429', "Sht");
            table.Add('\u042A', "A");    // Hard sign
            table.Add('\u042B', "A");
            table.Add('\u042E', "Iu");
            table.Add('\u042F', "Ia");

            // Lowercase modern Cyrillic characters.
            table.Add('\u0430', "a");
            table.Add('\u0431', "b");
            table.Add('\u0432', "v");
            table.Add('\u0433', "g");
            table.Add('\u0434', "d");
            table.Add('\u0435', "e");
            table.Add('\u0436', "zh");
            table.Add('\u0437', "z");
            table.Add('\u0438', "i");
            table.Add('\u0439', "i");
            table.Add('\u043A', "k");
            table.Add('\u043B', "l");
            table.Add('\u043C', "m");
            table.Add('\u043D', "n");
            table.Add('\u043E', "o");
            table.Add('\u043F', "p");
            table.Add('\u0440', "r");
            table.Add('\u0441', "s");
            table.Add('\u0442', "t");
            table.Add('\u0443', "u");
            table.Add('\u0444', "f");
            table.Add('\u0445', "h");
            table.Add('\u0446', "ts");
            table.Add('\u0447', "ch");
            table.Add('\u0448', "sh");
            table.Add('\u0449', "sht");
            table.Add('\u044A', "a");   // Hard sign
            table.Add('\u044B', "a");
            table.Add('\u044E', "iu");
            table.Add('\u044F', "ia");

            foreach (var row in table)
            {
                input = input.Replace(row.Key.ToString(), row.Value);
                input = input.Replace(@"""", string.Empty);
            }

            return input;
        }
    }
}
