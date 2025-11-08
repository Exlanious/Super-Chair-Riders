using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerSettings
    {
        public GameObject player;
        public Transform spawnpoint;

        public MovementScript mov;

        public PlayerSettings(GameObject player, Transform spawnpoint)
        {
            this.player = player;
            this.spawnpoint = spawnpoint;
            this.mov = player.GetComponent<MovementScript>();
        }
    }

    public string currentWinningTeam;
    public Color team1Color, team2Color;
    public List<StayObjective> objectives;
    public SpriteRenderer endPanel;
    public GameLoader loader;


    public List<PlayerSettings> playerSettings;
    public float respawnTime = 2f;

    void Update()
    {
        //player respawn
        foreach (PlayerSettings player in playerSettings)
        {
            if (player.mov.health <= 0)
            {
                //temporary respawn
                StartCoroutine(Respawn(player));
            }
        }

        int team1WinCount = 0;
        int team2WinCount = 0;
        foreach (StayObjective objective in objectives)
        {
            switch (objective.teamWon)
            {
                case null:
                    //do nothing
                    break;
                case "Team1":
                    team1WinCount++;
                    break;
                case "Team2":
                    team2WinCount++;
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        string teamWon = null;
        if (team1WinCount >= Mathf.Ceil(objectives.Count / 2f))
        {
            endPanel.color = team1Color; teamWon = "Team 1";
        }
        if (team2WinCount >= Mathf.Ceil(objectives.Count / 2f))
        {
            endPanel.color = team2Color; teamWon = "Team 2";
        }

        if (teamWon != null)
        {
            Debug.Log($"Team {teamWon} captured the objective!");
            loader.LoadSceneWithCoverUp();
        }
    }

    //temporary respawn
    private IEnumerator Respawn(PlayerSettings player)
    {
        player.player.SetActive(false);
        player.player.transform.position = player.spawnpoint.position;
        player.player.transform.rotation = player.spawnpoint.rotation;
        yield return new WaitForSeconds(respawnTime);
        player.mov.health = player.mov.maxHealth;
        player.player.SetActive(true);
    }
}
