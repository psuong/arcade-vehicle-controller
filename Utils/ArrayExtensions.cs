namespace Derby {

    public static class ArrayExtensions {
        
        /// <summary>
        /// Is each integer element equivalent in the array?
        /// </summary>
        /// <returns>True, if the the elements are all equal</returns>
        public static bool AreElementsEquivalent(this int[] elements) {
            var first = elements[0];
            for (int i = 0; i < elements.Length; i++) {
                if (elements[i] != first) {
                    return false;
                }
            }
            return true;
        }
    }
}
