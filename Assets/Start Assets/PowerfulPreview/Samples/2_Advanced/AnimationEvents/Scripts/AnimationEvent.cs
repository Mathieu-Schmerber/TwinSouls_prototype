using UnityEngine;

namespace StartAssets.AnimationEvents
{
    /// <summary>
    /// It's a basic class for any animation event. 
    /// </summary>
    public abstract class AnimationEvent : ScriptableObject
    {
        /// <summary>
        /// This method is called when current animation time is at the moment of the event is set to. 
        /// </summary>
        /// <param name="parent">Game object which is playing the animation.</param>
        /// <param name="coroutineMaster">A script that is responsible for running coroutines of this specific animation event.</param>
        public abstract void Invoke( GameObject parent, MonoBehaviour coroutineMaster );
    }
}