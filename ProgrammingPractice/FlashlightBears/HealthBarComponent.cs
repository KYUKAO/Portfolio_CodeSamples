using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : MonoBehaviour
{
    PlayerComponent Player;
    Slider slider;
    public enum HealthBarID
    {
        P1,
        P2,
    }
    public HealthBarID healthBarID;
    void Start()
    {
        slider = this.GetComponent<Slider>();
        var players = FindObjectsOfType<PlayerComponent>();
        foreach (var player in players)
        {
            if (healthBarID == HealthBarID.P1)
            {
                if (player.ThisPlayerID == PlayerComponent.PlayerID._P1)
                {
                    Player = player;
                }
            }
            if (healthBarID == HealthBarID.P2)
            {
                if (player.ThisPlayerID == PlayerComponent.PlayerID._P2)
                {
                    Player = player;
                }
            }
        }
    }

    void Update()
    {
        if (!Player) return;
        if (Player.CurrentHealth / Player.MaxHealth > 0)
        {
            slider.value = Player.CurrentHealth / Player.MaxHealth;
        }
        if (Player.CanMove == false)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
