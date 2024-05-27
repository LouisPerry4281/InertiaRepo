using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BERTHAAtt : MonoBehaviour
{
    public float cameraShakeMagnitude;
    public float cameraShakeLength;

    public Transform floorTarget1;
    public Transform floorTarget2;

    public GameObject rubble;

    bool isSecondHit;

    public void HitGround()
    {
        CinemachineShake.Instance.ShakeCamera(cameraShakeMagnitude, cameraShakeLength);
        AudioManager.instance.PlaySFX("RockSmash", 1, 1);

        if (isSecondHit)
        {
            GameObject rubbleInstance = Instantiate(rubble, floorTarget2.position, Quaternion.identity);
            rubbleInstance.transform.eulerAngles = new Vector3(-90, 0, 0);
            isSecondHit = false;
        }

        else
        {
            GameObject rubbleInstance = Instantiate(rubble, floorTarget1.position, Quaternion.identity);
            rubbleInstance.transform.eulerAngles = new Vector3(-90, 0, 0);
            isSecondHit = true;
        }
    }
}
