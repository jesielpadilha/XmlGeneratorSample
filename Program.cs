using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmlSample;

internal class Program
{
  private static void Main(string[] args)
  {
    var filePath = Path.Combine(".", "job-opportunities.xml");

    List<Job> jobs = new List<Job>
    {
      new Job("2b7a2d25-addf-4a1d-bd5a-0120cd8b5efc",
        "Company Name",
        "Quality Analyst",
        "job description...",
        "https://www.google.com/",
        "46546564",
        "São Paulo",
        "SP")
        {
          Salaries = new List<Salary>
          {
            new Salary{
              HighEnd = new Salary.SalaryData("5000", Salary.BrCurrency),
              LowEnd = new Salary.SalaryData("2000", Salary.BrCurrency)
            }
          },
          WorkplaceTypes = Job.WorkPlaceTypesHybrid
        }
    };

    List<Job> jobs2 = new List<Job>
    {
      new Job("2b7a2d25-addf-4a1d-bd5a-0120cd8b5tgd",
        "Company Name",
        "Senior Developer",
        "job description...",
        "https://www.google.com/",
        "46546564",
        "Florianopolis",
        "SC")
        {
          Salaries = new List<Salary>
          {
            new Salary{
              HighEnd = new Salary.SalaryData("15000", Salary.BrCurrency),
              LowEnd = new Salary.SalaryData("8000", Salary.BrCurrency)
            }
          },
          WorkplaceTypes = Job.WorkPlaceTypesRemote
        }
    };

    CreateJobsXml(filePath, jobs);
    CreateJobsXml(filePath, jobs2, true);
    jobs.AddRange(jobs2);

    var document = new LimitedListingLinkedin
    {
      PublisherUrl = "https://www.google.com/",
      Publisher = "ATS",
      Jobs = jobs
    };

    AddDocumentationHeader(filePath, document);
  }

  private static void CreateJobsXml(string filePath, IList<Job> jobs, bool appendFile = false)
  {
    if (!appendFile)
    {
      var xmlSettings = new XmlWriterSettings
      {
        Indent = true,
        Encoding = Encoding.UTF8
      };
      using (var writer = XmlWriter.Create(filePath, xmlSettings))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("source");
        foreach (var job in jobs)
        {
          writer.WriteStartElement("job");
          writer.WriteElementString("partnerJobId", SetCdataFormat(job.PartnerJobId));
          writer.WriteElementString("company", SetCdataFormat(job.Company));
          writer.WriteElementString("title", SetCdataFormat(job.Title));
          writer.WriteElementString("description", SetCdataFormat(job.Description));
          writer.WriteElementString("applyUrl", SetCdataFormat(job.ApplyUrl));
          writer.WriteElementString("companyId", SetCdataFormat(job.CompanyId));
          writer.WriteElementString("city", SetCdataFormat(job.City));
          writer.WriteElementString("state", SetCdataFormat(job.State));
          writer.WriteElementString("country", SetCdataFormat(job.Country));

          //optional params 
          if (job.Salaries.Any(s => s.HighEnd != null || s.LowEnd != null))
          {
            writer.WriteStartElement("salaries");
            foreach (var salary in job.Salaries)
            {
              writer.WriteStartElement("salary");
              if (salary.HighEnd != null)
              {
                writer.WriteStartElement("highEnd");
                writer.WriteElementString("amount", SetCdataFormat(salary.HighEnd.Amount));
                writer.WriteElementString("currencyCode", salary.HighEnd.CurrencyCode);
                writer.WriteEndElement();
              }
              if (salary.LowEnd != null)
              {
                writer.WriteStartElement("lowEnd");
                writer.WriteElementString("amount", SetCdataFormat(salary.LowEnd.Amount));
                writer.WriteElementString("currencyCode", salary.LowEnd.CurrencyCode);
                writer.WriteEndElement();
              }
              writer.WriteEndElement();
            }
            writer.WriteEndElement();
          }
          if (!string.IsNullOrEmpty(job.WorkplaceTypes))
            writer.WriteElementString("workplaceTypes", SetCdataFormat(job.WorkplaceTypes));

          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.Flush();
      };
    }
    else
    {
      var xDocument = XDocument.Load(filePath);
      if (xDocument == null) return;

      var root = xDocument.Element("source");
      if (root == null) return;

      foreach (var job in jobs)
      {
        XElement newRow = root.Descendants("job").Last();
        newRow.AddAfterSelf(
          new XElement("job",
            new XElement("partnerJobId", SetCdataFormat(job.PartnerJobId)),
            new XElement("company", SetCdataFormat(job.Company)),
            new XElement("title", SetCdataFormat(job.Title)),
            new XElement("description", SetCdataFormat(job.Description)),
            new XElement("applyUrl", SetCdataFormat(job.ApplyUrl)),
            new XElement("companyId", SetCdataFormat(job.CompanyId)),
            new XElement("city", SetCdataFormat(job.City)),
            new XElement("state", SetCdataFormat(job.State)),
            new XElement("country", SetCdataFormat(job.Country))
          )
        );

        //optional params 
        XElement currentJobRow = root.Descendants("job").Last();
        if (job.Salaries.Any(s => s.HighEnd != null || s.LowEnd != null))
        {
          var salariesElements = new XElement("salaries");
          foreach (var salary in job.Salaries)
          {
            var salaryElement = new XElement("salary");
            if (salary.HighEnd != null)
            {
              var highEndElement = new XElement("highEnd",
                new XElement("amount", SetCdataFormat(salary.HighEnd.Amount)),
                new XElement("currencyCode", salary.HighEnd.CurrencyCode)
              );
              salaryElement.Add(highEndElement);
            }
            if (salary.LowEnd != null)
            {
              var lowEndElement = new XElement("lowEnd",
                new XElement("amount", SetCdataFormat(salary.LowEnd.Amount)),
                new XElement("currencyCode", salary.LowEnd.CurrencyCode)
              );
              salaryElement.Add(lowEndElement);
            }
            salariesElements.Add(salaryElement);
          }
          currentJobRow.Add(salariesElements);
        }
        if (!string.IsNullOrEmpty(job.WorkplaceTypes))
        {
          currentJobRow.Add(new XElement("workplaceTypes", SetCdataFormat(job.WorkplaceTypes)));
        }
      }

      xDocument.Save(filePath);
    }
  }

  private static void AddDocumentationHeader(string filePath, LimitedListingLinkedin document)
  {
    var xDocument = XDocument.Load(filePath);
    if (xDocument == null) return;

    var root = xDocument.Element("source");
    if (root == null) return;

    root.AddFirst(
        new XElement("lastBuildDate", document.LastBuildDate),
        new XElement("publisherUrl", document.PublisherUrl),
        new XElement("publisher", document.Publisher),
        new XElement("expectedJobCount", SetCdataFormat(document.ExpectedJobCount))
    );
    xDocument.Save(filePath);
  }

  private static string SetCdataFormat(string content)
    => $"<![CDATA[{content}]]>";
}