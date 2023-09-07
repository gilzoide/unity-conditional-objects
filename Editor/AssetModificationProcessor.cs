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

        static string[] OnWillSaveAssets(string[] paths)
        {
            EmbeddedPresetHolder.Instance.HandleAssetSave(paths);
            return paths;
        }
    }
}
