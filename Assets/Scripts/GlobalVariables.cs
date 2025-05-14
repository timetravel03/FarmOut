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

    public static bool SaveFileExists()
    {
        FileInfo cropFile = new FileInfo(Path.Combine(Application.persistentDataPath, "cropdata.fmout"));
        return cropFile.Exists;
    }

}
