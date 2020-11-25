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
    private static string dialogueText = @"
	
	
>>>>! a
Dr. Green /: ... :/
Dr. Green /: ... :/
Dr. Green /: Enviro, can you copy? Over. :/
Enviro /: Huh? :/
Dr. Green /: Enviro, are you there? :/
Enviro /: Who- who are you again?. :/
Dr. Green /: Oh, it would appear as if your memory has been wiped in the teleportation. :/ 
Dr. Green /: Well... At least I finally got a hold of you. I thought you were blown to smithereens. :/
Dr. Green /: It would appear as if your teleportation module is a bit defective. :/
Enviro /: My... What? :/
Dr. Green /: Oh... Well, you have a time traveling module that lets you go through the past and future. :/
Dr. Green /: You are even able to see some of the differences between the two worlds. :/
Dr. Green /: You can switch between the two by pressing the clock button. :/
Enviro /: Wait, why would I even have that? :/
Dr. Green /: Enviro... I have built and designed you for the sole purpose of saving the planet. :/
Dr. Green /: You are our last chance. :/
Dr. Green /: The world has been destroyed by Corpo-corp and the future is a wasteland which cannot be lived in! :/
Dr. Green /: That is why I sent you to the past, to put an end to their endeavours and save all of us. :/
Dr. Green /: Unfortunately, while your time traveling module works, your teleportation one is not working properly. :/
Dr. Green /: I-- erm, have no idea why that is, given that I'm such a brilliant scientist and all...  :/
Dr. Green /: ...Uhm, nonetheless, you will need to find the teleporters that were spread throughout the world to get to your destination. :/
Dr. Green /: They are like some yellow sparkling portals... Their are quite pretty! :/
Dr. Green /:  But don't look at them for too long, the research of my peers has proved that they are capable of temporarily blinding you... :/
Dr. Green /:  ...For the rest of your life... But don't worry about them! Just find them and get to your destination! :/
Enviro /: Uhuh, ok? I guess I will try. :/
***!

>>>>! b
Dr. Green /: Haha, it would appear as if you managed to find one of the portals! :/
Enviro /: Just barely... But... Why is the world flooded? :/
Dr. Green /: Hmm, already huh? These are the effects of what Corpo-corp is doing. :/
Dr. Green /: It is because of their carbon footprint, the world is gradually heating up more and more.  :/
Dr. Green /: Carbon dioxide retains heat, and the more its density in the atmosphere increases, the warmer is gets.  :/
Dr. Green /: This is known as the greenhouse effect, and because of it, the glaciers have already melted and flooded the planet.  :/
Enviro /: But... That's horrible! Why would they even do this? :/
Dr. Green /: Their goals aren't inherently evil perse... They only desire the technological development of mankind. :/
Dr. Green /: To be frank... *cough* Without them, the technology required to build you would not even exist... :/
Dr. Green /: However, their method of reaching their goals is causing the world a lot of harm. :/
Dr. Green /: That is why I have created you. *cough* *cough* :/
Enviro /: I... Wouldn't exist without them? :/
Dr. Green /: Yes... But do not think of yourself that you are responsible for their actions in any way! :/
Enviro /: That is true... And, afterall, you are the one who created me. :/
Enviro /: At least you claim so. :/
Dr. Green /: Haha, yes indeed! This GENIUS hahaha *cough* *cough*... Well, anyway. :/
Dr. Green /: Onward you go. I greatly trust you, Enviro. :/
Enviro /: I appreciate it... However, I am still unsure about you and your goals. :/
Enviro /: For all I know, you might be the bad person. :/
Dr. Green /: ... :/
Dr. Green /: I can understand your skepticism, however, I can only ask you to trust me for now. :/
Enviro /: ...Hm, I will try to. :/
***!

