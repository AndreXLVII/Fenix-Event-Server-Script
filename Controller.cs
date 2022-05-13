using HoldfastSharedMethods;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class updatedController : IHoldfastSharedMethods
{

    public void OnIsServer(bool server)
    {
        //Code from Wrex: https://github.com/CM2Walki/HoldfastMods/blob/master/NoShoutsAllowed/NoShoutsAllowed.cs

        //Get all the canvas items in the game
        var canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            //Find the one that's called "Game Console Panel"
            if (string.Compare(canvases[i].name, "Game Console Panel", true) == 0)
            {
                //Inside this, now we need to find the input field where the player types messages.
                MainScript.f1MenuInputField = canvases[i].GetComponentInChildren<InputField>(true);
                if (MainScript.f1MenuInputField != null)
                {
                    Debug.Log("FENIX MOD:Found the Game Console Panel");
                }
                else
                {
                    Debug.Log("FENIX MOD:We did Not find Game Console Panel");
                }
                break;
            }
        }
        Debug.LogFormat("FENIX MOD:IsServer {0}", server);
    }

    public void OnIsClient(bool client, ulong steamId)
    {
        Debug.LogFormat("IsClient {0} {1}", client, steamId);
    }

    #region Player Join/leave 
    public void OnPlayerJoined(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
    {
        Players.PlayerJoined(playerId,steamId,playerName,regimentTag,isBot);
    }

    public void OnPlayerLeft(int playerId)
    {
        Players.PlayerLeft(playerId);
    }
    #endregion


    #region CONSOLE COMMANDS

    public void OnConsoleCommand(string input, string output, bool success)
    {
        //Debug.LogFormat("OnConsoleCommand {0} {1} {2}", input, output, success);
        if (success && MainScript.gameStarted)
        {
            if (MainScript.CheckCommandIssued(input) == 1)  // If commmand issued is slay
            {
                MainScript.deadPeopleBefDep++;
            }
        }
    }

    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
    {
        //CustomCommands.OnRCLogin(playerId, inputPassword, isLoggedIn);
        //Debug.LogFormat("OnRCLogin {0} {1} {2}", playerId, inputPassword, isLoggedIn);
    }

    public void OnRCCommand(int playerId, string input, string output, bool success)
    {
        //CustomCommands.OnRCCommand(playerId, input, output, success);
        //Debug.LogFormat("OnRCCommand {0} {1} {2} {3}", playerId, input, output, success);

    }

    #endregion

    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject)
    {
        Players.PlayerSpawned(playerId , spawnSectionId, playerFaction, playerClass,  uniformId);
    }

    #region Shoot or kill
    public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string additionalDetails)
    {
        MainScript.deadPeopleBefDep++;

    }

    public void OnPlayerShoot(int playerId, bool dryShot)
    {

    }
    #endregion

    #region TIME
    public void OnUpdateElapsedTime(float time)
    {
        //Debug.LogWarningFormat("OnUpdateElapsedTime {0}", time);
        MainScript.timePassed = (int)time;
    }
    public void OnUpdateTimeRemaining(float time)
    {
        MainScript.timeGame = (int)time;

        if (!MainScript.inLobby) { 

            if (time <= MainScript.timeLive && !MainScript.gameStarted) MainScript.AtLive(); // Calls live

            if (time <= MainScript.timeCharge && !MainScript.chargeStarted) MainScript.LastCharge(); // Calls last charge

            if (time <= MainScript.timeNextDeploy && !MainScript.chargeStarted && MainScript.gameStarted) MainScript.Deployment(); // Reinforces Deployment

        }
    }
    #endregion

    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
    {
       
        Debug.LogFormat("FENIX MOD: RoundDetails {0} {1} {2} {3} {4} {5} {6}", roundId, serverName, mapName, attackingFaction, defendingFaction, gameplayMode, gameType);
        MainScript.CheckLobby(mapName); // Call check if it is one of the lobby maps

  
    }
    public void OnTextMessage(int playerId, TextChatChannel channel, string text)
    {
       
    }

    #region CONQUEST
    public void OnCapturePointCaptured(int capturePoint)
    {
        //Debug.LogFormat("OnCapturePointCaptured {0}", capturePoint);
    }

    public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry)
    {
        //Debug.LogFormat("OnCapturePointOwnerChanged {0} {1}", capturePoint, factionCountry.ToString());

    }

    public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount)
    {
        //Debug.LogFormat("OnCapturePointDataUpdated {0} {1} {2}", capturePoint, defendingPlayerCount, attackingPlayerCount);
    }

    #endregion

    #region Variable passing 

    public void PassConfigVariables(string[] value)
    {

    }

    public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason)
    {
        if (action == ServerAdminAction.Slay)
        {
            MainScript.deadPeopleBefDep++;
        }

    }

    public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject, InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
    {
    }

    public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
    {
    }


    #endregion

    #region NOT USED METHODS




    public void OnSyncValueState(int value)
    {
    }

    public void OnUpdateSyncedTime(double time)
    {
    }
    public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {
    }

    public void OnScorableAction(int playerId, int score, ScorableActionType reason)
    {
    }

    public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId)
    {
    }

    public void OnPlayerMeleeStartSecondaryAttack(int playerId)
    {
    }

    public void OnPlayerWeaponSwitch(int playerId, string weapon)
    {
    }

    public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason)
    {
    }

    public void OnRoundEndPlayerWinner(int playerId)
    {
    }

    public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject)
    {
    }

    public void OnPlayerEndCarry(int playerId)
    {
    }

    public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase)
    {
    }

    public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType)
    {
    }

    public void OnEmplacementConstructed(int itemId)
    {
    }

    public void OnBuffStart(int playerId, BuffType buff)
    {
    }

    public void OnBuffStop(int playerId, BuffType buff)
    {
    }

    public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances, float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles, float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues, float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections, int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds)
    {

    }

    public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject, int ownerPlayerId)
    {
    }

    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {
    }

    public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details)
    {
    }

    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipNameId)
    {
    }

    public void OnShipDamaged(int shipId, int oldHp, int newHp)
    {
    }

    #endregion


}