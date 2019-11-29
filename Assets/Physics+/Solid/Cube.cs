using UnityEngine;

public class Cube : MonoBehaviour
{

    public Vector3 size = Vector3.one; // 物体の大きさ(m)
    public Vector3 tensileLoad = Vector3.zero; // 引張荷重(N)
    float crossSectionArea = 1f; // 断面積(m²)
    Vector3 strain; // 歪
    public float poissonsRatio = 0.2f; // ポアソン比
    public float youngsModulus = 0.1f; // ヤング率(GPa)
    public float ultimateTensileStrength = 450f; // 引張強度(MPa)
    public bool debug = false;

    Vector3 impulse = Vector3.zero;

    void FixedUpdate()
    {
        var stress = (tensileLoad + impulse * Time.deltaTime) / crossSectionArea / 1000;
        impulse = Vector3.zero;

        // 応力を与える
        strain = stress / youngsModulus;
        var b = new Vector3(strain.y + strain.z, strain.x + strain.z, strain.x + strain.y);
        transform.localScale = Vector3.one + strain - b * poissonsRatio;

        // TODO 永久ひずみ

        // 破断
        if (debug)
            print(stress.sqrMagnitude * 1000);
        if (stress.sqrMagnitude * 1000 > ultimateTensileStrength)
            print("破断しました");
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        impulse -= Quaternion.Inverse(transform.rotation) * collisionInfo.impulse;
    }
}
