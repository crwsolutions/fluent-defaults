namespace FluentDetails.Sample.Model;

public class Person
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int? Age { get; set; }

    public decimal? Discount { get; set; }

    public bool? IsVip { get; set; }
}