using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;
using Avalonia.Styling;

namespace Nulah.Shortcuts.UI.Animations;

public class HeightTranstion : IPageTransition
{
	public Easing SlideInEasing { get; set; } = new CubicEaseInOut();

	public TimeSpan Duration { get; set; }

	public HeightTranstion()
	{
	}

	public HeightTranstion(TimeSpan duration)
	{
		Duration = duration;
	}

	public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		var tasks = new List<Task>();

		if (to is ContentPresenter { Content: not null } toPresenter)
		{
			var fromHeight = (from as ContentPresenter)?.DesiredSize.Height ?? 0;
			var toHeight = toPresenter.DesiredSize.Height;
			to.IsVisible = true;

			var animation = new Animation
			{
				Easing = SlideInEasing,
				Children =
				{
					new KeyFrame
					{
						Setters =
						{
							new Setter { Property = Layoutable.HeightProperty, Value = fromHeight },
						},
						Cue = new Cue(0d)
					},
					new KeyFrame
					{
						Setters =
						{
							new Setter { Property = Layoutable.HeightProperty, Value = toHeight },
						},
						Cue = new Cue(1d)
					}
				},
				Duration = Duration
			};
			tasks.Add(animation.RunAsync(toPresenter, cancellationToken));
		}

		if (from is ContentPresenter { Content: not null } fromPresenter)
		{
			var fromHeight = fromPresenter.DesiredSize.Height;
			var toHeight = (to as ContentPresenter)?.DesiredSize.Height ?? 0;

			var animation = new Animation
			{
				Easing = SlideInEasing,
				Children =
				{
					new KeyFrame
					{
						Setters =
						{
							new Setter { Property = Layoutable.HeightProperty, Value = fromHeight },
						},
						Cue = new Cue(0d)
					},
					new KeyFrame
					{
						Setters =
						{
							new Setter { Property = Layoutable.HeightProperty, Value = toHeight },
						},
						Cue = new Cue(1d)
					}
				},
				Duration = Duration
			};
			tasks.Add(animation.RunAsync(fromPresenter, cancellationToken));
		}

		await Task.WhenAll(tasks);

		if (from != null && !cancellationToken.IsCancellationRequested)
		{
			from.IsVisible = false;
		}
	}
}