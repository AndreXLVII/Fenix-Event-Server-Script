using HoldfastSharedMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{

    // Implementare un sistema di salvataggio score? Annuncio vittoria di impero/coalizione

    public static int timeLive = 1120; // 18:40
    public static int timeToRespawn = 100; // 1:40
    public static int timeToRespawnBefLive = 5; // 0:05
    public static int timeToRespawnMax = 100000; // max
    public static int timeCharge = 120; // 120 = 2:00;
    public static int timeAnnounceBeforeCharge = 30; // time of the announcement message before charge
    public static int timeAnnounceCharge = 0; // calculated at PreLive()
    public static int timeGame = -1;
    public static int timePassed = -1;

    public static int randomMsg = -1; // number message for event broadcast flavour

    public static int timeNextDeploy = -1;
    public static int deadPeopleBefDep = 0; // People dead before deployment
    public static int numDeploy = 0;

    public static bool gameStarted = false;
    public static bool chargeStarted = false;
    public static bool inLobby = false;

    public static string[] regimentTags = { "gde", "telm", "fenix", "4teri", "3rd", "ir54", "admin"};

    public static InputField f1MenuInputField;

    #region Strings

    //public static Dictionary<int, playerStruct> playerDictionary = new Dictionary<int, playerStruct>();
    public static string[] lobbyMaps = { "marquette", "spanishfarm", "lamarshfen" };

    public static string messMeschia = "Bentornato al tuo settimanale impiego Meschia!";
    public static string messAra = "Il campo di battaglia Vi attende, Colonnello";
    public static string messBarba = "Saluti, Maggiore, la truppa Vi attende";
    public static string messJack = "Ossequi, Colonnello in 2a, il reggimento si sta preparando allo scontro";
    public static string messAndre = "Austriae Est Imperare Orbi Universo";
    public static string defaultMess = "Greetings, thank you for joining us in our friday event!";

    #endregion



    public static void Greetings(int playerId, ulong steamId, string name)
    {

        var foreigner = true; // initialise foreigner statement

        foreach (string tag in regimentTags)  // "login" system 
        {
            if (name.ToLower().Contains(tag)) // if on of the regimentTags is found in the player name than its not counted as foreigner.
            {
                foreigner = false; 
                Debug.Log("FENIX MOD: Player with OK tag detected (" + name + ")");
                break;
            }
        }

        if (foreigner) // Warning if foreigner player detected
        {
            ConsoleController.PrivateMessage(playerId, "{WARNING} No known regiment TAG recognized in your name, you are at risk of kick. If it's a mistake pls contact an admin",f1MenuInputField);
            Debug.Log("FENIX MOD: Player with foreign tag detected (" + name + ")");
        }

        if (inLobby && !foreigner) // Lobby greetings
        {
            switch (steamId)
            {
                case 76561198006563901: ConsoleController.PrivateMessage(playerId, messAra, f1MenuInputField); break;
                case 76561198203697996: ConsoleController.PrivateMessage(playerId, messMeschia, f1MenuInputField); break;
                case 76561198036280127: ConsoleController.PrivateMessage(playerId, messBarba, f1MenuInputField); break;
                case 76561198132279953: ConsoleController.PrivateMessage(playerId, messJack, f1MenuInputField); break;
                case 76561198287972137: ConsoleController.PrivateMessage(playerId, messAndre, f1MenuInputField); break;

                default:

                    var rndm = UnityEngine.Random.Range(0, 3);

                    if (name.ToLower().Contains("gde") && rndm == 0) { ConsoleController.PrivateMessage(playerId, "Good evening dear GDE ally, the battlefield awaits you!", f1MenuInputField); break; }
                    if (name.ToLower().Contains("gde") && rndm == 1) { ConsoleController.PrivateMessage(playerId, "Welcome fellow GDE member, enjoy the event!", f1MenuInputField); break; }
                    if (name.ToLower().Contains("gde") && rndm == 2) {ConsoleController.PrivateMessage(playerId, "Préparez votre mousquet Grognard de l'Empereur!", f1MenuInputField); break; }

                    if (name.ToLower().Contains("rrcg") && rndm == 0) {ConsoleController.PrivateMessage(playerId, "Welcome back dear RRCG ally, get your musket ready!", f1MenuInputField); break;}
                    if (name.ToLower().Contains("rrcg") && rndm == 1) {ConsoleController.PrivateMessage(playerId, "Greetings RRCG friend, enjoy the event!", f1MenuInputField); break; }
                    if (name.ToLower().Contains("rrcg") && rndm == 2) {ConsoleController.PrivateMessage(playerId, "Good evening fellow RRCG member, get ready for the battle!", f1MenuInputField); break; }

                    if (name.ToLower().Contains("fenix") && rndm == 0) {ConsoleController.PrivateMessage(playerId, "Buonasera membro del Fenix, la battaglia ti attende", f1MenuInputField); break; }
                    if (name.ToLower().Contains("fenix") && rndm == 1) {ConsoleController.PrivateMessage(playerId, "Salve soldato del 7° di linea, lo scontro sta per cominciare!", f1MenuInputField); break; }
                    if (name.ToLower().Contains("fenix") && rndm == 2) {ConsoleController.PrivateMessage(playerId, "Ehilà, benvenuto al nostro evento battleground!", f1MenuInputField); break; }

                    ConsoleController.PrivateMessage(playerId, defaultMess, f1MenuInputField); break;
            }
        }

    } 

    public static int CheckCommandIssued(string command)
    {
        // Check what command has been sent

        var commandSlay = "serverAdmin slay";

        if (command.Contains(commandSlay)) return 1; // returns 1 if is slay command

        else return 0; // returns 0 if is any other command

    }
    public static void CheckLobby(string mapName)
    {
        inLobby = false; // initialise inLobby statement

        if (lobbyMaps.Contains(mapName.ToLower()))
        {
            inLobby = true; Debug.Log("FENIX MOD: Lobby Map Found");
            InLobby();
        }
        else
        {
            PreLive(); Debug.Log("FENIX MOD: " + mapName + " is not a lobby map, Loading Pre Live");
        }
    }


    public static void InLobby()
    {
        Debug.Log("FENIX MOD: *** Lobby *** ");

    }


    #region Round methods
    public static void PreLive()
    {
        Debug.Log("FENIX MOD: *** Pre live *** ");

        // Initialise all game variables
        gameStarted = false;          
        chargeStarted = false;
        deadPeopleBefDep = 0;
        numDeploy = 0;
        timeNextDeploy = -1;
        timeAnnounceCharge = timeCharge + timeAnnounceBeforeCharge; // When to annouce the charge
        randomMsg = UnityEngine.Random.Range(0, 3);

        ConsoleController.SetBool("allowFiring", "false", f1MenuInputField); // 
        ConsoleController.SetBool("characterGodMode", "true", f1MenuInputField);
        ConsoleController.ChangeRespawnTime(timeToRespawnBefLive, f1MenuInputField); // Change respawn time

        DelayedBroadcast("Live at "+FormatTime(timeLive), timeLive + 30);

        DelayedBroadcast("Live in 3...", timeLive + 4);
        DelayedBroadcast("Live in 2...", timeLive + 3);
        DelayedBroadcast("Live in 1...", timeLive + 2);

    }


    public static void AtLive()
    {
        gameStarted = true;

        Debug.Log("FENIX MOD: *** Live *** ");

        ConsoleController.SetBool("allowFiring", "true", f1MenuInputField); // Enable fire
        ConsoleController.SetBool("characterGodMode", "false", f1MenuInputField); // Disable godmode

        Broadcast("LIVE! Lines may move out of spawn"); // Broadcast live

        ConsoleController.ChangeRespawnTime(timeToRespawnMax, f1MenuInputField); // Close spawn

        timeNextDeploy = timeLive - timeToRespawn; // Set time first respawns

        DelayedBroadcast("First reinforcements will join at " + FormatTime(timeNextDeploy), timeGame);

        switch (randomMsg)
        {
            case 0: DelayedBroadcast("Our reinforcements are running out!", timeAnnounceCharge); break;
            case 1: DelayedBroadcast("The battle is about to end, prepare to receive final orders!", timeAnnounceCharge); break;
            case 2: DelayedBroadcast("A final reinforce is expected in about 30 seconds", timeAnnounceCharge); break;
        } 
    }

    public static void Deployment()
    {

        numDeploy++;
        Debug.Log("FENIX MOD: numDeploy set to: " + numDeploy);

        Debug.Log("FENIX MOD: *** Deployment Nr. " + numDeploy + " ***");

        ConsoleController.ChangeRespawnTime(-1, f1MenuInputField); // Respawn time to -1 to let respawn happen

        Broadcast(deadPeopleBefDep + " reinforcements have been deployed");

        deadPeopleBefDep = 0; // Reset counter of dead players
        Debug.Log("FENIX MOD: deadPeopleBefDep set to: " + deadPeopleBefDep);

        ConsoleController.DelayChangeRespawnTime(timeToRespawnMax, f1MenuInputField); // Close respawns after deployment happened
        Debug.Log("FENIX MOD: respawnTime set to: " + timeToRespawnMax);

        Debug.Log("FENIX MOD: Deployment  Done");

        // Preparation next deployment

        if ((timeGame - timeToRespawn) > timeAnnounceCharge) // Next deployment cant be after last charge announce 
        {
            timeNextDeploy = timeNextDeploy - timeToRespawn;
            Debug.Log("FENIX MOD: timeNextDeploy set to: " + timeNextDeploy);

            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0: DelayedBroadcast("Additional men will join at " + FormatTime(timeNextDeploy), timeGame - 3); break;
                case 1: DelayedBroadcast("Further reserve troops will join by " + FormatTime(timeNextDeploy), timeGame - 3); break;
                case 2: DelayedBroadcast("Next reinforcements will join at " + FormatTime(timeNextDeploy), timeGame - 3); break;
            }
          
        }
        else
        {
            timeNextDeploy = -1; // If there's charge set it -1
            Debug.Log("FENIX MOD: timeNextDeploy set to: " + timeNextDeploy);

        }
    }

    public static void LastCharge()
    {
        chargeStarted = true;
        Debug.Log("FENIX MOD: *** Last Charge *** ");

        ConsoleController.ChangeRespawnTime(-1, f1MenuInputField); // Respawn time to -1 to let respawn happen
        Debug.Log("FENIX MOD: Character Respawn Time set to -1");

        var random = UnityEngine.Random.Range(0, 3);

        switch (random)
        {
            case 0: Broadcast("Last reinforcements deployed, commit to your last stand!");break;
            case 1: Broadcast("Our general has ordered a last offensive, victory or death!"); break;
            case 2: Broadcast("The last men have joined the battle: The final assault begins!"); break;
        }            

        ConsoleController.DelayChangeRespawnTime(timeToRespawnMax, f1MenuInputField); // Close respawns after deployment happened
        Debug.Log("FENIX MOD: Character Respawn Time set to " + timeToRespawnMax);

    }

    #endregion

    /// <summary>
    /// Broadcast without admin prefix
    /// </summary>
    /// <param name="message"></param>
    public static void Broadcast(string message)
    {
        ConsoleController.Broadcast(message, f1MenuInputField);
    }
    public static void DelayedBroadcast(string message, int time)
    {
        ConsoleController.DelayedBroadcast(message, time, f1MenuInputField);
    }

    public static string FormatTime(int time)    // From seconds to string (MM:SS)
    {
        var minutes = time / 60;
        var seconds = time % 60;
        string formattedTime;

        if (minutes == 0 && seconds == 0) formattedTime = ("00:00");
        else if(minutes == 0) formattedTime = ("00:"+seconds);
        else if (seconds == 0) formattedTime = (minutes + ":00");
        else formattedTime = (minutes + ":" + seconds);

        return formattedTime;
    }



    #region NOT USED 
    /*
    public static void PlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
       {
           playerStruct temp = new playerStruct()
           {
               _steamId = steamId,
               _name = name,
               _regimentTag = regimentTag,
               _isBot = isBot
           };

           playerDictionary[playerId] = temp;
       }

    public static void PlayerLeft(int playerId)
    {
        playerStruct temp;

        if (playerDictionary.TryGetValue(playerId, out temp) && !temp._isAlive) // If who left was dead
        {
            numberOfPlayersDead--;  // Remove from number of dead players
        }

        playerDictionary.Remove(playerId);

        */
    #endregion


}

