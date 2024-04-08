using DiscussifyApi.Utils;

namespace DiscussifyApiTests.Utils
{
    public class StringUtilTests
    {
        [Fact]
        public void ToTitleCase_LowerCaseString_ReturnsTitleCase()
        {
            // Arrange
            var lowerCaseString = "lorem ipsum";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(lowerCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToTitleCase_UpperCaseString_ReturnsTitleCase()
        {
            // Arrange
            var upperCaseString = "LOREM IPSUM";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(upperCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToTitleCase_TitleCaseString_ReturnsTitleCase()
        {
            // Arrange
            var titleCaseString = "Lorem Ipsum";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(titleCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void RandomString_GivenLength_ReturnsStringWithGivenLength()
        {
            // Arrange
            var length = 10;

            // Act
            var result = StringUtil.RandomString(length);

            // Assert
            Assert.Equal(length, result.Length);
        }
    }
}