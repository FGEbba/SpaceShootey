using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

/*
 * Used a Unity tutorial for help with this ECS thing. I think I understand it a bit more now.
 * https://learn.unity.com/tutorial/entity-component-system#5c7f8528edbc2a002053b67b
 * 
 * 
 * Sooo, after getting a bit into the tutorial. I found out it was very outdated. 
 * After some digging I found another one which saved my ass.
 * https://levelup.gitconnected.com/a-simple-guide-to-get-started-with-unity-ecs-b0e6a036e707
 */

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public static EntityManager EM;

    [Header("Simulation Setting")]
    public float topBound = 16.5f;
    public float bottomBound = -13.5f;
    public float rightBound = 23.5f;
    public float leftBound = -23.5f;

    [Header("Enemy Settings")]
    public GameObject enemyShipPrefab;
    public float enemySpeed = 10f;

    private Entity enemyShipEntityPrefab;


    [Header("Spawn Settings")]
    public int enemyShipCount = 500;
    public int enemyShipIncrement = 500;
    public GameObject parent;

    NativeArray<Entity> ships;

    TransformAccessArray transforms;
    MovementJob moveJob;
    JobHandle moveHandle;

    private int count;

    private void OnDisable()
    {
        moveHandle.Complete();
        transforms.Dispose();
    }



    void Awake()
    {
        if (GM == null)
            GM = this;
        else if (GM != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        //Used to be World.Active.EntityManager
        //But that doesn't work anymore.

        EM = World.DefaultGameObjectInjectionWorld.EntityManager;
        //enemyShipEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyShipPrefab, GameObjectConversionSettings.FromWorld(w, null));

        transforms = new TransformAccessArray(0, -1);
        AddShips(enemyShipCount);
    }

    void Update()
    {
        moveHandle.Complete();

        moveJob = new MovementJob()
        {
            moveSpeed = enemySpeed,
            topBound = topBound,
            bottomBound = bottomBound,
            deltaTime = Time.deltaTime
        };

        moveHandle = moveJob.Schedule(transforms);

        //Do the jobs!
        JobHandle.ScheduleBatchedJobs();
    }


    private void AddShips(int amount)
    {
        moveHandle.Complete();

        //ships = new NativeArray<Entity>(amount, Allocator.TempJob);
        //EM.Instantiate(enemyShipEntityPrefab, ships);

        transforms.capacity = transforms.length + amount;

        for (int i = 0; i < amount; i++)
        {
            float xVal = Random.Range(leftBound, rightBound);
            float yVal = Random.Range(0f, 10f);

            Vector3 pos = new Vector3(xVal, 0f, yVal + topBound);
            Quaternion rot = Quaternion.Euler(0f, 180f, 0f);

            //EM.SetComponentData(ships[i], new Translation { Value = pos });
            //EM.SetComponentData(ships[i], new Rotation { Value = rot });

            var obj = Instantiate(enemyShipPrefab, pos, rot) as GameObject;
            if (parent != null)
                obj.transform.SetParent(parent.transform);

            transforms.Add(obj.transform);
        }

        count += amount;
        //ships.Dispose();
    }

    //Just to keep track of where the bounds are
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(leftBound, 0f, bottomBound), new Vector3(rightBound, 0f, bottomBound));

        Gizmos.DrawLine(new Vector3(leftBound, 0f, topBound), new Vector3(rightBound, 0f, topBound));
    }
}
