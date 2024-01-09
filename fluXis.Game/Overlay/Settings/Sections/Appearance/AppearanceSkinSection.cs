using fluXis.Game.Configuration;
using fluXis.Game.Graphics.UserInterface.Panel;
using fluXis.Game.Overlay.Settings.UI;
using fluXis.Game.Skinning;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace fluXis.Game.Overlay.Settings.Sections.Appearance;

public partial class AppearanceSkinSection : SettingsSubSection
{
    public override string Title => "Skin";
    public override IconUsage Icon => FontAwesome.Solid.PaintBrush;

    [Resolved]
    private SkinManager skinManager { get; set; }

    private SettingsDropdown<string> currentDropdown;

    [BackgroundDependencyLoader]
    private void load(SkinManager skinManager, FluXisGameBase gameBase)
    {
        AddRange(new Drawable[]
        {
            currentDropdown = new SettingsDropdown<string>
            {
                Label = "Current Skin",
                Bindable = Config.GetBindable<string>(FluXisSetting.SkinName),
                Items = skinManager.GetSkinNames()
            },
            new SettingsButton
            {
                Label = "Open Skin editor",
                ButtonText = "Open",
                Action = gameBase.OpenSkinEditor
            },
            new SettingsButton
            {
                Label = "Open Skin folder",
                Action = skinManager.OpenFolder,
                ButtonText = "Open"
            },
            new SettingsButton
            {
                Label = "Export Skin",
                Description = "Export the current skin as a .fsk file.",
                ButtonText = "Export",
                Action = skinManager.ExportCurrent
            },
            new SettingsButton
            {
                Label = "Delete Skin",
                Description = "Delete the current skin.",
                ButtonText = "Delete",
                Action = () =>
                {
                    if (skinManager.IsDefault)
                        return;

                    gameBase.Overlay = new ConfirmDeletionPanel(() =>
                    {
                        skinManager.Delete(skinManager.SkinFolder);
                    }, itemName: "skin");
                }
            }
        });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        skinManager.SkinListChanged += () => currentDropdown.Items = skinManager.GetSkinNames();
    }
}
