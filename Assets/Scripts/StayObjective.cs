using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StayObjective : MonoBehaviour
{
    [System.Serializable]
    public class PlayerTime
    {
        public string id;
        public float time;
        public Color color;

        public PlayerTime(string id, Color color, float time)
        {
            this.id = id;
            this.color = color;
            this.time = time;
        }
    }

    [Header("References")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ScalePercentage scalePercentage;
    [SerializeField] private SpriteRenderer _renderer;

    [Header("Settings")]
    [SerializeField] private float requiredTime = 5f; // in seconds
    [SerializeField] private bool useRandomColor = false;

    [Header("GameEnd")]
    [SerializeField] private GameLoader loader;

    [Header("Runtime")]
    [SerializeField] private PlayerTime currentPlayerTime;
    [SerializeField] private List<MovementScript> players = new();

    void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        if (_collider == null)
            Debug.LogError("No collider attached to the StayObjective.");
    }

    void Update()
    {
        if (currentPlayerTime == null) return;
        if (players.Count > 1) return;

        if (players.Count == 1)
        {
            if (currentPlayerTime.id == players[0].playerId)
                currentPlayerTime.time++;
            else
                currentPlayerTime.time--;

            if (currentPlayerTime.time <= 0)
            {
                Color color = GetPlayerColor(players[0]);
                currentPlayerTime = new PlayerTime(players[0].playerId, color, 0);
                _renderer.color = color;
            }
        }

        if (currentPlayerTime.time >= requiredTime)
        {
            Debug.Log($"Player {currentPlayerTime.id} won!");
            loader.LoadScene();
        }

        scalePercentage.percentage = currentPlayerTime.time / requiredTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        MovementScript playerMovement = collision.gameObject.GetComponent<MovementScript>();
        if (playerMovement != null)
        {
            players.Add(playerMovement);

            if (currentPlayerTime == null)
            {
                Color color = GetPlayerColor(playerMovement);
                currentPlayerTime = new PlayerTime(playerMovement.playerId, color, 0);
                _renderer.color = color;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        MovementScript playerMovement = other.gameObject.GetComponent<MovementScript>();
        if (playerMovement != null)
        {
            players.Remove(playerMovement);
        }
    }

    private Color GetPlayerColor(MovementScript player)
    {
        if (useRandomColor)
            return new Color(Random.value, Random.value, Random.value);

        ColorIdentity id = player.GetComponent<ColorIdentity>();
        return id != null ? id.color : Color.white;
    }
}
