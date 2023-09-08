using UnityEditor;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            EmbeddedPresetHolder.Instance.HandleAssetDelete(assetPath);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