>>>>! c
Dr. Green /: Enviro? Can you copy? :/
Dr. Green /: Helooooo? *cough* :/
Enviro /: Yes yes, roger. :/
Dr. Green /: Hey, what's with that attitude? :/
Enviro /: I've already told you I am not sure if I fully trust you. :/
Dr. Green /: I... I would like to make this better somehow, but I am not actually sure how... :/
Dr. Green /: Hmm... Oh, I know! I will give you a tip. That will help you. :/
Enviro /: You mean help you to achieve your goal? :/
Dr. Green /: Erm- *cough* Don't take it like that! Just listen. :/
Dr. Green /: Corpo-corp has some hostile units they might have deployed from the future to get you. :/
Dr. Green /: It is very likely they know about your existence... *cough* :/
Dr. Green /: There are two main units, one of them is The Maggie, and the second one is The Trashy. :/
Dr. Green /: Once they spot you, they will try their best to destroy you. :/
Dr. Green /: And they can also time travel as well, so switching between the two won't get you away from them. :/
Dr. Green /: However, they are quite easy to get rid off. You cannot destroy them, but they have a very limited range! :/
Dr. Green /: If you handle them properly, you can easily run away from them. :/
Enviro /: Oh- Incredible, you've helped me know about some enemies I have to face because of you. Thanks. :/
Dr. Green /: Can you even pretend to be grateful? :/
Enviro /: Perhaps I wasn't programmed for that. :/
Dr. Green /: Hmm that might be true, unfortunately. :/
Dr. Green /: However, I did install a remote destruction device that is in my posession if I recall correctly. Might use if I find it convenient. :/
Enviro /: Are you sure the enemy is Corpo-corp? :/
***!

>>>>! d
Dr. Green /: Heeey, Earth to planet Envirooo. Copy? :/
Enviro /: Roger that. :/
Dr. Green /: That was much better than last time, are you finally starting to open up? :/
Enviro /: I wouldn't go so far ahead... :/
Enviro /: You do look very sketchy with that outfit. :/
Dr. Green /: Wh- Wha?! :/
Dr. Green /: What does that even mean?! :/
Enviro /: Anyway, the units you mentioned last time are definitely no pushover, but I managed to get past and find the portal. :/
Enviro /: Speaking of which, how many of these things are there? :/
Dr. Green /: Hm... I can't say for sure, however, you are definitely getting closer. :/
Dr. Green /: Just keep pushing, Enviro, I can trust you! :/
Enviro /: Are you implying you wouldn't trust your own creation? :/
Dr. Green /: ...Wouldn't say it'd be the first time. :/
Dr. Green /: But *cough* you are different. :/
Enviro /: ... :/
Enviro /: Thanks. :/
***!

>>>>! e
Dr. Green /: Eloooo heeey, Envi! You good? *cough* *cough* :/
Enviro /: Don't you have anything better to do than calling me every minute or so? :/
Enviro /: Go outside or talk to your colleagues. :/
Dr. Green /: I... Can't really do that. :/
Enviro /: I can understand that, talking to you does feel like a chore sometimes. :/
Dr. Green /: They all died in the past months. :/
Enviro /: ... :/
Dr. Green /: The air is unbreathable outside, all humans now live isolated inside and have air fitration systems. :/
Dr. Green /: But they are not perfect *cough* *cough*, which is why most people still die of respiratory failure. :/
Dr. Green /: That is how they all died...  And to be honest, I am not sure how many people are still left. :/
Dr. Green /: Afterall... It's been over 5 months since I last spoke to someone. *cough* :/
Dr. Green /: I'm also not sure how much time I have left... :/
Enviro /: ... :/
Enviro /: Doc... I... :/
Dr. Green /: Don't worry, there was no way for you to have known. *cough* *cough* :/
Dr. Green /: Good luck on your journey, Enviro. :/
Dr. Green /: Green out. :/
Enviro /: ... :/
***!

>>>>! f
Enviro /: ... :/
Enviro /: ... :/
Enviro /: ... :/
Enviro /: ...He isn't calling? :/
Enviro /: I wonder if something happened... :/
***!

>>>>! g
Enviro /: Hello? Doctor Green, do you copy? Over. :/
Dr. Green /: Huh? Enviro?? You Actually called? :/
Enviro /: It's...  It's been a while since we last spoke, I just wanted to make sure nothing happened to you. :/
Dr. Green /: Hmm? Do I sense a bit of concern? :/
Dr. Green /: Don't tell me you're actually sorry for what you said. :/
Enviro /: I... Of course not... Afterall, you could be lying to me. :/
Dr. Green /: That might be true. But I am very glad you called me. :/
Enviro /: ...I just wanted to make sure you're alright. :/
Dr. Green /: Haha, I appreciate that. :/
Dr. Green /: I am also delighted to see that your journey is progressing smoothly. :/
Enviro /: Thank you for that, doc... I mean it. :/
***!

