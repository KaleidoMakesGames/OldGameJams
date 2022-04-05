using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource beginning;
    public AudioSource gameLoop;
    public AudioSource defeat;
    public AudioSource victory;

    public enum GamePhase { Menu, Game, Defeat, Victory }
    public GamePhase currentPhase;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        currentPhase = GamePhase.Menu;
    }

    // Start is called before the first frame update
    void Start()
    {
        beginning.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentPhase) {
            case GamePhase.Menu:
                defeat.Stop();
                victory.Stop();
                gameLoop.Stop();
                if(!beginning.isPlaying) {
                    beginning.Play();
                }
                break;
            case GamePhase.Game:
                if(!gameLoop.isPlaying && !beginning.isPlaying) {
                    gameLoop.Play();
                }
                defeat.Stop();
                victory.Stop();
                break;
            case GamePhase.Defeat:
                gameLoop.Stop();
                beginning.Stop();
                victory.Stop();
                defeat.Play();
                break;
            case GamePhase.Victory:
                gameLoop.Stop();
                beginning.Stop();
                defeat.Stop();
                victory.Play();
                break;
        }
    }
}
