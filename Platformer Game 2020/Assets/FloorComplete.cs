using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorComplete : MonoBehaviour
{
    public Enemy end;
    public static bool win = false;

    public void FixedUpdate() {
        if (end.isDead) {
            win = true;
        }
    }
}
