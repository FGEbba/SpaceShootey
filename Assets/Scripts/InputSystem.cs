using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity;

public class InputSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        Entities.WithAll<InputComponent>().ForEach((Entity entity, InputComponent input) =>
        {
            input.Horizontal = hor;
            input.Vertical = ver;

        });
        //throw new System.NotImplementedException();
    }
}
