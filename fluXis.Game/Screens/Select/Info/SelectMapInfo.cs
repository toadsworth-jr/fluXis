using fluXis.Game.Audio;
using fluXis.Game.Database.Maps;
using fluXis.Game.Graphics.Background;
using fluXis.Game.Map;
using fluXis.Game.Screens.Select.Info.Scores;
using fluXis.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace fluXis.Game.Screens.Select.Info;

public partial class SelectMapInfo : FillFlowContainer
{
    [Resolved]
    private MapStore maps { get; set; }

    private RealmMap map;

    public ScoreList ScoreList;

    private BackgroundStack backgroundStack;
    private SpriteText titleText;
    private SpriteText artistText;
    private SpriteText difficultyText;
    private SpriteText mapperText;
    private SpriteText bpmText;
    private SpriteText lengthText;

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.CentreRight;
        Origin = Anchor.CentreRight;
        RelativeSizeAxes = Axes.Both;
        Width = .5f;
        Padding = new MarginPadding { Vertical = 10 };
        X = 10;
        Direction = FillDirection.Vertical;
        Spacing = new Vector2(0, 10);

        InternalChildren = new Drawable[]
        {
            new Container
            {
                Name = "Metadata",
                CornerRadius = 10,
                RelativeSizeAxes = Axes.Both,
                Height = .25f,
                Masking = true,
                Children = new Drawable[]
                {
                    backgroundStack = new BackgroundStack(maps),
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black,
                        Alpha = .4f
                    },
                    titleText = new SpriteText
                    {
                        Text = "Title",
                        Font = new FontUsage("Quicksand", 60, "Bold"),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.BottomLeft,
                        X = 20,
                        Y = 10
                    },
                    artistText = new SpriteText
                    {
                        Text = "Artist",
                        Font = new FontUsage("Quicksand", 32, "Bold"),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.TopLeft,
                        X = 20
                    }
                }
            },
            new Container
            {
                Name = "Technical Info",
                RelativeSizeAxes = Axes.Both,
                Height = .25f,
                CornerRadius = 10,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.FromHex("#222228")
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(10),
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Children = new Drawable[]
                                {
                                    new FillFlowContainer
                                    {
                                        Direction = FillDirection.Horizontal,
                                        AutoSizeAxes = Axes.Both,
                                        Spacing = new Vector2(4, 0),
                                        Y = -5,
                                        Children = new Drawable[]
                                        {
                                            difficultyText = new SpriteText
                                            {
                                                Text = "diffname",
                                                Font = new FontUsage("Quicksand", 22, "Bold"),
                                            },
                                            new SpriteText
                                            {
                                                Text = "mapped by",
                                                Font = new FontUsage("Quicksand", 22),
                                            },
                                            mapperText = new SpriteText
                                            {
                                                Text = "mapper",
                                                Font = new FontUsage("Quicksand", 22),
                                            },
                                        }
                                    },
                                    new Container
                                    {
                                        AutoSizeAxes = Axes.Both,
                                        CornerRadius = 10,
                                        Masking = true,
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        Margin = new MarginPadding { Right = 10 },
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Colour4.FromHex("#2a2a30")
                                            },
                                            new FillFlowContainer
                                            {
                                                Direction = FillDirection.Horizontal,
                                                AutoSizeAxes = Axes.Both,
                                                Spacing = new Vector2(10, 0),
                                                Padding = new MarginPadding { Bottom = 2, Top = 3, Horizontal = 10 },
                                                Y = -5,
                                                Children = new Drawable[]
                                                {
                                                    bpmText = new SpriteText
                                                    {
                                                        Text = "BPM",
                                                        Font = new FontUsage("Quicksand", 22, "Bold"),
                                                    },
                                                    lengthText = new SpriteText
                                                    {
                                                        Text = "Length",
                                                        Font = new FontUsage("Quicksand", 22, "Bold"),
                                                    },
                                                }
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new Container
            {
                Name = "Scores Wrapper",
                RelativeSizeAxes = Axes.Both,
                Height = .5f,
                Padding = new MarginPadding { Bottom = 20, Right = 20 },
                Child = new Container
                {
                    Name = "Scores",
                    RelativeSizeAxes = Axes.Both,
                    Child = ScoreList = new ScoreList()
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        Conductor.OnBeat += OnBeat;

        base.LoadComplete();
    }

    public void ChangeMap(RealmMap map)
    {
        this.map = map;

        backgroundStack.ChangeMap(map);
        ScoreList.SetMap(map);
        titleText.Text = map.Metadata.Title;
        artistText.Text = map.Metadata.Artist;
        difficultyText.Text = map.Difficulty;
        mapperText.Text = map.Metadata.Mapper;
        lengthText.Text = "Length " + TimeUtils.Format(map.Length, false);
        bpmText.Text = map.BPMMin == map.BPMMax ? $"BPM {map.BPMMin}" : $"BPM {map.BPMMin}-{map.BPMMax}";
    }

    private void OnBeat(int beat)
    {
        bpmText.FadeIn().FadeTo(.6f, Conductor.BeatTime);
    }

    protected override void Dispose(bool isDisposing)
    {
        Conductor.OnBeat -= OnBeat;
        base.Dispose(isDisposing);
    }

    private partial class BackgroundStack : Container
    {
        private readonly MapStore mapStore;

        public BackgroundStack(MapStore mapStore)
        {
            this.mapStore = mapStore;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void ChangeMap(RealmMap map)
        {
            var background = new MapBackground(map)
            {
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fill,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Add(background);
            background.FadeInFromZero(300);
        }

        protected override void Update()
        {
            base.Update();

            while (Children.Count > 1)
            {
                if (Children[1].Alpha == 1)
                    Remove(Children[0], false);
                else
                    break;
            }
        }
    }
}
