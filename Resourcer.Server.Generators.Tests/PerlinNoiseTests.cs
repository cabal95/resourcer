using OpenGameKit.Generators;

namespace Resourcer.Server.Generators.Tests
{
    /// <summary>
    /// A set of tests against the PerlinNoise generator to ensure no change
    /// in implementation results in different values being generated.
    /// </summary>
    public class PerlinNoiseTests
    {
        /// <summary>
        /// Ensures the coordinate values for the given seed produce the
        /// expected results.
        /// </summary>
        [Test]
        [TestCase( 1234u, 0, 0, 0.5f )]
        [TestCase( 1234u, 1.283f, 3.183f, 0.647818804f )]
        [TestCase( 1235u, 1.283f, 3.183f, 0.708170533f )]
        [TestCase( 1235u, 0.283f, 3.183f, 0.648635268f )]
        [TestCase( 1235u, 0.483f, 0.483f, 0.655749559f )]
        [TestCase( 1235u, 0.753f, 0.813f, 0.697115421f )]
        public void NoiseGeneratesStandardValues( uint seed, float x, float y, float expectedValue )
        {
            var pn = new PerlinNoise( seed );
            var actualValue = pn.Noise( x, y );

            Assert.That( actualValue, Is.EqualTo( expectedValue ) );
        }

        /// <summary>
        /// Ensures the distribution of values is correct. This is not really
        /// a test but a way to easily see the distribution.
        /// </summary>
        [Test]
        public void DistributionTest()
        {
            var pn = new PerlinNoise( 1234u );
            var distribution = new int[11];

            for ( int y = 0; y < 1_000; y++ )
            {
                for ( int x = 0; x < 1_000; x++ )
                {
                    var value = pn.Noise( x / 283.0f, y / 193.0f );
                    var valueInt = ( int ) Math.Round( value * 10.0f );

                    distribution[valueInt]++;
                }
            }

            Assert.Pass( $"Distribution: {string.Join( ", ", distribution.Select( v => v.ToString() ) )}" );
        }
    }
}