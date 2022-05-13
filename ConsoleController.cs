using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class controls input into the console for "rc" commands.
/// </summary>
public class ConsoleController
{

    public static void SetBool(string what, string input, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("set {0} {1}", what, input);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);

    }


    /// <summary>
    /// Change the respawn time of players.
    /// </summary>
    /// <param name="time"></param>
    public static void ChangeRespawnTime(float time, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("set characterRespawnTime {0}", time);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);

    }

    public static void DelayChangeRespawnTime(float time, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("delayed {0} set characterRespawnTime {1}", MainScript.timeGame - 3, time);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);

    }

    /// <summary>
    /// broadcast without admin prefix added
    /// </summary>
    /// <param name="message"></param>
    public static void Broadcast(string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("broadcast {0}", message);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    public static void DelayedBroadcast(string message, float time, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("delayed {0} broadcast {1}", time, message);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    /// <summary>
    /// private message the player (with small delay)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="when"></param>
    public static void PrivateMessage(int playerID, string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("serverAdmin privateMessage " + playerID + " " + message);
    }


    #region NOT USED

    public static void SendInternalCharge(string message, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke(message);
    }

    /// <summary>
    /// Revives the player.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    public static void RevivePlayer(int playerID, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin revive {0}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    } // Not used

    public static void RevivePlayerDelayed(int playerID, string reason, int time, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("delayed {0} serverAdmin revive {1} ", time, playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    } //Not used

    /// <summary>
    /// What is says on the tin.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    public static void SlayPlayer(int playerID, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin slay {0} {1}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }  // Not used

    /// <summary>
    /// Yup slaps them.
    /// TODO: add a pm. 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="damege"></param>
    /// <param name="reason"></param>
    public static void SlapPlayer(int playerID, int damege, string reason, InputField f1MenuInputField)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("serverAdmin slap {0} {1} {2}", playerID, damege, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }

    #endregion 
}