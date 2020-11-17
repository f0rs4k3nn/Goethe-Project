using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslationController : MonoBehaviour
{
    public Translatable[] text;
    private Dictionary<string, string> translationsEN;
    private Dictionary<string, string> translationsDE;
    private string[] _npcSentenceDE, _npcSentenceEN;
    private void Awake()
    {
        GameManager.Instance.translationController = this;
    }

    public void Start()
    {
        prepareLanguage();
        RefreshLanguage(GameManager.Instance.Language);
    }


    public void RefreshLanguage(string language = "")
    {
        if (language == "")
            language = GameManager.Instance.Language;
        
        switch (language)
        {
            case "de_DE":
                for (int i = 0; i < text.Length; ++i)
                {
                    if(translationsDE.ContainsKey(text[i].key))
                        text[i].Text = translationsDE[text[i].key];
                    else
                    {
                        Debug.LogWarning("Key: " + text[i].key + " is not found in " + language + text[i].transform.parent.name);
                        text[i].debug();
                        
                    }
                }
                break;
            default:
                for (int i = 0; i < text.Length; ++i)
                {
                    if(translationsEN.ContainsKey(text[i].key))
                        text[i].Text = translationsEN[text[i].key];
                    else
                    {
                        Debug.LogWarning("Key: " + text[i].key + " is not found in " + language + text[i].transform.parent.name);
                        text[i].debug();
                    }
                }
                break;
        }
    }

    public string GetTranslation(string key, string language = "")
    {
        if (language == "")
            language = GameManager.Instance.Language;
            
        switch (language)
        {
            case "de_DE":
                if(translationsDE.ContainsKey(key))
                    return translationsDE[key];
                else
                    Debug.LogWarning("Key: " + key + " is not found in Deutsch");
                break;
            default:
                if(translationsEN.ContainsKey(key))
                    return translationsEN[key];
                else
                    Debug.LogWarning("Key: " + key + " is not found in English");
                break;
        }
        return key;
    }
    
    public string GetNPCSentence(int index)
    {
        if (index < 0)
            return "";
        switch (GameManager.Instance.Language)
        {
            case "de_DE":
                if (index < translationsDE.Count)
                    return _npcSentenceDE[index]; 
                else
                    Debug.LogWarning(index + " in Language DE out of bounds");
                break;
            default:
                if (index < translationsEN.Count)
                    return _npcSentenceEN[index]; 
                else
                    Debug.LogWarning(index + " in Language EN out of bounds");
                break;
        }

        return "";
    }
    
    private void prepareLanguage()
    {
        // de_DE
        translationsDE = new Dictionary<string, string>();
        
        translationsDE.Add(">Continue", ">Weiter");
        translationsDE.Add(">Start Game", ">Spiel starten");
        translationsDE.Add(">Select Level", ">Level wählen");
        translationsDE.Add(">Customize", ">Anpassen");
        translationsDE.Add(">Credits", ">Nachspann");
        translationsDE.Add(">Quit", ">Schließen");
        
        translationsDE.Add(">Back to menu", ">Zurück zum Hauptmenü");
        translationsDE.Add(">StandardRobot", ">StandardRobot");
        translationsDE.Add(">Copper", ">Kupfer");
        translationsDE.Add(">Brand New", ">Brandneu");
        translationsDE.Add(">Amethyst", ">Amethyst");
        translationsDE.Add(">Shiny", ">Glänzend");
        translationsDE.Add(">Scraps", ">Schrott");
        translationsDE.Add("Cost :", "Kosten :");
        translationsDE.Add("[Activate]", "[Aktivieren]");
        translationsDE.Add("[Purchase]", "[Einkauf]");
        translationsDE.Add("Not enough Scrap _", "Nicht genügend Schrott _");
        translationsDE.Add("Activated", "Aktiviert");

        _npcSentenceDE = new []
        {
            "Wussten Sie, dass das weggeworfene Papier in jedem Jahr ca. 640 Millionen Bäumen entspricht?",
            "Wussten Sie, dass beim Abholzen des Walds das Feuchtigkeitsniveau abnimmt und die Pflanzen in der Nähe austrocknen?",
            "Wussten Sie, dass die Bäume mit Hilfe ihrer Wurzeln Wasser absorbieren und speichern und durch die Abholzung der Boden die Fähigkeit verliert, das Wasser zu speichern? Das führt zu Flut in manchen Gebieten und zu Dürre in anderen!",
            "Wussten Sie, dass fast 70% des industriellen Abfalls in Gewässer geworfen wird und zu der Verschmutzung der nutzbaren Wasserversorgung führt?",
            "Wussten Sie, dass die Bäume eine große Rolle in der Kontrolle der globalen Erwärmung spielen? Sie nutzen Treibhausgase und stellen das Gleichgewicht in der Atmosphäre wieder her.",
            "Wussten Sie, dass einige Tiere und Pflanzen überall auf der Welt an ihren natürlichen Lebensraum gewöhnt sind? Die Abholzung ihres natürlichen Lebensraums macht es schwieriger für sie, sich an eine neue Umwelt anzupassen.",
            "Wussten Sie, dass durch jede Glastonne, die recycelt wird, die Freigabe von 315 Kilogramm Kohlendioxyd in die Atmosphäre vorgebeugt werden kann?",
            "Wussten Sie, dass das Recyceln von Textilien zur Minimierung der Ackerland-, Wasser- und Düngemittelnutzung führt, was für das Baumwollwachstum genutzt wird?",
            "Wussten Sie, dass das Recyceln Enviro helfen kann, neue Skins zu entsperren?",
            "Die Verschlechterung der Umwelt diskriminiert nicht und auch wir sollten das nicht tun!",
            "Die Vielfalt ist ein Geschenk des Lebens! Unsere Unterschiede machen uns stärker!",
            "Auge für Auge wird die ganze Welt blind machen. Erinnere dich daran, dass Gewalt niemals zur Konfliktlösung beiträgt. Es ist besser die Probleme durch Kommunikation zu lösen."
        };
        
        // en_GB
        translationsEN = new Dictionary<string, string>();

        translationsEN.Add(">Continue", ">Continue");
        translationsEN.Add(">Start Game", ">Start Game");
        translationsEN.Add(">Select Level", ">Select Level");
        translationsEN.Add(">Customize", ">Customize");
        translationsEN.Add(">Credits", ">Credits");
        translationsEN.Add(">Quit", ">Quit");
        
        translationsEN.Add(">Back to menu", ">Back to menu");
        translationsEN.Add(">StandardRobot", ">StandardRobot");
        translationsEN.Add(">Copper", ">Copper");
        translationsEN.Add(">Brand New", ">Brand New");
        translationsEN.Add(">Amethyst", ">Amethyst");
        translationsEN.Add(">Shiny", ">Shiny");
        translationsEN.Add(">Scraps", ">Scraps");
        translationsEN.Add("Cost :", "Cost :");
        translationsEN.Add("[Activate]", "[Activate]");
        translationsEN.Add("[Purchase]", "[Purchase]");
        translationsEN.Add("Not enough Scrap _", "Not enough Scrap _");

        _npcSentenceEN = new[]
        {
            "Did you know that the paper thrown out each year is equivalent to approximately 640 million trees?", 
            "Did you know that when a forest is cut down the humidity level goes down and the nearby plants dry out?",
            "Did you know that trees absorb and store water using their roots and when they are cut down the soil loses the ability to retain water? This leads to floods in some areas and drought in others!",
            "Did you know that almost 70% of the industrial waste is tossed into the water bodies and this causes the pollution of the usable water supplies?",
            "Did you know that several animals and plants across the world are accustomed to their natural habitat? The deforestation of their natural habitats will make it difficult for them to adapt to a new environment.",
            "Did you know that recycling one aluminum can save enough energy to power a TV for two hours?",
            "Did you know that for every ton of glass you recycle prevents 315 kilograms of carbon dioxide from being released into the atmosphere?",
            "Did you know that recycling textile materials results in less land, water and fertilizer used for cotton growth?",
            "Did you know that recycling can help Enviro unlock new skins?",
            "Environment degradation does not discriminate and neither should we!",
            "Diversity is a gift of life! Our differences make us stronger!",
            "An eye for an eye will make the whole world blind. Remember, never resort to violence to solve a conflict. It’s better to solve the problems through communication."
        };
    }
}
