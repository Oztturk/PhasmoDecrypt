using System;
using System.Security.Principal;
using System.Security.Cryptography;
namespace PhasmoDecrypt;

internal class Program
{
  private static void Main(string[] args)
  {
    //I will do an optimization update in the future, repetitive code reduces readability too much

    Console.Title = Globals.Title;


    string[] options = new string[] { "Decrypt", "Encrypt", "Exit" };
    int SelectedIndex = 0;


    while (true) //the system here is old so I made a new one a class but I will not update this one ^^
    {

      Console.Clear();
      Tag();

      for (int i = 0; i < options.Length; i++)
      {
        Console.Write($"\t\t");
        if (i == SelectedIndex)
        {
          Console.BackgroundColor = ConsoleColor.DarkMagenta;
          Console.ForegroundColor = ConsoleColor.White;
          Console.Write($"\t{options[i]}\t");
          //Console.Write(string.Format("\n {0}"), options[i]);
          Console.ResetColor();
        }
        else
        {
          Console.Write($"\t{options[i]}\t");
        }
      }

      ConsoleKey key = Console.ReadKey(true).Key;

      if (key == ConsoleKey.LeftArrow)
      {
        SelectedIndex = (SelectedIndex == 0) ? options.Length - 1 : SelectedIndex - 1;
      }
      else if (key == ConsoleKey.RightArrow)
      {
        SelectedIndex = (SelectedIndex == options.Length - 1) ? 0 : SelectedIndex + 1;
      }
      else if (key == ConsoleKey.Enter)
      {
        Console.Clear();
        switch (SelectedIndex)
        {
          case 0:
            DecryptTab();
            break;
          case 1:
            EncryptTab();
            break;
          case 2:
            Environment.Exit(0);
            break;
        }
      }


    }
  }

  static void DecryptTab()
  {
    Console.Clear();
    Tag();

    var crypter = new Crypter();

    string[] options = { "Use My Save File", "I'll add it myself", "Back" };
    var menu = new UI(
        options,
        onSelect: selectedIndex =>
        {
          switch (selectedIndex)
          {
            case 0:
              string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "Kinetic Games", "Phasmophobia");
              string saveFile = Path.Combine(path, "SaveFile.txt");
              if (!File.Exists(saveFile))
              {
                Console.WriteLine("No saveFile.txt found at the specified location. Please select the other option.");
                Console.ReadKey();
                DecryptTab();
              }

              string decryptedText = crypter.Decrypt(File.ReadAllBytes(saveFile));
              Globals.DecryptedText = decryptedText;
              AfterDecryptTab();
              break;

            case 1:
            AddFileSection:
              Console.Clear();
              Tag();
              System.Console.Write("File Path > ");
              string? filePath = Console.ReadLine()?.Trim('"');

              if (string.IsNullOrWhiteSpace(filePath))
              {
                Console.WriteLine("Invalid file path. Please try again.");
                Console.ReadKey();
                goto AddFileSection;
              }

              if (!File.Exists(filePath))
              {
                Console.WriteLine("No saveFile.txt found at the specified location. Please select the other option.");
                Console.ReadKey();
                goto AddFileSection;
              }

              string decryptedTextManual = crypter.Decrypt(File.ReadAllBytes(filePath));
              Globals.DecryptedText = decryptedTextManual;
              AfterDecryptTab();
              break;

            case 2:
              Main(new string[0]);
              break;
          }
        }
    );

