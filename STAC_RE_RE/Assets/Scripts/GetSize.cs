using System;
using UnityEngine;
using System.Collections;

public class GetSize : MonoBehaviour
{
 public static GetSize instance;

 private void Awake()
 {
  if (instance == null)
   instance = this;
 }

 public Vector3 GetSpriteSize(GameObject _target)
 {
  Vector3 worldSize = Vector3.zero;
  if(_target.GetComponent<SpriteRenderer>())
  {
   Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
   Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
   worldSize = localSpriteSize;
   worldSize.x *= _target.transform.lossyScale.x;
   worldSize.y *= _target.transform.lossyScale.y;
  }
  else
  {
   Debug.Log ("SpriteRenderer Null");
  }
  
  
  return worldSize;
 }
}