>>>>! h
Enviro /: Hello? Doctor Green, do you copy? Over. :/
Dr. Green /: Huh? Enviro?? You Actually called? :/
Enviro /: It's...  It's been a while since we last spoke, I just wanted to make sure nothing happened to you. :/
Dr. Green /: Hmm? Do I sense a bit of concern? :/
Dr. Green /: Don't tell me you're actually sorry for what you said. :/
Enviro /: I... Of course not... Afterall, you could be lying to me. :/
Dr. Green /: That might be true. But I am very glad you called me. :/
Enviro /: ...I just wanted to make sure you're alright. :/
Dr. Green /: Haha, I appreciate that. :/
Dr. Green /: I am also delighted to see that your journey is progressing smoothly. :/
Enviro /: Thank you for that, doc... I mean it. :/
***!

>>>>! i
Enviro /: Doc? Are you there? :/
Dr. Green /: What?! You actually called me two times in a row?? :/
Dr. Green /: I didn't expect you to be even needier than me. :/
Enviro /: Yea... The thing is, I wanted to say something... :/
Dr. Green /: That apology, is it? :/
Enviro /: Why do you feel the need to make it even harder... :/
Dr. Green /: Hahaha *cough* *cough*, I already told you, you don't have to worry about it. :/
Enviro /: Yes, you said that, but I cannot accept the fact that I said that. :/
Enviro /: I genuinely feel sorry for what I have said to you. :/
Enviro /: I doubted you up until now, but... Constantly seeing the future like it is and everything... :/
Enviro /: I realised you might tell me the truth... There really are no more humans alive beside you... :/
Enviro /: And that is the reason I want to apologise for doubting you for so long. :/
Dr. Green /: I... Didn't expect that, Enviro, but I am incredibly glad to hear this from you. :/
Enviro /: That is all that I had to say for now, I have to go save the world. :/
Dr. Green /: Haha! *cough* *cough* That's the spirit! Go get them! :/
***!

>>>>! j
Enviro /: Doctor Green, can you hear me? Over. :/
Enviro /: ... :/
Enviro /: ... :/
Enviro /: Doctor Green? :/
Enviro /: ... :/
Enviro /: ... :/
Enviro /: Maybe it's the tunnels... :/
***!

>>>>! k
Dr. Green /: I cannot believe this... You really got there! :/
Enviro /: Yea... It definitely wasn't a small journey, but finally, I will save everyone... :/
Enviro /: ... :/
Enviro /: ... :/
Dr. Green /: ...Anything the matter, Enviro? :/
Enviro /: Oh... No... :/
Enviro /: It's just that... I was wondering... :/
Enviro /: If I destroy their HQ and prevent them from poluting this planet, what's gonna happen with us? :/
Dr. Green /: ...I was afraid you might think of that. :/
Enviro /: Theoretically, if the world is saved, there is no need for me to be built... And no way for us to meet... :/
Dr. Green /: ... :/
Dr. Green /: ...I know... I thought about that as well. :/
Dr. Green /: I do not know if the future we are trying to write is a better one, but all I can have is hope that everything will be better. :/
Dr. Green /: And just like that, I also have hope that we will still meet eachother, regardless of what timeline we are in. :/
Enviro /: ...Perhaps... :/
Enviro /: If you genuinely believe that, doc, then I, also, have hope this is for the better future. :/
Dr. Green /: ... :/
Dr. Green /: Thank you, Enviro... :/
Enviro /: Let's do this! :/
***!";




//MemoryStream cacat = new MemoryStream(data, 0, data.Length);


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
		char index = (char)(indez+(char)96);


/*        string targetFile;

        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
            targetFile = @"Assets\Resources\Dialogue.txt";
        else // Platform is not Windows
        {
            targetFile = @"Assets/Resources/Dialogue.txt";
        }*/
        
        System.IO.StringReader file = new StringReader( dialogueText );
        
        string line;
        rawDialogue = new List<string>();

        while ((line = file.ReadLine()) != null)
        {
            if (line.Contains(dialogueSignal))
            {
                if (line.Contains( index.ToString() /*index.ToString()*/)) //if desired dialogue
                {
					Debug.Log("Bula numara"+index.ToString());
                    while (!(line = file.ReadLine()).Contains(dialogueEndSignal))
                    {
                        rawDialogue.Add(line);
                        //Debug.Log(line);
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

            Debug.Log("Prostu spune:"  +speakingCharacter[i] + ": " + dialogueLines[i]);
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
