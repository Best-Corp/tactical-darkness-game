using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<GameObject> combatants = new List<GameObject>();
    private int kills = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Health[] healths = FindObjectsOfType<Health>();
        foreach (Health h in healths)
        {
            RegisterCombatant(h.gameObject);
        }

        Debug.Log("Combattants enregistrés: " + combatants.Count);
    }

    public void RegisterCombatant(GameObject combatant)
    {
        if (!combatants.Contains(combatant))
        {
            combatants.Add(combatant);

            Health health = combatant.GetComponent<Health>();
            if (health != null)
                health.Died += HandleDied;
        }
    }

    void HandleDied(Health deadHealth)
    {
        GameObject dead = deadHealth.gameObject;

        if (!combatants.Contains(dead)) return;

        combatants.Remove(dead);
        kills++;

        Debug.Log("Élimination! " + combatants.Count + " restants. Kills: " + kills);

        MapShrink[] shrinks = FindObjectsOfType<MapShrink>();
        foreach (MapShrink s in shrinks)
        {
            s.Shrink();
        }

        if (combatants.Count <= 1)
        {
            if (combatants.Count == 1)
                Debug.Log("VICTOIRE!");
            else
                Debug.Log("MATCH NUL!");
        }
    }
}
