using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour
{
#region Variables

    public static TargetLocation singleton;

    Vector3 V0, V1, V2;
    int z;

#endregion

#region Monobehaviour

    void Start() {
        InitSingleton();
        V0 = this.gameObject.transform.position;
        z = (int)V0.z;
    }

    void InitSingleton() {
        if (singleton != null) {
            Destroy(this.gameObject);
        }
        else {
            singleton = this;
        }
    }

#endregion

#region Methods

    public Vector3 NewLocation() {
        /* V1 = V0 + Vector3.forward * (this.gameObject.transform.position.z);
        V2 = V0 + Vector3.forward * (-this.gameObject.transform.position.z);

        Vector3 temp = V2 - V1;
        temp = V1 + Random.value * temp; */

        int rand = Random.Range(-5, 6);
        while (z == rand || z + 1 == rand || z - 1 == rand) {
            rand = Random.Range(-5, 6);
            Debug.Log("hi");
        }
        z = rand;
        Vector3 temp = new Vector3(V0.x, V0.y, z);

        return temp;
    }

#endregion
}
