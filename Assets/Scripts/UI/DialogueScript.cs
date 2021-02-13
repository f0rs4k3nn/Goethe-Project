using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    private static string dialogueSignal = ">>>>! ";
    private static string dialogueEndSignal = "***!";
    private static string lineStartSignal = "/:";
    private static string lineEndSignal = ":/";

    private static List<string> rawDialogue;
    private static string[] dialogueLines;
    private static string[] speakingCharacter; //which character is speaking at a given line

    //public static System.IO.StreamReader file;
    //
    // Summary:
    //     Initializez the script and fetches the dialogue
    //     in respect to the given index
    //
    // Parameters:
    //   index: current dialogue to play
    //
    public static void FetchDialogue(int indez)
    {
        char index = (char)(indez + (char)96);

        TextAsset myTextDataEN = Resources.Load<TextAsset>("DialogueFolder/Dialogue");
        TextAsset myTextDataDE = Resources.Load<TextAsset>("DialogueFolder/DialogueDE");

        string textEnglish = myTextDataEN.text;
        string textDeutch = myTextDataDE.text;

        string targetFile;

        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            switch (GameManager.Instance.Language)
            {
                case "en_GB":
                    targetFile = "EN";
                    break;
                case "de_DE":
                    targetFile = "DE";
                    break;
                default:
                    targetFile = "EN";
                    break;
            }
        }
        else // Platform is not Windows
        {
            switch (GameManager.Instance.Language)
            {
                case "en_GB":
                    targetFile = "EN";
                    break;
                case "de_DE":
                    targetFile = "DE";
                    break;
                default:
                    targetFile = "EN";
                    break;
            }
        }


        StringReader fileEN = new StringReader(@textEnglish);
        StringReader fileDE = new StringReader(@textDeutch);


        string line;
        rawDialogue = new List<string>();

        if (targetFile == "EN")
        {
            while ((line = fileEN.ReadLine()) != null)
            {
                if (line.Contains(dialogueSignal))
                {
                    if (line.Contains(index.ToString())) //if desired dialogue
                    {
                        while (!(line = fileEN.ReadLine()).Contains(dialogueEndSignal))
                        {
                            rawDialogue.Add(line);
                            // Debug.Log(line);
                        }
                    }
                }
            }
        }
        else if (targetFile == "DE")
        {
            while ((line = fileDE.ReadLine()) != null)
            {
                if (line.Contains(dialogueSignal))
                {
                    if (line.Contains(index.ToString())) //if desired dialogue
                    {
                        while (!(line = fileDE.ReadLine()).Contains(dialogueEndSignal))
                        {
                            rawDialogue.Add(line);
                            // Debug.Log(line);
                        }
                    }
                }
            }
        }


        dialogueLines = new string[rawDialogue.Count];
        speakingCharacter = new string[rawDialogue.Count];

        for (int i = 0; i < rawDialogue.Count; i++)
        {
            line = rawDialogue[i];

            int lineStart = line.IndexOf(lineStartSignal, System.StringComparison.Ordinal);
            int lineEnd = line.IndexOf(lineEndSignal, System.StringComparison.Ordinal);

            Debug.Log(i + " of " + index);
            if (lineStart < 0)
            {
                Debug.LogWarning(line);
                lineStart = line.IndexOf(lineStartSignal, System.StringComparison.Ordinal);
                Debug.Log(lineStart);

                speakingCharacter[i] = line.Substring(0, lineStart);
                dialogueLines[i] = line.Substring((lineStart + lineStartSignal.Length), (line.Length - (lineStart + lineStartSignal.Length) - lineEndSignal.Length));
            }
            else
            {
                speakingCharacter[i] = line.Substring(0, lineStart);
                //(lineStart + lineStartSignal.Length)
                dialogueLines[i] = line.Substring((lineStart + lineStartSignal.Length), (line.Length - (lineStart + lineStartSignal.Length) - lineEndSignal.Length));
                //            Debug.Log(speakingCharacter[i] + ": " + dialogueLines[i]);
            }
        }
    }

    //public static System.IO.StreamReader file;
    //
    // Summary:
    //     Returns the dialogue array in respect to the 
    //     given index in the initialize function.
    //     Will return null if Initialize() wasn't called.
    //
    public static string[] GetDialogueArray() { return dialogueLines; }

    //public static System.IO.StreamReader file;
    //
    // Summary:
    //     Returns the array with the currently speaking character in respect to the 
    //     given index in the initialize function.
    //     Will return null if Initialize() wasn't called.
    //
    public static string[] GetSpeakingCharArray() { return speakingCharacter; }
}
