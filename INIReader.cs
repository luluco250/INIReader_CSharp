using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class INIReader {
    /// Dictionary used to store the values parsed from the ini file.
    public Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();

    /// <summary>
    /// Parse an INI file to a dictionary.
    /// <para><param name="filePath">Path to the INI file.</param></para>
    /// </summary>
    public INIReader(string filePath) {
        StringBuilder sb = new StringBuilder("");
        string category = "", name = "", value = "";
        dictionary.Add("", new Dictionary<string, string>());
        string[] lines = File.ReadAllLines(filePath);

        for (int l = 0; l < lines.Length; ++l) {
            sb.Length = 0; //reset sb in case a parsing error in the previous line left it with garbage data
            lines[l].TrimStart(' ', '\t'); //trim any whitespace

            if (lines[l] == "" || lines[l][0] == ';' || lines[l][0] == '#') {
                continue; //skip line if it's empty and/or is a comment
            }
            else if (lines[l][0] == '[') { //set category
                for (int c = 1; c < lines[l].Length - 1; ++c) {
                    sb.Append(lines[l][c]);
                }
                category = sb.ToString();
                dictionary.Add(category, new Dictionary<string, string>());
                continue;
            }

            for (int c = 0; c < lines[l].Length; ++c) {
                //each character of the current line
                if (lines[l][c] == '=') {
                    name = sb.ToString();
                    sb.Length = 0; //reset sb for building the value
                    ++c;
                }
                sb.Append(lines[l][c]);
                if (c == (lines[l].Length - 1)) {
                    value = sb.ToString();
                    break;
                }
            }
            //each line of the file
            dictionary[category].Add(name, value);
        }
    }

    /// <summary>
    /// Get the value of Type type from the INI file.
    /// <para><param name="category">The [category] in which the value is located in the INI file.</param></para>
    /// <para><param name="name">Name of the value.</param></para>
    /// <para><param name="defaultValue">Default value in case the value is not found or there's a type mismatch.</param></para>
    /// </summary>
    public type Get<type>(string category, string name, type defaultValue) {
        try {
            return (type)Convert.ChangeType(dictionary[category][name], typeof(type));
        }
        catch {
            return defaultValue;
        }
    }

    /// <summary>
    /// Get the value of Type type from the INI file.
    /// <para><param name="category">The [category] in which the value is located in the INI file.</param></para>
    /// <para><param name="name">Name of the value.</param></para>
    /// <para><para>'default(type)' is used in case the value is not found or there's a type mismatch.</para></para>
    /// </summary>
    public type Get<type>(string category, string name) {
        return Get<type>(category, name, default(type));
    }

    /// <summary>
    /// Get the value of Type type from the INI file.
    /// <para>Looks for values without a category.</para>
    /// <para><param name="name">Name of the value.</param></para>
    /// <para><para>'default(type)' is used in case the value is not found or there's a type mismatch.</para></para>
    /// </summary>
    public type Get<type>(string name) {
        return Get<type>("", name, default(type));
    }

    /// <summary>
    /// Get the value of Type type from the INI file.
    /// <para>Looks for values without a category.</para>
    /// <para><param name="name">Name of the value.</param></para>
    /// <para><param name="defaultValue">Default value in case the value is not found or there's a type mismatch.</param></para>
    /// </summary>
    public type Get<type>(string name, type defaultValue) {
        return Get<type>("", name, defaultValue);
    }

}
