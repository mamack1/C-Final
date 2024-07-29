using C_DiscApp.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C_DiscAppUnitTests
{
    public class StarRatingTagHelperTests
    {
        [Fact]
        public void Process_GeneratesCorrectStarRatingHtml()
        {
            // Arrange
            var tagHelper = new StarRatingTagHelper { Rating = 4, MaxRating = 5 };
            var context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            var output = new TagHelperOutput("star-rating", new TagHelperAttributeList(), (useCachedResult, encoder) =>
                Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(context, output);

            // Assert
            Assert.Equal("div", output.TagName);
            Assert.Equal("star-rating", output.Attributes["class"].Value);
            Assert.Equal(
                "<span class='star filled'>&#9733;</span><span class='star filled'>&#9733;</span><span class='star filled'>&#9733;</span><span class='star filled'>&#9733;</span><span class='star'>&#9733;</span>",
                output.Content.GetContent());
        }
    }
}