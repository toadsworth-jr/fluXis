using System.Linq;
using fluXis.Game.Configuration;
using fluXis.Game.Graphics.UserInterface.Color;
using fluXis.Game.Graphics.UserInterface.Text;
using fluXis.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Platform;

namespace fluXis.Game.Overlay.FPS;

public partial class FpsOverlay : Container
{
    [Resolved]
    private GameHost host { get; set; }

    [Resolved]
    private FluXisConfig config { get; set; }

    private double lastTime;
    private FluXisTextFlow textFlow;
    private Bindable<bool> showFps;
    private bool visible;

    [BackgroundDependencyLoader]
    private void load()
    {
        AutoSizeAxes = Axes.Both;
        Anchor = Anchor.BottomRight;
        Origin = Anchor.BottomRight;
        Margin = new MarginPadding(16);
        CornerRadius = 4;
        Masking = true;
        AlwaysPresent = true;
        Alpha = 0;

        InternalChildren = new Drawable[]
        {
            new Box
            {
                Colour = FluXisColors.Background4,
                RelativeSizeAxes = Axes.Both
            },
            new Container
            {
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding(6),
                Child = textFlow = new FluXisTextFlow
                {
                    AutoSizeAxes = Axes.Both,
                    TextAnchor = Anchor.TopRight,
                    WebFontSize = 10
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        showFps = config.GetBindable<bool>(FluXisSetting.ShowFps);
    }

    protected override void Update()
    {
        base.Update();

        if (showFps.Value != visible)
        {
            visible = showFps.Value;

            if (visible)
                Show();
            else
                Hide();
        }

        if (Time.Current - lastTime < 500) return;

        lastTime = Time.Current;

        textFlow.Clear();

        foreach (var thread in host.Threads)
        {
            var clock = thread.Clock;
            var firstChar = thread.Name.First();

            if (firstChar is 'U' or 'D') // Update or Draw
                textFlow.AddParagraph($"{clock.FramesPerSecond} {firstChar}PS ({clock.ElapsedFrameTime:0}ms)");
        }
    }

    protected override bool OnHover(HoverEvent e)
    {
        this.FadeIn(FluXisScreen.FADE_DURATION);
        return false;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        this.Delay(1200).FadeTo(.6f, FluXisScreen.FADE_DURATION);
    }

    public override void Show()
    {
        this.FadeTo(.6f, FluXisScreen.FADE_DURATION)
            .MoveToX(0, FluXisScreen.MOVE_DURATION, Easing.OutQuint);
    }

    public override void Hide()
    {
        this.FadeOut(FluXisScreen.FADE_DURATION)
            .MoveToX(40, FluXisScreen.MOVE_DURATION, Easing.OutQuint);
    }
}
