using Moq;
using NeoSharp.TestHelpers.AutoMock;
using System;
using System.Linq;

namespace NeoSharp.TestHelpers
{
    public abstract class TestBase
    {
        private readonly MockRepository _mockRepository;
        protected readonly Random Rand;

        public IAutoMockContainer AutoMockContainer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected TestBase()
        {
            Rand = new Random(Environment.TickCount);
            _mockRepository = new MockRepository(MockBehavior.Loose);
            AutoMockContainer = new UnityAutoMockContainer(_mockRepository);
        }

        /// <summary>
        /// Generate random strings
        /// </summary>
        /// <param name="length">String lenght</param>
        /// <returns>String</returns>
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // TODO: Very slow method, for 65K iteration with long text string

            return new string(Enumerable.Repeat(chars, length).Select(s => s[Rand.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generate a random integer
        /// </summary>
        /// <returns>A positive integer</returns>
        public int RandomInt()
        {
            return Rand.Next();
        }
        /// <summary>
        /// Generate a random integer with a max value
        /// </summary>
        /// <param name="max"></param>
        /// <returns>A positive integer that is smaller than max</returns>
        public int RandomInt(int max = int.MaxValue)
        {
            return Rand.Next(max);
        }

        /// <summary>
        /// Verify All
        /// </summary>
        public void VerifyAll()
        {
            _mockRepository.VerifyAll();
        }
    }
}