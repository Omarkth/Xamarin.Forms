using System;
using System.Windows.Input;

namespace Xamarin.Forms
{
	public sealed class SwipeGestureRecognizer : GestureRecognizer, ISwipeGestureController
	{
		// Default threshold in pixels before a swipe is detected.
		const uint DefaultSwipeThreshold = 100;

		double _totalX, _totalY;

		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SwipeGestureRecognizer), null);

		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(SwipeGestureRecognizer), null);

		public static readonly BindableProperty DirectionProperty = BindableProperty.Create("Direction", typeof(SwipeDirection), typeof(SwipeGestureRecognizer), default(SwipeDirection));

		public static readonly BindableProperty ThresholdProperty = BindableProperty.Create("Threshold", typeof(uint), typeof(SwipeGestureRecognizer), DefaultSwipeThreshold);

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public SwipeDirection Direction
		{
			get { return (SwipeDirection)GetValue(DirectionProperty); }
			set { SetValue(DirectionProperty, value); }
		}

		public uint Threshold
		{
			get { return (uint)GetValue(ThresholdProperty); }
			set { SetValue(ThresholdProperty, value); }
		}

		public event EventHandler<SwipedEventArgs> Swiped;

		void ISwipeGestureController.SendSwipe(Element sender, double totalX, double totalY)
		{
			_totalX = totalX;
			_totalY = totalY;
		}

		bool ISwipeGestureController.DetectSwipe(View sender, SwipeDirection direction)
		{
			SwipeDirection? detected = null;
			var threshold = Threshold;

			if (direction.IsLeft() && _totalX < -threshold)
			{
				detected = SwipeDirection.Left;
			}

			if (direction.IsRight() && _totalX > threshold)
			{
				detected = SwipeDirection.Right;
			}

			if (direction.IsDown() && _totalY > threshold)
			{
				detected = SwipeDirection.Down;
			}

			if (direction.IsUp() && _totalY < -threshold)
			{
				detected = SwipeDirection.Up;
			}

			var wasDetected = detected.HasValue;
			if (wasDetected)
			{
				SendSwiped(sender, direction, detected.Value);
			}

			return wasDetected;
		}

		public void SendSwiped(View sender, SwipeDirection direction, SwipeDirection detectedDirection)
		{
			ICommand cmd = Command;
			if (cmd != null && cmd.CanExecute(CommandParameter))
				cmd.Execute(CommandParameter);

			Swiped?.Invoke(sender, new SwipedEventArgs(CommandParameter, direction, detectedDirection));
		}
	}

	static class SwipeDirectionExtensions
	{
		public static bool IsLeft(this SwipeDirection self)
		{
			return (self & SwipeDirection.Left) == SwipeDirection.Left;
		}
		public static bool IsRight(this SwipeDirection self)
		{
			return (self & SwipeDirection.Right) == SwipeDirection.Right;
		}
		public static bool IsUp(this SwipeDirection self)
		{
			return (self & SwipeDirection.Up) == SwipeDirection.Up;
		}
		public static bool IsDown(this SwipeDirection self)
		{
			return (self & SwipeDirection.Down) == SwipeDirection.Down;
		}
	}
}