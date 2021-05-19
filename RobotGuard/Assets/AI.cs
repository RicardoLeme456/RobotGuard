using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player; //O objeto da cena a sofrer mudança de comportamento
    public Transform bulletSpawn; //Balas a ser spawnados
    public Slider healthBar;  //Barra de Vida 
    public GameObject bulletPrefab; //Balas a ser clonados

    NavMeshAgent agent; //O componente a ser pego
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f; //Vida do Robo
    float rotSpeed = 5.0f; //A velocidade de rotação

    float visibleRange = 80.0f; //O alcance a ser visivel pelo robo
    float shotRange = 40.0f; //O alcance do tiro

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>(); //Pegar o componente do NavMeshAgent
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth",5,0.5f); //Invoca o método update health em 5 segundos e repete a cada 0.5 segundos
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position); //A posição a qual vai estar o health bar em relação com  camera
        healthBar.value = (int)health; //A barra de vida a sofrer mudança
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0); //A posição do health bar na camera da cena 
    }

    void UpdateHealth()
    {
       if(health < 100) //Se a vida for menor que 100
        health ++; //Ela acrescentará uma vida
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "bullet") //Se o colisor do game object com a tag for igual a bullet
        {
            health -= 10; //Perde 10 de vida
        }
    }

    [Task]
    public void PickRandomDestination()
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)); //A posição aleatória a qaul o robo ira percorrer
        agent.SetDestination(dest); //Definir um novo caminho através das posições randomizadas
        Task.current.Succeed(); //Verifica se o Task foi concluído com sucesso ou não
    }

    [Task]
    public void MoveToDestination()
    {
        if (Task.isInspected) //Se o método do Wander do MoveToDestination é inspecionado
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time); //O wanderBT será lido
        if(agent.remainingDistance <=agent.stoppingDistance && !agent.pathPending) //Se a distância da posição do agente for menor ou igual a distancia de parada do agente e se esta sendo calculado?
        {
            Task.current.Succeed(); //Verifica se o Task foi concluído com sucesso ou não
        }
    }

}

