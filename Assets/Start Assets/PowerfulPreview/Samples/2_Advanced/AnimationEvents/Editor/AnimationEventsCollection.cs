using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StartAssets.AnimationEvents
{
    /// <summary>
    /// An utility class which reads all the existing animation event classes 
    /// and prepares a list of their types. 
    /// </summary>
    public class AnimationEventsCollection 
    {
        /// <summary>
        /// A way to access it globally 
        /// </summary>
        public static AnimationEventsCollection Instance
        {
            get
            {
                if( mInstance == null )
                {
                    mInstance = new AnimationEventsCollection();
                }
                return mInstance;
            }
        }
        private static AnimationEventsCollection mInstance;

        /// <summary>
        /// Returns an array of the names of the animation event classes. 
        /// </summary>
        public string[] EventNames
        {
            get
            {
                return mEventNames.ToArray();
            }
        }

        /// <summary>
        /// Creates an instance of the animation event class by it's name. 
        /// </summary>
        /// <param name="eventName">Name of the animation event class.</param>
        /// <returns>An instance of the animation event class.</returns>
        public AnimationEvent Create( string eventName )
        {
            if( !mNameToType.TryGetValue( eventName, out var selectedType ))
            {
                return null;
            }

            var eventInstance = ScriptableObject.CreateInstance(selectedType);
            var convertedEvent = Convert.ChangeType(eventInstance, selectedType);

            var animationEvent = convertedEvent as AnimationEvent;
            if( animationEvent != null )
            {
                animationEvent.name = eventName;
            }
            return animationEvent; 
        }
        /// <summary>
        /// Returns an index of the animation event class by it's type. 
        /// </summary>
        /// <param name="type">Type of the animation event class.</param>
        /// <returns>Index of the animation event class.</returns>
        public int GetIndex( Type type )
        {
            if( mTypeToIndex.TryGetValue( type, out var index ))
            {
                return index;
            }
            return -1;
        }

        /// <summary>
        /// Reads all the existing animation event classes. 
        /// </summary>
        public void Update()
        {
            mEventTypes = new List<Type>() { null };
            mEventNames = new List<string>() { "None" };
            mNameToType = new Dictionary<string, Type>();
            mTypeToIndex = new Dictionary<Type, int>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(AnimationEvent)) && type.IsClass && !type.IsAbstract)
                    {
                        mEventTypes.Add(type);
                        mEventNames.Add(type.Name);
                        mNameToType[type.Name] = type;
                        mTypeToIndex[type] = mEventTypes.Count - 1;
                    }
                }
            }
        }

        private AnimationEventsCollection()
        {
            Update();
        }

        private List<string> mEventNames;
        private List<Type> mEventTypes;
        private Dictionary<string, Type> mNameToType;
        private Dictionary<Type, int> mTypeToIndex; 
    }
}