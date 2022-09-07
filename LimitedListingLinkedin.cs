using System.Globalization;

namespace XmlSample
{
  public class LimitedListingLinkedin
  {
    #region Header - Optional
    public string LastBuildDate => DateTime.UtcNow.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'", new CultureInfo("en-US"));
    public string? PublisherUrl { get; set; }
    public string? Publisher { get; set; }
    public string ExpectedJobCount => Jobs.Count().ToString();
    #endregion

    public IList<Job> Jobs { get; set; } = new List<Job>();
  }

  public class Job
  {
    public const string WorkPlaceTypesLocal = "On-site";
    public const string WorkPlaceTypesHybrid = "Hybrid";
    public const string WorkPlaceTypesRemote = "Remote";
    public const string BrazilCountryCode = "BR";

    public Job(string partnerJobId,
        string company,
        string title,
        string description,
        string applyUrl,
        string companyId,
        string city,
        string state)
    {
      PartnerJobId = partnerJobId;
      Company = company;
      Title = title;
      Description = description;
      ApplyUrl = applyUrl;
      CompanyId = companyId;
      City = city;
      State = state;
      Country = BrazilCountryCode;
    }
    #region Required 
    public string PartnerJobId { get; private set; }
    public string Company { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ApplyUrl { get; private set; }
    public string CompanyId { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    #endregion

    #region Optional
    public IList<Salary> Salaries { get; set; } = new List<Salary>();
    public string? WorkplaceTypes { get; set; }
    #endregion
  }

  public class Salary
  {
    public const string BrCurrency = "BRL";
    public const string UsCurrency = "USD";

    public SalaryData? HighEnd { get; set; }
    public SalaryData? LowEnd { get; set; }

    public class SalaryData
    {
      public SalaryData(string amount, string currencyCode)
      {
        Amount = amount;
        CurrencyCode = currencyCode;
      }

      public string Amount { get; private set; }
      public string CurrencyCode { get; private set; }
    }
  }
}