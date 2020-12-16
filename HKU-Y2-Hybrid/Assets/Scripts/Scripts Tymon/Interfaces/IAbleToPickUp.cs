using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAbleToPickUp
{
    void PickUp(Transform parent);
    void Drop();
    void Trow();
}
