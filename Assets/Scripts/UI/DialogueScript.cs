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
    private static string lineStartSignal = " /: ";
    private static string lineEndSignal = " :/";

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
    public static void FetchDialogue(int index)
    {
        string targetFile;
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
            targetFile = @"Assets\Scripts\UI\Dialogue.txt";
        else // Platform is not Windows
        {
            targetFile = @"Assets/Scripts/UI/Dialogue.txt";
        }
        
        System.IO.StreamReader file = new System.IO.StreamReader(@targetFile);
        
        string line;
        rawDialogue = new List<string>();

        while ((line = file.ReadLine()) != null)
        {
            if (line.Contains(dialogueSignal))
            {
                if (line.Contains(index.ToString())) //if desired dialogue
                {
                    while (!(line = file.ReadLine()).Contains(dialogueEndSignal))
                    {
                        rawDialogue.Add(line);
                       // Debug.Log(line);
                    }
                }
            }
        }

        dialogueLines = new string[rawDialogue.Count];
        speakingCharacter = new string[rawDialogue.Count];

        for(int i = 0; i < rawDialogue.Count; i++)
        {
            line = rawDialogue[i];


            int lineStart = line.IndexOf(lineStartSignal, System.StringComparison.Ordinal);
            int lineEnd = line.IndexOf(lineEndSignal, System.StringComparison.Ordinal);

            speakingCharacter[i] = line.Substring(0, lineStart);

            //(lineStart + lineStartSignal.Length)
            dialogueLines[i] = line.Substring((lineStart + lineStartSignal.Length), (line.Length - (lineStart + lineStartSignal.Length) - lineEndSignal.Length));

            Debug.Log(speakingCharacter[i] + ": " + dialogueLines[i]);
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
