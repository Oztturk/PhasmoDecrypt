using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace PhasmoDecrypt;

public class EditJson
{


  public static void UnlockAllTier3(string data)
  {
    var obje = JObject.Parse(data);




    foreach (var prop in obje.Properties())
    {
      if (
          prop.Name.Contains("TierTwoUnlockOwned", StringComparison.OrdinalIgnoreCase) ||
          prop.Name.Contains("tierTwoUnlockOwned", StringComparison.OrdinalIgnoreCase) ||
          prop.Name.Contains("tierThreeUnlocked", StringComparison.OrdinalIgnoreCase) ||
          prop.Name.Contains("TierThreeUnlockOwned", StringComparison.OrdinalIgnoreCase))
      {
        if (prop.Value["__type"]?.ToString() == "bool")
        {
          prop.Value["value"] = true;
          Console.WriteLine($"Updated {prop.Name} to true");
        }
      }
    }

    Globals.DecryptedText = obje.ToString();
    //return obj.ToString();
  }


  public static string MaxItems(string data)
  {
    var obje = JObject.Parse(data);

    foreach (var prop in obje.Properties())
    {
      if (prop.Name.Contains("Inventory", StringComparison.OrdinalIgnoreCase))
      {
        if (prop.Value["__type"]?.ToString() == "int")
        {
          prop.Value["value"] = 999;
          Console.WriteLine($"Updated {prop.Name} to 999");
        }
      }
    }

    Globals.DecryptedText = obje.ToString();
    //File.WriteAllText("SaveFile_Decrypted.json", Globals.DecryptedText);

    return obje.ToString();
  }

  public static void EditMoney(string data, int amount)
  {
    var obje = JObject.Parse(data);

    foreach (var prop in obje.Properties())
    {
      if (prop.Name.Equals("PlayersMoney", StringComparison.OrdinalIgnoreCase))
      {
        if (prop.Value["__type"]?.ToString() == "int")
        {
          prop.Value["value"] = amount;
          Console.WriteLine($"Updated {prop.Name} to {amount}");
        }
      }
    }
    Globals.DecryptedText = obje.ToString();

  }

  public static void InfinityXp(string data, int amount)
  {
    var obje = JObject.Parse(data);

    foreach (var prop in obje.Properties())
    {
      if (prop.Name.Equals("Experience", StringComparison.OrdinalIgnoreCase))
      {
        if (prop.Value["__type"]?.ToString() == "int")
        {
          prop.Value["value"] = amount;
          Console.WriteLine($"Updated {prop.Name} to {amount}");
        }
      }
    }

    Globals.DecryptedText = obje.ToString();

  }
}
