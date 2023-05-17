using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentPatcher.Framework.Conditions;
using ContentPatcher.Framework.ConfigModels;
using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;

namespace ContentPatcher.Framework.Migrations
{
    /// <summary>Migrates patches to format version 1.8.</summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Named for clarity.")]
    internal class Migration_1_8 : BaseMigration
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public Migration_1_8()
            : base(new SemanticVersion(1, 8, 0))
        {
            this.AddedTokens = new InvariantSet(
                nameof(ConditionType.IsOutdoors),
                nameof(ConditionType.LocationName),
                nameof(ConditionType.Target),
                nameof(ConditionType.TargetWithoutPath)
            );
        }

        /// <inheritdoc />
        public override bool TryMigrate(ref PatchConfig[] patches, [NotNullWhen(false)] out string? error)
        {
            if (!base.TryMigrate(ref patches, out error))
                return false;

            foreach (PatchConfig patch in patches)
            {
                // 1.8 adds EditMap
                if (this.HasAction(patch, PatchType.EditMap))
                {
                    error = this.GetNounPhraseError($"using action {nameof(PatchType.EditMap)}");
                    return false;
                }

                // 1.8 adds MoveEntries
                if (patch.MoveEntries.Any())
                {
                    error = this.GetNounPhraseError($"using {nameof(PatchConfig.MoveEntries)}");
                    return false;
                }
            }

            return true;
        }
    }
}
