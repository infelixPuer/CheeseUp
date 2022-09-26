using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement _pm;

    private void Awake()
    {
        _pm = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_pm == null) { return; }

        if (collision.gameObject.CompareTag("Spikes") && !_pm.IsDead)
        {
            AudioManager.Instance.PlayDeathSound();
            _pm.IsDead = true;
            StartCoroutine(RestartLevel());
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}