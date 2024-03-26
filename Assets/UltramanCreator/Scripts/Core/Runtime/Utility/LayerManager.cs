using UnityEngine;

namespace TestGon
{
    public static class LayerManager
    {
        #region LayerMask
        public static readonly int BODY_PART_LAYER_MASK = LayerMask.GetMask("BodyPart");
        
        #endregion

        #region Layer
        public static readonly int IGNORE_RAYCAST_LAYER = LayerMask.NameToLayer("Ignore Raycast");
        public static readonly int BODY_PART_LAYER = LayerMask.NameToLayer("BodyPart");
        public static readonly int BODY_LAYER = LayerMask.NameToLayer("Body");
        
        #endregion

        #region LayerName
        public static readonly string IGNORE_RAYCAST_LAYER_NAME = "Ignore Raycast";
        public static readonly string BODY_PART_LAYER_NAME = "BodyPart";
        public static readonly string BODY_LAYER_NAME = "Body";
        public static readonly string TOOLS_LAYER_NAME = "Tools";

        #endregion
    }
}