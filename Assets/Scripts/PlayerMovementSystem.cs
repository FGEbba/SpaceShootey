using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;



public class PlayerMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Rigidbody, InputComponent>().ForEach( (Entity entity, InputComponent input, Rigidbody rb) =>
      {
          
          var moveVector = new Vector3(input.Horizontal, 0, input.Vertical);
          var movePosition = rb.position + moveVector.normalized * 3 * Time.DeltaTime;

          rb.MovePosition(movePosition);
      });

    }
}
