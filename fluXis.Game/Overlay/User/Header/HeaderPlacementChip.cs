using System;
using fluXis.Game.Graphics;
using fluXis.Game.Graphics.Sprites;
using fluXis.Game.Graphics.UserInterface.Color;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace fluXis.Game.Overlay.User.Header;

public partial class HeaderPlacementChip : Container
{
    public Func<Drawable> CreateIcon { get; set; } = () => new Container();
    public int Placement { get; set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        AutoSizeAxes = Axes.X;
        Height = 50;
        CornerRadius = 25;
        Masking = true;
        EdgeEffect = FluXisStyles.ShadowSmall;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = FluXisColors.Background2
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Horizontal,
                Padding = new MarginPadding { Horizontal = 20 },
                Spacing = new Vector2(10),
                Children = new[]
                {
                    CreateIcon(),
                    new FluXisSpriteText
                    {
                        Text = $"#{(Placement == 0 ? "-" : Placement.ToString())}",
                        WebFontSize = 16
                    }
                }
            }
        };
    }
}
