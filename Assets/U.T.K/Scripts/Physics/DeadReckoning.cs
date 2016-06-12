using UnityEngine;
namespace UTK.Physics.Prediction
{
    public class DeadReckoning
    {
        float previousTimeStamp = 0;
        float normalizedTime = 0;
        float timeSinceLastUpdate = 0;
        float updateRate = 1/30f;

        bool estimateVelocity = false;

        Math.Filtering.MovingAverageFilter updateRateFilter = new Math.Filtering.MovingAverageFilter(10);
        System.Diagnostics.Stopwatch updateTimer = new System.Diagnostics.Stopwatch();

        KinematicState stateOld;
        KinematicState stateNew;

        public float UpdateRate
        {
            get
            {
                return updateRate;
            }
        }

        IKinematic predictedPhysObject;
        public DeadReckoning(IKinematic physObject, bool estimateVelocity = false)
        {
            this.estimateVelocity = estimateVelocity;
            predictedPhysObject = physObject;
            stateOld = new KinematicState(physObject.Position, physObject.Velocity, physObject.Acceleration);
            stateNew = new KinematicState(physObject.Position, physObject.Velocity, physObject.Acceleration);

            updateTimer.Start();
        }

        public void UpdateExtrapolation()
        {
            timeSinceLastUpdate = (float)updateTimer.Elapsed.TotalSeconds;

            normalizedTime = Mathf.Clamp01(timeSinceLastUpdate / (updateRate));

            Vector3 vb = stateOld.Velocity + (stateNew.Velocity - stateOld.Velocity) * normalizedTime;

            Vector3 pt_old = stateOld.Position + vb * timeSinceLastUpdate + (0.5f) * stateNew.Acceleration * timeSinceLastUpdate * timeSinceLastUpdate;

            Vector3 pt_new = stateNew.Position + stateNew.Velocity * timeSinceLastUpdate + (0.5f) * stateNew.Acceleration * timeSinceLastUpdate * timeSinceLastUpdate;

            predictedPhysObject.Position = pt_old + (pt_new - pt_old) * normalizedTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="interpWeighting"></param>
        public void UpdateRealData(Vector3 position, Vector3 velocity, Vector3 acceleration)
        {
            updateTimer.Stop();

            stateOld = new KinematicState(predictedPhysObject.Position, predictedPhysObject.Velocity, predictedPhysObject.Acceleration);

            if (estimateVelocity)
            {
                velocity = (position - stateNew.Position) / (float)updateTimer.Elapsed.TotalSeconds;
            }

            predictedPhysObject.Velocity = velocity;
            predictedPhysObject.Acceleration = acceleration;
            stateNew = new KinematicState(position, velocity, acceleration);

            //Update filtered rate.
            updateRate = updateRateFilter.Insert((float)updateTimer.Elapsed.TotalSeconds);

            updateTimer.Reset();
            updateTimer.Start();

            timeSinceLastUpdate = 0;
        }

        /// <summary>
        /// We use this to predict a current state
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <param name="interpWeighting"></param>
        KinematicState BlendKinematicStates(IKinematic oldState, IKinematic newState, float interpWeighting = 0.5f)
        {
            interpWeighting = Mathf.Clamp01(interpWeighting);

            float interpNew = interpWeighting;
            float interpOld = 1 - interpWeighting;

            //Explanation of percentateToNew:
            //A value of 0.0 will return the exact same state as "oldState",
            //A value of 1.0 will return the exact same state as "newState",
            //A value of 0.5 will return a state with data exactly in the middle of that of "old" and "new".
            //Its value should never be outside of [0, 1].

            Vector3 blendedVelocity = oldState.Velocity + (newState.Velocity - oldState.Velocity) * normalizedTime;

            KinematicState final = new KinematicState();

            final.Position = (interpOld * oldState.Position) + (interpNew * newState.Position);
            final.Velocity = (interpOld * oldState.Velocity) + (interpNew * newState.Velocity);
            final.Acceleration = (interpOld * oldState.Acceleration) + (interpNew * newState.Acceleration);

            return final;
        }
    }
}
