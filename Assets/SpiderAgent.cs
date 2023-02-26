using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SpiderAgent : Agent {
    public SpiderController spiderController;

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

        var rot = spiderController.center.rotation;
        //var rotNormalized = rot.eulerAngles / 180.0f - Vector3.one;
        
        for (int i = 0; i < 12; i++) {
            sensor.AddObservation(normalize(spiderController.allServos[i].currentAngle));
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) {

        //float[] vectorAction = new float[12];
        //vectorActionInput.CopyTo(vectorAction, 0);

        var continuousActionsOut = actionBuffers.ContinuousActions;
        
        if (continuousActionsOut.Length != 12) throw new InvalidDataException("invalid number of actions");
        
        for (int i = 0; i < continuousActionsOut.Length - 1; i++) {
            if (continuousActionsOut[i] < 1 && continuousActionsOut[i] > -1) {
                continuousActionsOut[i] = denormalize(continuousActionsOut[i]);
            }
        }

        for (int i = 0; i < 12; i++) {
            spiderController.allServos[i].targetAngle = continuousActionsOut[i];
        }

        // actionBuffers.ContinuousActions = continuousActionsOut;
        // this does not work because actionBuffers.ContinuousActions is read only
        // as it is read only, it also might not be needed at all because it is not intended by MLAgents

        //reward
        SetReward(spiderController.getReward());
        
        //reset if invalid
        if (spiderController.isTurned()) {
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