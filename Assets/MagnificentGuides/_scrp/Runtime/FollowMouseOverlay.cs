using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseOverlay : MonoBehaviour {
  void Update () {
        transform.position = Input.mousePosition;
    }
}