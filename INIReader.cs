using System.Collections.Generic;
using System.IO;
using System.Text;

public class INIReader {
    private Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();

    public INIReader(string filename) {
        StringBuilder sb = new StringBuilder("");
        string name = "";
        string value = "";
        string category = "";
        dictionary.Add("", new Dictionary<string, string>());
        string[] lines = File.ReadAllLines(filename);
        for (int l = 0; l < lines.Length; ++l) {
            lines[l].TrimStart(' ', '\t'); //trim any whitespace
            if (lines[l] == "") { //if line is empty
                continue;
            }
            if (lines[l][0] == ';') { //go to the next line if the current one is a comment
                continue;
            }
            else if (lines[l][0] == '[') { //set category
                for (int c = 1; c < lines[l].Length - 1; ++c) {
                    sb.Append(lines[l][c]);
                }
                category = sb.ToString();
                dictionary.Add(category, new Dictionary<string, string>());
                sb.Length = 0;
                continue;
            }
            for (int c = 0; c < lines[l].Length; ++c) {
                //each character of the current line
                if (lines[l][c] == '=') {
                    name = sb.ToString();
                    sb.Length = 0;
                    ++c;
                }
                sb.Append(lines[l][c]);
                if (c == (lines[l].Length - 1)) {
                    value = sb.ToString();
                    sb.Length = 0;
                    break;
                }
            }
            //each line of the file
            dictionary[category].Add(name, value);
        }
    }

    public string GetString(string category, string name, string defaultValue) {
        try {
            return dictionary[category][name];
        }
        catch {
            return defaultValue;
        }
    }

    public string GetString(string category, string name) {
        return GetString(category, name, "");
    }

    public bool GetBool(string category, string name, bool defaultValue) {
        try {
            return bool.Parse(GetString(category, name, ""));
        } 
        catch {
            return defaultValue;
        }
    }

    public bool GetBool(string category, string name) {
        return GetBool(category, name, false);
    }

    public int GetInt(string category, string name, int defaultValue) {
        try {
            return int.Parse(GetString(category, name, ""));
        }
        catch {
            return defaultValue;
        }
    }

    public int GetInt(string category, string name) {
        return GetInt(category, name, 0);
    }

    public float GetFloat(string category, string name, float defaultValue) {
        try {
            return float.Parse(GetString(category, name, ""));
        }
        catch {
            return defaultValue;
        }
    }

    public float GetFloat(string category, string name) {
        return GetFloat(category, name, 0.0f);
    }

}
