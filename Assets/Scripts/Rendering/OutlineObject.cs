using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OutlineObject : MonoBehaviour
{
    public StateMachine PlayerStateMachine;
    public List<GameObject> Enemies = new List<GameObject>();   
    public GameObject OutlineTarget;
    

    public void Start()
    {
        PlayerStateMachine = GameObject.Find("Player").GetComponent<StateMachine>();
        
    }

    private void Update()
    {
        Enemies = PlayerStateMachine.EnemiesNearby;
        if (PlayerStateMachine.CurrentTarget != null)
        {
            OutlineTarget = PlayerStateMachine.CurrentTarget;
            Outline outline = OutlineTarget.AddComponent<Outline>();

        }
        else
        {
            foreach(GameObject enemy in Enemies)
            {
                if (enemy != OutlineTarget)
                {

                }
            }
            return;
        }

    }
}
