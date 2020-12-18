using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    public Discord.Discord discord;

    // Start is called before the first frame update
    void Start()
    {
        // Make new discord client
        discord = new Discord.Discord(784584524586680391, (System.UInt16)Discord.CreateFlags.Default);

        // Get the activity manager
        var activityManager = discord.GetActivityManager();

        // The activity if in game
        var activityGame = new Discord.Activity
        {
            Details = "Adventuring the lands",
            Assets =
            {
                LargeImage = "512x512"
            }
        };

        // Set the players activity
        activityManager.UpdateActivity(activityGame, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Discord set status complete");
            } else
            {
                Debug.Log("Discord set status failed");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }

    // When the game closes
    private void OnApplicationQuit()
    {
        // Get the activity manager
        var activityManager = discord.GetActivityManager();

        // Clear the activity
        activityManager.ClearActivity((result) =>
        {
            if(result != Discord.Result.Ok)
            {
                Debug.Log("The user status has failed to been cleared");
            }
        });

        discord.Dispose();
    }
}
