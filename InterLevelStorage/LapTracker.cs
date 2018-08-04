using CommonStructures;
using UnityEngine;

namespace Derby {
    
    // TODO: Use a triple to keep track of checkpoint, lap, and time.
    /// <summary>
    /// Stores every vehicle's progress and their laps. For a two-tuple, the LHS is the 
    /// checkpoint and the RHS is the lap number.
    /// </summary>
    [CreateAssetMenu(fileName = "Lap Tracker", menuName = "Derby/Lap Tracker")]
    public class LapTracker : ScriptableObject, ITupleArrayWrapper<int, int> {

        /// <summary>
        /// Returns a read only array of the progress done by each player.
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyCollection<Tuple<int, int>> Progress {
            get { 
                return System.Array.AsReadOnly<Tuple<int, int>>(progress); 
            }
        }

        /// <summary>
        /// Has the lap been completed? If so lock it until we complete the next lap.
        /// </summary>
        public bool IsLapLocked {
            get; set;
        }

        /// <summary>
        /// How many checkpoints are there?
        /// </summary>
        public int CheckpointLength {
            get; private set;
        } 

        /// <summary>
        /// How many players are we tracking?
        /// </summary>
        public int Length {
            get { 
                return progress.Length;
            }
        }
        
        /// <summary>
        /// Operator overload to read / write to a specific index in the array.
        /// </summary>
        /// <param name="key">Which index are we writing to?</param>
        public Tuple<int, int> this[int key] {
            get { 
                return progress[key];
            }
            set {
                progress[key] = value;
            }
        }
        
        public Tuple<int, int>[] progress;

        /// <summary>
        /// Use this like a constructor and initialize every tuple to a default value.
        /// </summary>
        /// <param name="playerSize">How many players are we keeping track of?</param>
        /// <param name="checkpointSize">How many checkpoints are we keeping track of?</param>
        public void Initialize(int playerSize, int checkpointSize) {
            IsLapLocked = false;
            CheckpointLength = checkpointSize;
            progress = new Tuple<int, int>[playerSize];

            for (int i = 0; i < playerSize; i++) {
                progress[i] = Tuple<int, int>.CreateTuple(-1, -1);
            }
        }
    }
}
