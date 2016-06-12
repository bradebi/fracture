using System;
using UnityEngine;

namespace UTK.Physics
{
    public class PhysConstants
    {
        public const float EarthsGravity = 9.81f;
    }
    public class Kinematics
    {
        /// <summary>
        /// This equation will extrapolate the next position of an object in 3D space
        /// given an initial position, initial acceleration, initial velocity, and a time delta
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="v0"></param>
        /// <param name="a0"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector3 ExtrapolatePosition(Vector3 p0, Vector3 v0, Vector3 a0, float deltaTime)
        {
            return p0 + v0 * deltaTime + 0.5f * a0 * deltaTime * deltaTime;
        }

        /// <summary>
        /// Will modify and update a phys object with an extrapolated position based on a delta time
        /// </summary>
        /// <param name="physObject"></param>
        /// <param name="deltaTime"></param>
        public static IKinematic ExtrapolatePosition(IKinematic physObject, float deltaTime)
        {
            Vector3 newPos = physObject.Position + physObject.Velocity * deltaTime + 0.5f * physObject.Acceleration * deltaTime * deltaTime;

            return new KinematicState(newPos, physObject.Velocity, physObject.Acceleration);
        }
    }

    public interface IKinematic
    {
        Vector3 Velocity { get; set; }
        Vector3 Position { get; set; }
        Vector3 Acceleration { get; set; }
    }

    public class KinematicState : IKinematic
    {
        Vector3 _acceleration = Vector3.zero;
        public Vector3 Acceleration
        {
            get
            {
                return _acceleration;
            }

            set
            {
                _acceleration = value;
            }
        }

        Vector3 _position = Vector3.zero;
        public Vector3 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity
        {
            get
            {
                return _velocity;
            }

            set
            {
                _velocity = value;
            }
        }
        
        public KinematicState()
        {

        }

        public KinematicState(Vector3 position, Vector3 velocity, Vector3 acceleration)
        {
            _position = position;
            _velocity = velocity;
            _acceleration = acceleration;
        }
    }
}