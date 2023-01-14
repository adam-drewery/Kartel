namespace Zoopla.ApiClient;

public enum Furnishing
{
    [Parameter("furnished")] Furnished,
    [Parameter("unfurnished")] Unfurnished,
    [Parameter("part-furnished")] PartFurnished
}