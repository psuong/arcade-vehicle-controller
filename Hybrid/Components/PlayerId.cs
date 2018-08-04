using Unity.Entities;

namespace Derby {
    
    /// <summary>
    /// Stores an integer ID and allows GameObjects in Unity to add this Component.
    /// </summary>
    public class PlayerId : ComponentDataWrapper<IntId> { }
}
