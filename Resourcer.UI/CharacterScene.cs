using System.Drawing;

using OpenGameKit.Graphics;

namespace Resourcer.UI
{
    /// <summary>
    /// Draws any characters that should be visible on the map.
    /// </summary>
    internal class CharacterScene : Scene
    {
        /// <summary>
        /// The main player character.
        /// </summary>
        private readonly Sprite _character;

        /// <summary>
        /// The current offset.
        /// </summary>
        private Point _offset;

        /// <summary>
        /// The offset of the viewport.
        /// </summary>
        public Point Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                RequestLayout();
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="CharacterScene"/>.
        /// </summary>
        /// <param name="sprites">The provider for the textures.</param>
        public CharacterScene( SpriteProvider sprites )
        {
            _character = new Sprite( sprites.CharacterTile );

            Children.Add( _character );
        }

        /// <inheritdoc/>
        protected override void Layout()
        {
            // Determine the X,Y map coordinates we will start painting from.
            var mapLeft = ( int ) Math.Floor( Offset.X / 64.0 );
            var mapTop = ( int ) Math.Floor( Offset.Y / 64.0 );

            // Determine the drawing offsets.
            var left = ( mapLeft * 64 ) - Offset.X;
            var top = ( mapTop * 64 ) - Offset.Y;

            var characterPosX = 12;
            var characterPosY = 12;

            var x = left + ( ( characterPosX - mapLeft ) * 64 );
            var y = top + ( ( characterPosY - mapTop ) * 64 );

            _character.Frame = new Rectangle( x + 8, y + 8, 48, 48 );
            System.Diagnostics.Debug.WriteLine( $"Character = {_character.Frame}" );
        }
    }
}
