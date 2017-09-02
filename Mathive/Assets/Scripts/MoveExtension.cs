using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveExtension {

  public static IEnumerator Scale(this Transform t, Vector3 target, float duration)
  {
    Vector3 diffVector = (target - t.localScale);
    float diffLength = diffVector.magnitude;
    diffVector.Normalize();
    float counter = 0;
    while (counter < duration)
    {
      float newAmount = (Time.deltaTime * diffLength) / duration;
        t.localScale += diffVector * newAmount;
        counter += Time.deltaTime;
        yield return null;
    }
    t.localScale = target;
  }

  public static IEnumerator Scale(this Transform t, Vector3 target, float duration, bool active)
  {
    if (active) t.gameObject.SetActive(active);
    Vector3 diffVector = (target - t.localScale);
    float diffLength = diffVector.magnitude;
    diffVector.Normalize();
    float counter = 0;
    while (counter < duration)
    {
      float newAmount = (Time.deltaTime * diffLength) / duration;
      t.localScale += diffVector * newAmount;
      counter += Time.deltaTime;
      yield return null;
    }
    t.localScale = target;
    if(!active) t.gameObject.SetActive(active);
  }

  public static IEnumerator Scale(this Transform t, Vector3 target, float duration, Hive hive)
  {
    Vector3 diffVector = (target - t.localScale);
    float diffLength = diffVector.magnitude;
    diffVector.Normalize();
    float counter = 0;
    while (counter < duration)
    {
      float newAmount = (Time.deltaTime * diffLength) / duration;
      t.localScale += diffVector * newAmount;
      counter += Time.deltaTime;
      yield return null;
    }
    t.localScale = target;
    hive.DestroyMyself();
  }
}
