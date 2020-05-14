using System.IO;
using MLAgents;

public class SpiderAgent : Agent {
    public SpiderController spiderController;

    public override void AgentReset() {
        spiderController.moveToStart();
    }

    public override float[] Heuristic() {
        var vals = SpiderManualControler.getValues();
        for (int i = 0; i < 12; i++) {
            vals[i] = normalize(vals[i]);
        }

        return vals;
    }

    public override void CollectObservations() {

        var rot = spiderController.center.rotation;
        //var rotNormalized = rot.eulerAngles / 180.0f - Vector3.one;
        
        for (int i = 0; i < 12; i++) {
            AddVectorObs(normalize(spiderController.allServos[i].currentAngle));
        }
    }

    public override void AgentAction(float[] vectorAction) {

        //float[] vectorAction = new float[12];
        //vectorActionInput.CopyTo(vectorAction, 0);
        
        if (vectorAction.Length != 12) throw new InvalidDataException("invalid number of actions");
        
        for (int i = 0; i < vectorAction.Length - 1; i++) {
            if (vectorAction[i] < 1 && vectorAction[i] > -1) {
                vectorAction[i] = denormalize(vectorAction[i]);
            }
        }

        for (int i = 0; i < 12; i++) {
            spiderController.allServos[i].targetAngle = vectorAction[i];
        }

        //reward
        SetReward(spiderController.getReward());
        
        //reset if invalid
        if (spiderController.isTurned()) {
            print("spider turned!");
            Done();
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