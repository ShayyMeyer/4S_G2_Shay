using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SimpleEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private bool isRotating = false;
    [SerializeField] private float rotatingSpeed = 20f;
    
    private NavMeshAgent agent;
    
    public bool hasTarget = false;
    [SerializeField] private Transform agentTarget;
    [SerializeField] private GameObject gameOverScreen;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameOverScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    
    private void Update()
    {
        if (hasTarget)
        {
            agent.SetDestination(agentTarget.position);
        }
        else
        {
            if (isRotating)
            {
                transform.Rotate(transform.up, rotatingSpeed * Time.deltaTime);
            }
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
