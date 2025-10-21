namespace FluentDefaults.Tests.Model;

internal sealed class Address
{
    public int Id { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }

    public string Region { get; set; } = "Far, far away";

    public List<HouseSpec>? Specs { get; set; }
}

internal sealed class HouseSpec
{
    public SpecsEnum? Spec { get; set; }
    public string? Description { get; set; }
}

internal enum SpecsEnum
{
    Yard,
    Garage,
    Pool
}