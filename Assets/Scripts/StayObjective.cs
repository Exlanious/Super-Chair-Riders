using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StayObjective : MonoBehaviour
{
    [System.Serializable]
    public class PlayerTime
    {
        public string teamId;
        public float time;
        public Color color;

        public PlayerTime(string teamId, Color color, float time)
        {
            this.teamId = teamId;
            this.color = color;
            this.time = time;
        }
    }

    [Header("References")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ScalePercentage scalePercentage;
    [SerializeField] private SpriteRenderer _renderer;

    [Header("Settings")]
    [SerializeField] private float requiredTime = 5f;
    [SerializeField] private Color defaultColor = Color.white;
    [Tooltip("When no players are inside the objective, this rate (units/sec) will drain capture progress back toward zero.")]
    [SerializeField] private float passiveDrainRate = 1f;

    [Header("Teams")]
    [SerializeField] private Color team1color;
    [SerializeField] private GameObject team11;
    [SerializeField] private GameObject team12;
    [SerializeField] private Color team2color;
    [SerializeField] private GameObject team21;
    [SerializeField] private GameObject team22;

    //[Header("GameEnd")]
    //[SerializeField] private GameLoader loader;

    [Header("Runtime")]
    [SerializeField] private PlayerTime currentPlayerTime;
    [SerializeField] private List<MovementScript> players = new();
    public bool locked = false;
    public string teamWon = null;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        currentPlayerTime = new PlayerTime("", defaultColor, 0);
    }

    private void FixedUpdate()
    {
        //this target has been acquired
        if (locked) { return; }

        string teamInZone;
        bool mixedTeams;
        AnalyzeTeamsInZone(out teamInZone, out mixedTeams);

        if (mixedTeams)
        {
            // Mixed teams -> decay
            currentPlayerTime.time -= Time.fixedDeltaTime;
        }
        else if (!string.IsNullOrEmpty(teamInZone))
        {
            // Single team present
            if (string.IsNullOrEmpty(currentPlayerTime.teamId))
            {
                // No current owner -> start new capture
                currentPlayerTime.teamId = teamInZone;
                currentPlayerTime.color = GetTeamColor(teamInZone);
                _renderer.color = currentPlayerTime.color;
            }
            else if (currentPlayerTime.teamId == teamInZone)
            {
                // Same team -> capture progress
                currentPlayerTime.time += Time.fixedDeltaTime;
            }
            else
            {
                // Opposing team -> must drain first
                currentPlayerTime.time -= Time.fixedDeltaTime;

                // If fully drained, switch control
                if (currentPlayerTime.time <= 0)
                {
                    currentPlayerTime.teamId = teamInZone;
                    currentPlayerTime.color = GetTeamColor(teamInZone);
                    _renderer.color = currentPlayerTime.color;
                    currentPlayerTime.time = 0; // start fresh next frame
                }
            }
        }
        else
        {
            // No players -> passive drain towards neutral
            currentPlayerTime.time -= passiveDrainRate * Time.fixedDeltaTime;
        }

        // Clamp progress
        currentPlayerTime.time = Mathf.Clamp(currentPlayerTime.time, 0, requiredTime);

        // Update scale display
        scalePercentage.percentage = currentPlayerTime.time / requiredTime;

        // Reset to neutral if fully decayed and no team holding
        if (currentPlayerTime.time <= 0 && string.IsNullOrEmpty(teamInZone))
        {
            currentPlayerTime.teamId = "";
            _renderer.color = defaultColor;
        }

        // Win condition
        if (currentPlayerTime.time >= requiredTime)
        {
            locked = true;
            teamWon = teamInZone;
            // Debug.Log($"Team {currentPlayerTime.teamId} captured the objective!");
            //loader.LoadScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovementScript player = collision.GetComponent<MovementScript>();
        if (player != null && !players.Contains(player))
            players.Add(player);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MovementScript player = collision.GetComponent<MovementScript>();
        if (player != null)
            players.Remove(player);
    }

    private void AnalyzeTeamsInZone(out string singleTeam, out bool mixedTeams)
    {
        singleTeam = null;
        mixedTeams = false;

        if (players.Count == 0)
            return;

        string firstTeam = GetPlayerTeam(players[0]);
        foreach (var player in players)
        {
            if (player == null) continue;

            string team = GetPlayerTeam(player);
            if (team != firstTeam)
            {
                mixedTeams = true;
                return;
            }
        }

        singleTeam = firstTeam;
    }

    private string GetPlayerTeam(MovementScript player)
    {
        GameObject obj = player.gameObject;

        if (obj == team11 || obj == team12)
            return "Team1";
        if (obj == team21 || obj == team22)
            return "Team2";

        return null;
    }

    private Color GetTeamColor(string teamId)
    {
        return teamId switch
        {
            "Team1" => team1color,
            "Team2" => team2color,
            _ => defaultColor
        };
    }
}
