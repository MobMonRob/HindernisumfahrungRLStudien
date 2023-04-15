using UnityEngine;
using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SpiderAgent : Agent {
    public SpiderController spiderController;

    [Header("Target To Walk Towards")]
    public Transform TargetPrefab;
    private Transform m_Target;

    public override void Initialize() {
        SpawnTarget(TargetPrefab, transform.position);
    }

    private void SpawnTarget(Transform prefab, Vector3 pos) {
        m_Target = Instantiate(prefab, pos, Quaternion.identity, transform.parent);
    }

    public override void OnEpisodeBegin() {
        spiderController.moveToStart();
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var vals = SpiderManualControler.getValues();
        for (int i = 0; i < 12; i++) {
            continuousActionsOut[i] = normalize(vals[i]);
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Servo angles
        for (int i = 0; i < 12; i++) {
            sensor.AddObservation(normalize(spiderController.allServos[i].currentAngle));
        }

        // target position
        var targetPosition = m_Target.transform.position;
        targetPosition.y = 0;
        sensor.AddObservation(targetPosition);

        // robot position
        var robotPosition = spiderController.getCenterPosition(); // TODO: AVG Position?
        robotPosition.y = 0;
        sensor.AddObservation(robotPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {
        var continuousActionsOut = actionBuffers.ContinuousActions;
        
        // can this even happen anymore? just user error should be possible
        if (continuousActionsOut.Length != 12) throw new InvalidDataException("invalid number of actions");
        
        for (int i = 0; i < continuousActionsOut.Length - 1; i++) {
            if (continuousActionsOut[i] < 1 && continuousActionsOut[i] > -1) {
                continuousActionsOut[i] = denormalize(continuousActionsOut[i]);
            }
        }

        for (int i = 0; i < 12; i++) {
            spiderController.allServos[i].targetAngle = continuousActionsOut[i];
        }
    }

    void FixedUpdate() {
        //reset if invalid
        if (spiderController.isTurned()) {
            AddReward(-100f);
            print("spider turned!");
            EndEpisode();
        }

        // reward = (walked distance) * (walked distance leads to target) * (look at target)
        // maybe later on add *(stability of body)

        // var walkedDistanceReward = spiderController.getCenterProgress();
        var avgSpeedReward = spiderController.getAvgSpeed().magnitude;
        
        var currentMovementDirection = spiderController.getAvgDirection(); // can velocity be used or is calculation with last position necessary ?
        
        var targetPosition = m_Target.position;
        targetPosition.y = 0;
        var robotPosition = spiderController.getAvgPosition();
        robotPosition.y = 0;
        
        var targetMovementDirection = targetPosition - robotPosition;
        var movementInTargetDirectionReward = (180 - Vector3.Angle(currentMovementDirection, targetMovementDirection)) / 180;


        var centerAngle = spiderController.getAngle();
        var calmCenterAngleReward = (90 - centerAngle) / 90;

        AddReward((avgSpeedReward * movementInTargetDirectionReward * calmCenterAngleReward));
        spiderController.updatePositions();
    }

    
    //convert angles from degrees to normalized value in range [-1, 1]
    private float denormalize(float val) {
        return val * 360f;
    }

    private float normalize(float val) {
        return val / 360f;
    }
}