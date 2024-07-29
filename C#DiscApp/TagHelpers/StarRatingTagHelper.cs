using Microsoft.AspNetCore.Razor.TagHelpers;

namespace C_DiscApp.TagHelpers
{
    [HtmlTargetElement("star-rating")]
    public class StarRatingTagHelper : TagHelper
    {
        public int Rating { get; set; }
        public int MaxRating { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "star-rating");

            for (int i = 1; i <= MaxRating; i++)
            {
                if (i <= Rating)
                {
                    output.Content.AppendHtml("<span class='star filled'>&#9733;</span>");
                }
                else
                {
                    output.Content.AppendHtml("<span class='star'>&#9733;</span>");
                }
            }
        }
    }
}