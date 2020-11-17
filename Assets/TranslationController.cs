using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslationController : MonoBehaviour
{
    
    public enum KEY
    {
        CONTINUE,
        START_GAME,
        SELECT_LEVEL,
        CUSTOMIZE,
        CREDITS,
        QUIT,
        
        BACK_TO_MENU,
        LOAD_LEVEL,
        STANDARD_ROBOT,
        COPPER,
        BRAND_NEW,
        AMETHYST,
        SHINY,
        SCRAPS,
        COST,
        ACTIVATE,
        PURCHASE,
        NOT_ENOUGH_SCRAP,
        ACTIVATED,
        CUSTOMIZE_TITLE,
        PAUSED_TITLE,
        RESUME,
        NEXT_LEVEL
    }
    
    public Translatable[] text;
    private Dictionary<KEY, string> translationsEN;
    private Dictionary<KEY, string> translationsDE;
    
    private string[] _npcSentenceDE, _npcSentenceEN;
    
    private void Awake()
    {
        GameManager.Instance.translationController = this;
        prepareLanguage();
    }

    public void Start()
    {
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
                        text[i].SetText(translationsDE[text[i].key]);
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
                        text[i].SetText(translationsEN[text[i].key]);
                    else
                    {
                        Debug.LogWarning("Key: " + text[i].key + " is not found in " + language + text[i].transform.parent.name);
                        text[i].debug();
                    }
                }
                break;
        }
    }

    public string GetTranslation(KEY key, string language = "")
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
        return key.ToString();
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
        translationsDE = new Dictionary<KEY, string>();
        
        translationsDE.Add(KEY.CONTINUE, ">Weiter");
        translationsDE.Add(KEY.START_GAME, ">Spiel starten");
        translationsDE.Add(KEY.SELECT_LEVEL, ">Level wählen");
        translationsDE.Add(KEY.CUSTOMIZE, ">Anpassen");
        translationsDE.Add(KEY.CREDITS, ">Nachspann");
        translationsDE.Add(KEY.QUIT, ">Schließen");
        
        translationsDE.Add(KEY.CUSTOMIZE_TITLE, "Anpassen:_");
        translationsDE.Add(KEY.BACK_TO_MENU, ">Zurück zum Hauptmenü");        
        translationsDE.Add(KEY.LOAD_LEVEL, "LevelLaden:_");
        translationsDE.Add(KEY.STANDARD_ROBOT, ">StandardRobot");
        translationsDE.Add(KEY.COPPER, ">Kupfer");
        translationsDE.Add(KEY.BRAND_NEW, ">Brandneu");
        translationsDE.Add(KEY.AMETHYST, ">Amethyst");
        translationsDE.Add(KEY.SHINY, ">Glänzend");
        translationsDE.Add(KEY.SCRAPS, ">Schrott");
        translationsDE.Add(KEY.COST, "Kosten :");
        translationsDE.Add(KEY.ACTIVATE, "[Aktivieren]");
        translationsDE.Add(KEY.PURCHASE, "[Einkauf]");
        translationsDE.Add(KEY.NOT_ENOUGH_SCRAP, "Nicht genügend Schrott _");
        translationsDE.Add(KEY.ACTIVATED, "Aktiviert");
        
        translationsDE.Add(KEY.RESUME, ">Fortsetzen"); // Check those translations
        translationsDE.Add(KEY.NEXT_LEVEL, ">Nächste Level");
        translationsDE.Add(KEY.PAUSED_TITLE, "Pausieren:/");
           

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
        translationsEN = new Dictionary<KEY, string>();

        translationsEN.Add(KEY.CONTINUE, ">Continue");
        translationsEN.Add(KEY.START_GAME, ">Start Game");
        translationsEN.Add(KEY.SELECT_LEVEL, ">Select Level");
        translationsEN.Add(KEY.CUSTOMIZE, ">Customize");
        translationsEN.Add(KEY.CREDITS, ">Credits");
        translationsEN.Add(KEY.QUIT, ">Quit");
        
        translationsEN.Add(KEY.CUSTOMIZE_TITLE, "Customize:_");
        translationsEN.Add(KEY.BACK_TO_MENU, ">Back to menu");
        translationsEN.Add(KEY.LOAD_LEVEL, "LoadLevel:_");
        translationsEN.Add(KEY.STANDARD_ROBOT, ">StandardRobot");
        translationsEN.Add(KEY.COPPER, ">Copper");
        translationsEN.Add(KEY.BRAND_NEW, ">Brand New");
        translationsEN.Add(KEY.AMETHYST, ">Amethyst");
        translationsEN.Add(KEY.SHINY, ">Shiny");
        translationsEN.Add(KEY.SCRAPS, ">Scraps");
        translationsEN.Add(KEY.COST, "Cost :");
        translationsEN.Add(KEY.ACTIVATE, "[Activate]");
        translationsEN.Add(KEY.PURCHASE, "[Purchase]");
        translationsEN.Add(KEY.NOT_ENOUGH_SCRAP, "Not enough Scrap _");
        translationsEN.Add(KEY.ACTIVATED, "Activated");
        
        translationsEN.Add(KEY.RESUME, ">Resume");
        translationsEN.Add(KEY.NEXT_LEVEL, ">Next Level");
        translationsEN.Add(KEY.PAUSED_TITLE, "Paused:/");

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
