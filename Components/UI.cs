using System;
using System.Collections.Generic;

/*
  <summary>
  Represents a UI menu with selectable options.
  </summary>
  <remarks>
  Usage:
  var menu = new UI(new string[] { "Option1", "Option2" }, index => Console.WriteLine($"Selected {index}"));
  menu.Display();
  </remarks>
 */

public class UI
{
  private string[] _options;
  private Action<int> _onSelect;
  private ConsoleColor _selectedBackgroundColor;
  private ConsoleColor _selectedForegroundColor;
  public UI(string[] options, Action<int> onSelect,
      ConsoleColor selectedBackgroundColor = ConsoleColor.DarkMagenta,
      ConsoleColor selectedForegroundColor = ConsoleColor.White)
  {
    _options = options;
    _onSelect = onSelect;
    _selectedBackgroundColor = selectedBackgroundColor;
    _selectedForegroundColor = selectedForegroundColor;
  }
  public void Display()
  {
    int selectedIndex = 0;
    while (true)
    {
      Console.Clear();
      Tag();
      for (int i = 0; i < _options.Length; i++)
      {
        Console.Write("\t");
        if (i == selectedIndex)
        {
          Console.BackgroundColor = _selectedBackgroundColor;
          Console.ForegroundColor = _selectedForegroundColor;
          Console.Write($"\t{_options[i]}\t");
          Console.ResetColor();
        }
        else
        {
          Console.Write($"\t{_options[i]}\t");
        }
      }

      ConsoleKey key = Console.ReadKey(true).Key;

      if (key == ConsoleKey.LeftArrow)
      {
        selectedIndex = (selectedIndex == 0) ? _options.Length - 1 : selectedIndex - 1;
      }
      else if (key == ConsoleKey.RightArrow)
      {
        selectedIndex = (selectedIndex == _options.Length - 1) ? 0 : selectedIndex + 1;
      }
      else if (key == ConsoleKey.Enter)
      {
        Console.Clear();
        _onSelect.Invoke(selectedIndex);
        break;
      }
    }
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
