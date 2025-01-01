namespace TimeInABottle.Core.Models.Filters;
/// <summary>
/// Represents a filter that can be applied to a value.
/// </summary>
public interface IValueFilter : IFilter
{
    /// <summary>
    /// Gets or sets the criteria for the filter.
    /// </summary>
    string Criteria { get; set; }
}