    menu.Display();
  }

  static void AfterDecryptTab()
  {
    Console.Clear();
    Tag();

    string[] options = { "Save Decrypted File", "Presets", "Back" };
    var menu = new UI(
            options,
            onSelect: selectedIndex =>
            {
              switch (selectedIndex)
              {
                case 0:

                  if (Globals.DecryptedText == null)
                  {
                    Console.WriteLine("No data to save. Please decrypt the file first.");
                    Console.ReadKey();
                    AfterDecryptTab();
                    //Main(new string[0]);
                    return;
                  }

                  File.WriteAllText("SaveFile_Decrypted.json", Globals.DecryptedText);
                  System.Console.WriteLine("> Data saved to SaveFile_Decrypted.json");
                  Console.ReadKey();
                  Main(new string[0]);
                  break;
                case 1:
                  PresetsTab();
                  break;
                case 2:
                  DecryptTab();
                  break;
              }
            }
        );

    menu.Display();
  }

  static void PresetsTab()
  {
    Console.Clear();
    Tag();
    var crypter = new Crypter();

    string[] options = { "Unlock All tier 3", "Max Items", "Change Money", "\n\n\t\tChange Xp", "\tUse all", "\tSave and encrypt", "\n\n\t\tSave decrypted", "\tBack", };
    var menu = new UI(
            options,
            onSelect: selectedIndex =>
            {
              switch (selectedIndex)
              {
                case 0:
                  EditJson.UnlockAllTier3(Globals.DecryptedText);
                  System.Console.WriteLine("> Succes press any key to continue");
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 1:
                  EditJson.MaxItems(Globals.DecryptedText);
                  System.Console.WriteLine("> Succes press any key to continue");
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 2:
                  Console.WriteLine("Enter the amount you want to change the money to:");
                  string? amount = Console.ReadLine();
                  if (!int.TryParse(amount, out int moneyAmount))
                  {
                    Console.WriteLine("Invalid amount. Please enter a valid integer.");
                    Console.ReadKey();
                    PresetsTab();
                    break;
                  }
                  EditJson.EditMoney(Globals.DecryptedText, moneyAmount);
                  System.Console.WriteLine("> Succes press any key to continue");
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 3:
                  Console.WriteLine("Enter the amount you want to change the xp to:");
                  string? xpAmount = Console.ReadLine();
                  if (!int.TryParse(xpAmount, out int xp))
                  {
                    Console.WriteLine("Invalid amount. Please enter a valid integer.");
                    Console.ReadKey();
                    PresetsTab();
                    break;
                  }
                  EditJson.InfinityXp(Globals.DecryptedText, xp);
                  System.Console.WriteLine("> Succes press any key to continue");
                  Console.ReadKey();
                  PresetsTab();
                  break;


                case 4:
                  EditJson.UnlockAllTier3(Globals.DecryptedText);
                  EditJson.MaxItems(Globals.DecryptedText);

                xp:
                  Console.Write("Enter the amount you want to change the xp to: ");
                  string? xpAmountt = Console.ReadLine();
                  if (!int.TryParse(xpAmountt, out int xpp))
                  {
                    Console.WriteLine("Invalid amount. Please enter a valid integer.");
                    Console.ReadKey();
                    goto xp;
                  }

                money:
                  Console.Write("Enter the amount you want to change the money to ");
                  string? Money = Console.ReadLine();
                  if (!int.TryParse(Money, out int mny))
                  {
                    Console.WriteLine("Invalid amount. Please enter a valid integer.");
                    Console.ReadKey();
                    goto money;
                  }

                  EditJson.EditMoney(Globals.DecryptedText, mny);
                  EditJson.InfinityXp(Globals.DecryptedText, xpp);
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 5:
                  byte[] decryptedText = crypter.EncryptData(Globals.DecryptedText);
                  File.WriteAllBytes("SaveFile_Encrypted.txt", decryptedText);
                  Console.WriteLine("Encrypt Successful saved to SaveFile_Encrypted.txt");
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 6:
                  File.WriteAllText("SaveFile_Decrypted.json", Globals.DecryptedText);
                  System.Console.WriteLine("> Data saved to SaveFile_Decrypted.json");
                  Console.ReadKey();
                  PresetsTab();
                  break;
                case 7:
                  DecryptTab();
                  break;
              }
            }
        );

    menu.Display();
  }



  static void EncryptTab()
  {
    Console.Clear();
    Tag();

    var crypter = new Crypter();

    string[] options = { "Use My Decrypted File", "I ll add it myself", "Back" };
    var menu = new UI(
            options,
            onSelect: s =>
            {
              switch (s)
              {
                case 0:
                  string saveFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveFile_Decrypted.json");
                  if (!File.Exists(saveFile))
                  {
                    Console.WriteLine("No SaveFile_Decrypted.json found at the specified location. Please select the other option.");
                    Console.ReadKey();
                  }
                  var crypter = new Crypter();

                  byte[] decryptedText = crypter.EncryptData(File.ReadAllText(saveFile));
                  File.WriteAllBytes("SaveFile_Encrypted.txt", decryptedText);
                  Console.WriteLine("Encrypt Successful saved to SaveFile_Encrypted.txt");
                  Console.ReadKey();
                  Main(new string[0]);
                  break;
                case 1:
                AddFileSection:
                  Console.Clear();
                  Tag();
                  System.Console.Write("File Path > ");
                  string? filePath = Console.ReadLine()?.Trim('"');

                  if (string.IsNullOrWhiteSpace(filePath))
                  {
                    Console.WriteLine("Invalid file path. Please try again.");
                    Console.ReadKey();
                    goto AddFileSection;
                  }

                  if (!File.Exists(filePath))
                  {
                    Console.WriteLine("No SaveFile_Decrypted.txt found at the specified location. Please select the other option.");
                    Console.ReadKey();
                    goto AddFileSection;
                  }
                  var crypters = new Crypter();

                  byte[] decryptedTextManual = crypters.EncryptData(File.ReadAllText(filePath));
                  File.WriteAllBytes("SaveFile_Encrypted.txt", decryptedTextManual);
                  Console.WriteLine("Encrypt Successful saved to SaveFile_Encrypted.txt");
                  Console.ReadKey();
                  Main(new string[0]);
                  break;
                case 2:
                  Main(new string[0]);
                  break;
              }
            }
        );

    menu.Display();
  }

  static void Tag()
  {
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.WriteLine(@"
             _____   __  __  ______  ____             _____       ____    ____    ______  ______   
            /\  _`\ /\ \/\ \/\  _  \/\  _`\   /'\_/`\/\  __`\    /\  _`\ /\  _`\ /\__  _\/\__  _\  
            \ \ \L\ \ \ \_\ \ \ \L\ \ \,\L\_\/\      \ \ \/\ \   \ \ \L\_\ \ \/\ \/_/\ \/\/_/\ \/  
             \ \ ,__/\ \  _  \ \  __ \/_\__ \\ \ \__\ \ \ \ \ \   \ \  _\L\ \ \ \ \ \ \ \   \ \ \  
              \ \ \/  \ \ \ \ \ \ \/\ \/\ \L\ \ \ \_/\ \ \ \_\ \   \ \ \L\ \ \ \_\ \ \_\ \__ \ \ \ 
               \ \_\   \ \_\ \_\ \_\ \_\ `\____\ \_\\ \_\ \_____\   \ \____/\ \____/ /\_____\ \ \_\
                \/_/    \/_/\/_/\/_/\/_/\/_____/\/_/ \/_/\/_____/    \/___/  \/___/  \/_____/  \/_/
                ");
    Console.ResetColor();
  }


}