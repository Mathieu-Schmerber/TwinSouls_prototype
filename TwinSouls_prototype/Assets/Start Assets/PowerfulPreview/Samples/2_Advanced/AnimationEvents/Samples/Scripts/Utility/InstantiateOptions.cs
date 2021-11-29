using StartAssets.PowerfulPreview;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstantiateOptions 
{
    public bool linkToBodyPart;
    public HumanBodyBones bodyPart;

    public Vector3 positionOffset = Vector3.zero;
    public Vector3 anglesOffset = Vector3.zero;
    public Vector3 scale = Vector3.one;
}
