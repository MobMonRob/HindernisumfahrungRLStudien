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
        Destroy(m_Target.gameObject);
        SpawnTarget(TargetPrefab, transform.position);
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

        AddReward(spiderController.getReward(m_Target.position));

        if (spiderController.isFlipped()) {
            print("spider turned!");
            EndEpisode();
        }
    }
    
    //convert angles from degrees to normalized value in range [-1, 1]
    private float denormalize(float val) {
        return val * 360f;
    }

    private float normalize(float val) {
        return val / 360f;
    }
}