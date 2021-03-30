using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WpfApp.CaesarCypher
{
  public partial class MainWindow : Window
  {
    int Key = 0;
    string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"; // 33 symbols
    string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //
    List<string> InputText = new List<string>();
    List<string> OutputText = new List<string>();
    public MainWindow()
    {
      InitializeComponent();
    }
    // Browse button click event to open file
    private void buttonBrowse_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog()
      {
        CheckFileExists = false,
        CheckPathExists = true,
        Multiselect = false,
        Title = "Choose file",
        Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
      };

      if (dialog.ShowDialog() == true)
      {
        tbInput.Text = File.ReadAllText(dialog.FileName, Encoding.UTF8).ToUpper(); // could ToUpper
      }
    }
    // Encrypt button click event
    private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
    {
      InputText.Clear();
      OutputText.Clear();
      Key = Convert.ToInt32(tbKey.Text);
      for (int i = 0; i < tbInput.LineCount; i++)
      {
        InputText.Add(tbInput.GetLineText(i).ToUpper()); // could ToUpper
      }
      
      
      if(radioEnglish.IsChecked == true)
      {
        Encryption(EnglishAlphabet, Key % EnglishAlphabet.Length);
      }
      if(radioRussian.IsChecked == true)
      {
        Encryption(RussianAlphabet, Key % RussianAlphabet.Length);
      }  
    }
    // Encrypt function
    private void Encryption(string alphabet, int key)
    {
      string newLine = "";
      foreach (var line in InputText)
      {
        for (int i = 0; i < line.Length; i++)
        {
          if(alphabet.Contains(line[i].ToString()))
          {
            newLine += alphabet[(alphabet.IndexOf(line[i]) + key) % alphabet.Length];
          }
          else
          {
            newLine += line[i];
          }
        }
        OutputText.Add(newLine);
        newLine = "";
      }
      tbOutput.Clear();
      for (int i = 0; i < OutputText.Count; i++)
      {
        tbOutput.Text += OutputText[i];
      }
    }
    // Decrypt button click event
    private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
    {
      InputText.Clear();
      OutputText.Clear();
      Key = Convert.ToInt32(tbKey.Text);
      for (int i = 0; i < tbInput.LineCount; i++)
      {
        InputText.Add(tbInput.GetLineText(i).ToUpper()); // could ToUpper
      }


      if (radioEnglish.IsChecked == true)
      {
        Decryption(EnglishAlphabet, Key % EnglishAlphabet.Length);
      }
      if (radioRussian.IsChecked == true)
      {
        Decryption(RussianAlphabet, Key % RussianAlphabet.Length);
      }
    }
    // Decrypt function
    private void Decryption(string alphabet, int key)
    {
      string newLine = "";
      foreach (var line in InputText)
      {
        for (int i = 0; i < line.Length; i++)
        {
          if (alphabet.Contains(line[i].ToString()))
          {
            newLine += alphabet[(alphabet.IndexOf(line[i]) + alphabet.Length - key) % alphabet.Length];
          }
          else
          {
            newLine += line[i];
          }
        }
        OutputText.Add(newLine);
        newLine = "";
      }
      tbOutput.Clear();
      for (int i = 0; i < OutputText.Count; i++)
      {
        tbOutput.Text += OutputText[i];
      }
    }
    // Browse button click event to save file
    private void buttonSave_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog dialog = new SaveFileDialog()
      {
        Title = "Save file",
        Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
      };
      if (dialog.ShowDialog() == true)
      {
        File.WriteAllText(dialog.FileName, tbOutput.Text, Encoding.UTF8);
      }
    }
    // Only numeric input in key textbox
    private void tbKey_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!Char.IsDigit(e.Text, 0))
      {
        e.Handled = true;
      }
    }
  }
}
