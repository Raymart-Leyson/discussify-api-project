using DiscussifyApi.Utils;

namespace DiscussifyApiTests.Utils
{
    public class CryptoTests
    {
        [Fact]
        public void Hash_GivenPassword_ReturnsHashedPassword()
        {
            // Arrange
            var password = "password";

            // Act
            var result = Crypto.Hash(password);

            // Assert
            Assert.NotEqual(password, result);
        }

        [Fact]
        public void VerifyPassword_GivenPasswordAndHashedPassword_ReturnsTrue()
        {
            // Arrange
            var password = "password";
            var hashedPassword = Crypto.Hash(password);

            // Act
            var result = Crypto.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.True(result);
        }
    }
}