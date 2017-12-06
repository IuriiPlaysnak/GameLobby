using UnityEngine;

namespace PlaysnakRealms
{
	[RequireComponent (typeof(RealmsPrevNextHandButtonUI))]

	public class RealmsPrevNextHandButton : RealmsHandButton
	{
		enum Direction : byte {
			PREV,
			NEXT
		}

		[SerializeField]
		private Direction _direction;

		[SerializeField]
		private RealmGallery _gallery;

		protected override void Init ()
		{
			base.Init ();
			_ui.SetState (_direction == Direction.PREV ? RealmsHandButtonUI.State.PREV : RealmsHandButtonUI.State.NEXT);
		}

		protected override void OnInteractionClick ()
		{
			base.OnInteractionClick ();

			switch (_direction) {

			case Direction.PREV:
				_gallery.PrevImage ();
				break;

			case Direction.NEXT:
				_gallery.NextImage ();
				break;
			}
		}
	}
}

