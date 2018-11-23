using System;

namespace Xamarin.Forms
{
	public class SwipedEventArgs : EventArgs
	{
		public SwipedEventArgs(object parameter, SwipeDirection direction, SwipeDirection detectedDirection)
		{
			Parameter = parameter;
			Direction = direction;
			DetectedDirection = detectedDirection;
		}

		public object Parameter { get; }

		public SwipeDirection Direction { get; }

		public SwipeDirection DetectedDirection { get; }
	}
}