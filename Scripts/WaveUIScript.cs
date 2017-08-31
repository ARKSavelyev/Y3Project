using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveUIScript : MonoBehaviour {

    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    // Use this for initialization
    void Start()
    {
        if (spawner == null)
        {
            Debug.LogError("No spawner referenced!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator referenced!");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCountdownText referenced!");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText referenced!");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner.state)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        previousState = spawner.state;
    }

    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            //Debug.Log("COUNTING");
        }
        waveCountdownText.text = ((int)spawner.WaveCountDown).ToString();
    }

    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);
            int i = spawner.NextWave + 1;
            waveCountText.text = i.ToString();

            //Debug.Log("SPAWNING");
        }
    }
}
