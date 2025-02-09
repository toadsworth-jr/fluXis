using fluXis.Graphics.Sprites;
using fluXis.Skinning;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace fluXis.Screens.Gameplay.HUD.Components;

public partial class HealthBar : GameplayHUDComponent
{
    [Resolved]
    private SkinManager skinManager { get; set; }

    private Drawable bar;
    private SpriteIcon icon;

    private bool showingIcon;

    [BackgroundDependencyLoader]
    private void load()
    {
        AutoSizeAxes = Axes.Both;

        InternalChildren = new[]
        {
            skinManager.GetHealthBarBackground(),
            bar = skinManager.GetHealthBar(HealthProcessor),
            icon = new FluXisSpriteIcon
            {
                Size = new Vector2(30),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome6.Solid.XMark,
                Alpha = 0
            }
        };
    }

    protected override void Update()
    {
        if (!showingIcon && HealthProcessor.FailedAlready)
        {
            showingIcon = true;
            icon.FadeIn(600).Then(400).FadeOut(600).Loop();
        }

        bar.Height = HealthProcessor.SmoothHealth / 100;

        base.Update();
    }
}
