using RPG.Players;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [field: SerializeField] public Player Player { get; private set; }

    
}
