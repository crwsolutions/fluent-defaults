using FluentDefaults.Extensions;

namespace FluentDefaults.Tests;

public class SimpleCascadingDefaultForTests
{
    [Fact]
    public void Year_ShouldCascade_ShouldBeSetToDefault()
    {
        var savingsPlan = new SavingsPlanDto
        {
            PersonId = "1",
            WithdrawalStartYear = 2030,
            WithdrawalStartMonth = 2,
            WithdrawalValue = 1,
        };
        var defaulter = new SavingsPlanDtoDefaulter(TestTimeProvider.Date20230101);

        var savingsPlanWithDefaults = savingsPlan.ApplyDefaulter(defaulter);

        Assert.Equal(savingsPlan, savingsPlanWithDefaults);
        Assert.Equal("1", savingsPlan.Id);
        Assert.Equal(2023, savingsPlan.PeriodStartYear);
        Assert.Equal(1, savingsPlan.PeriodStartMonth);
        Assert.Equal(2034, savingsPlan.PeriodEndYear);
        Assert.Equal(1, savingsPlan.PeriodEndMonth);
        Assert.Equal(2023, savingsPlan.DepositStartYear);
        Assert.Equal(1, savingsPlan.DepositStartMonth);
        Assert.Equal(2030, savingsPlan.DepositEndYear);
        Assert.Equal(2, savingsPlan.DepositEndMonth);
        Assert.Equal(2030, savingsPlan.WithdrawalStartYear);
        Assert.Equal(2, savingsPlan.WithdrawalStartMonth);
        Assert.Equal(2034, savingsPlan.WithdrawalEndYear);
        Assert.Equal(1, savingsPlan.WithdrawalEndMonth);
    }
}

public sealed class SavingsPlanDtoDefaulter : AbstractDefaulter<SavingsPlanDto>
{
    private int _counter = 1;
    private int _id = 1;

    public SavingsPlanDtoDefaulter(TimeProvider timeProvider)
    {
        DefaultFor(x => x.Name).Is(() => $"SavingsPlan #{_counter++}");
        DefaultFor(x => x.Id).Is(() => _id++.ToString());
        DefaultFor(x => x.InitialAmount).Is(0m);
        DefaultFor(x => x.PeriodStartYear).Is(timeProvider.GetLocalNow().Year);
        DefaultFor(x => x.PeriodStartMonth).Is(1);
        DefaultFor(x => x.PeriodEndYear).Is(timeProvider.GetLocalNow().Year + 11);
        DefaultFor(x => x.PeriodEndMonth).Is(1);
        DefaultFor(x => x.DepositValue).Is(0);
        DefaultFor(x => x.DepositInterval).Is(1);
        DefaultFor(x => x.DepositStartYear).Is(timeProvider.GetLocalNow().Year);
        DefaultFor(x => x.DepositStartMonth).Is(1);

        //these dates follow one another
        DefaultFor(x => x.DepositEndYear).Is(x => x.WithdrawalStartYear).When(x => x.WithdrawalStartYear.HasValue && x.WithdrawalValue > 0);
        DefaultFor(x => x.DepositEndMonth).Is(x => x.WithdrawalStartMonth).When(x => x.WithdrawalStartYear.HasValue && x.WithdrawalValue > 0);

        DefaultFor(x => x.DepositEndYear).Is(x => x.PeriodEndYear);
        DefaultFor(x => x.DepositEndMonth).Is(x => x.PeriodEndMonth);

        DefaultFor(x => x.WithdrawalValue).Is(0);
        DefaultFor(x => x.WithdrawalInterval).Is(1);
        DefaultFor(x => x.WithdrawalStartYear).Is(x => x.DepositEndYear);
        DefaultFor(x => x.WithdrawalStartMonth).Is(x => x.DepositEndMonth);

        DefaultFor(x => x.WithdrawalEndYear).Is(x => x.PeriodEndYear);
        DefaultFor(x => x.WithdrawalEndMonth).Is(x => x.PeriodEndMonth);
    }
}

public partial record SavingsPlanDto
{
    public string? Id { get; set; }
    public required string PersonId { get; init; }
    public string? Name { get; set; }
    public decimal? InitialAmount { get; set; }
    public decimal? DepositValue { get; set; }


    /// <summary>
    /// The interval specifies the number of units of the given frequency between each recurrence. For example, an interval of 2 with a frequency of monthly means the amount recurs every 2 months.
    /// </summary>
    public int? DepositInterval { get; set; }
    public int? DepositStartYear { get; set; }
    public int? DepositStartMonth { get; set; }
    public int? DepositEndYear { get; set; }
    public int? DepositEndMonth { get; set; }
    public decimal? WithdrawalValue { get; set; }

    /// <summary>
    /// The interval specifies the number of units of the given frequency between each recurrence. For example, an interval of 2 with a frequency of monthly means the amount recurs every 2 months.
    /// </summary>
    public int? WithdrawalInterval { get; set; }
    public int? WithdrawalStartYear { get; set; }
    public int? WithdrawalStartMonth { get; set; }
    public int? WithdrawalEndYear { get; set; }
    public int? WithdrawalEndMonth { get; set; }
    public int? PeriodStartYear { get; set; }
    public int? PeriodStartMonth { get; set; }
    public int? PeriodEndYear { get; set; }
    public int? PeriodEndMonth { get; set; }
    public decimal? InterestRate { get; set; }
}

internal class TestTimeProvider : TimeProvider
{
    public static TimeProvider Date20230101 => new TestTimeProvider(2023, 1, 1);

    private readonly DateTimeOffset _dateTimeOffset;

    internal TestTimeProvider(int year, int month, int day)
    {
        _dateTimeOffset = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
    }

    public override DateTimeOffset GetUtcNow() => _dateTimeOffset;
}