using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GlobalVariables
{
    public static bool ResumeGame = false;
    public static bool GoToSleep = false;
    public static bool LockPlayerMovement = false; // mover a playercontroller
    public static bool CursorOverClickableObject;

    public static bool SaveFileExists()
    {
        FileInfo cropFile = new FileInfo(Path.Combine(Application.persistentDataPath, "cropdata.fmout"));
        FileInfo inventoryFile = new FileInfo(Path.Combine(Application.persistentDataPath, "inventorydata.fmout"));
        return cropFile.Exists && inventoryFile.Exists;
    }

    // convierte un string a un vector3int
    public static Vector3Int Vector3IntFromString(string vector)
    {
        Vector3Int parsedVector;
        string[] values;
        string tempString;

        tempString = vector.Substring(1, vector.Length - 2);
        values = tempString.Split(',');
        parsedVector = new Vector3Int(int.Parse(values[0].Trim()), int.Parse(values[1].Trim()), int.Parse(values[2].Trim()));

        return parsedVector;
    }

    public static Vector3 VectorFromString(string vector)
    {
        Vector3 parsedVector;
        string[] values;
        string tempString;

        tempString = vector.Substring(1, vector.Length - 2);
        values = tempString.Split(',');
        parsedVector = new Vector3(float.Parse(values[0].Trim().Replace('.',',')), float.Parse(values[1].Trim().Replace('.', ',')), float.Parse(values[2].Trim().Replace('.', ',')));

        return parsedVector;
    }
}
