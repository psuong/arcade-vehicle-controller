using Unity.Entities;

namespace Derby {

    /// <summary>
    /// A struct which holds the horizontal data from the controller.
    /// </summary>
    public struct SelectionBrowseData : IComponentData {
        public int horizontal;
    }

    /// <summary>
    /// Stores batches of horizontal data from every controller.
    /// </summary>
    public struct SelectionBrowseComponent {
        public readonly int Length;
        public ComponentDataArray<SelectionBrowseData> input;
    }

    /// <summary>
    /// A struct which holds confirmation and cancel actions from a controller.
    /// </summary>
    public struct SelectionConfirmationData : IComponentData {
        public int accept, cancel;
    }

    /// <summary>
    /// Stores batches confirmation / cancellation data from every controller.
    /// </summary>
    public struct SelectionConfirmationComponent {
        public readonly int Length;
        public ComponentDataArray<SelectionConfirmationData> input;
    }
}
