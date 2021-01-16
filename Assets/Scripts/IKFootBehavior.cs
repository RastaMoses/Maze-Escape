using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKFootBehavior : MonoBehaviour
{
    [SerializeField] private Transform footTransformR;
    [SerializeField] private Transform footTransformL;
    private Transform[] allFootTransforms;
    
    [SerializeField] private Transform footTargetTransformR;
    [SerializeField] private Transform footTargetTransformL;
    private Transform[] allTargetTransforms;

    [SerializeField] private GameObject footRigR;
    [SerializeField] private GameObject footRigL;
    private TwoBoneIKConstraint[] allFootIKConstraints;

    [SerializeField] Animator animator;
    private float[] allFootWeights;
    [Header("Character Rig Specific")]
    [SerializeField] float maxHitDistance = 0.6f;
    [SerializeField] float addedHeight = 0.4f;
    [SerializeField] float yOffsetFoot = 0.09f;

    private LayerMask groundLayerMask;
    private bool[] allGroundSphereCastHits;
    private LayerMask hitLayer;
    private Vector3[] allHitNormals;
    private float angleAboutX;
    private float angleAboutZ;
    private float yOffset;

    void Start()
    {
        yOffset = yOffsetFoot;
        allFootTransforms = new Transform[2];
        allFootTransforms[0] = footTransformR;
        allFootTransforms[1] = footTransformL;
        
        allTargetTransforms = new Transform[2];
        allTargetTransforms[0] = footTargetTransformR;
        allTargetTransforms[1] = footTargetTransformL;

        allFootIKConstraints = new TwoBoneIKConstraint[2];
        allFootIKConstraints[0] = footRigR.GetComponent<TwoBoneIKConstraint>();
        allFootIKConstraints[1] = footRigL.GetComponent<TwoBoneIKConstraint>();

        groundLayerMask = LayerMask.NameToLayer("Ground");

        allGroundSphereCastHits = new bool[3];

        allHitNormals = new Vector3[2];

        allFootWeights = new float[2];
    }


    void FixedUpdate()
    {
        RotateCharacterFeet();
    }


    private void CheckGroundBelow(out Vector3 hitPoint, out bool gotGroundSpherecast, out Vector3 hitNormal, out LayerMask hitLayer, out float currentHitDistance,
        Transform objectTransform, int checkForLayerMask, float maxHitDistance, float addedHeight)
    {
        
        RaycastHit hit;
        Vector3 startSpherecast = objectTransform.position + new Vector3(0f, addedHeight, 0f);
        if (checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist!");
            //Null Layer
            gotGroundSpherecast = false;
            currentHitDistance = 0f;
            hitLayer = LayerMask.NameToLayer("Player");
            hitNormal = Vector3.up;
            hitPoint = objectTransform.position;
        }
        else
        {
            int layerMask = (1 << checkForLayerMask);
            if (Physics.SphereCast(startSpherecast, 0.2f, Vector3.down, out hit, maxHitDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                hitLayer = hit.transform.gameObject.layer;
                currentHitDistance = hit.distance - addedHeight;
                hitNormal = hit.normal;
                gotGroundSpherecast = true;
                hitPoint = hit.point;
            }
            else
            {
                //Null Layer
                gotGroundSpherecast = false;
                currentHitDistance = 0f;
                hitLayer = LayerMask.NameToLayer("Player");
                hitNormal = Vector3.up;
                hitPoint = objectTransform.position;
            }
        }

        

    }

    Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal);
    }

    private void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform footTargetTransform, Vector3 hitNormal)
    {
        Vector3 xAxisProjected = ProjectOnContactPlane(footTargetTransform.forward, hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnContactPlane(footTargetTransform.right, hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(footTargetTransform.forward, xAxisProjected, footTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(footTargetTransform.right, zAxisProjected, footTargetTransform.forward);
    }

    private void RotateCharacterFeet()
    {
        allFootWeights[0] = animator.GetFloat("R Foot Weight");
        allFootWeights[1] = animator.GetFloat("L Foot Weight");
        for (int i = 0; i < 2; i++)
        {
            allFootIKConstraints[i].weight = allFootWeights[i];

            CheckGroundBelow(out Vector3 hitPoint, out allGroundSphereCastHits[i], out Vector3 hitNormal, out hitLayer, out _,
                allFootTransforms[i], groundLayerMask, maxHitDistance, addedHeight);
            allHitNormals[i] = hitNormal;
            
            if (allGroundSphereCastHits[i] == true)
            {
                yOffset = yOffsetFoot;
                if (allFootTransforms[i].position.y < allTargetTransforms[i].position.y - 0.01f) //if error, change this to smaller!!!!!!!!!!!
                {
                    yOffset += allTargetTransforms[i].position.y - allFootTransforms[i].position.y;
                }
                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, allFootTransforms[i], allHitNormals[i]);

                allTargetTransforms[i].position = new Vector3(allFootTransforms[i].position.x, hitPoint.y + yOffset,allFootTransforms[i].position.z);

                allTargetTransforms[i].rotation = allFootTransforms[i].rotation;

                allTargetTransforms[i].localEulerAngles = new Vector3(allTargetTransforms[i].localEulerAngles.x + angleAboutX,
                    allTargetTransforms[i].localEulerAngles.y, allTargetTransforms[i].localEulerAngles.z + angleAboutZ);
            }
            else
            {
                allTargetTransforms[i].position = allFootTransforms[i].position;
                allTargetTransforms[i].rotation = allFootTransforms[i].rotation;
            }

        }
    }


}
