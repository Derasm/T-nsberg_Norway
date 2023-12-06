using Shared.Common.Models;
namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// a) Create a LINQ expression for all guest names that occur on more than once
        /// (across all invoice groups and invoices, not per invoice group or invoice).
        /// </summary>
        [TestMethod]
        public void Task_a_LINQ()
        {
            List<InvoiceGroup> invoiceGroups = new List<InvoiceGroup>();
            IEnumerable<string> repeatedGuestNames = invoiceGroups
                .SelectMany(invoiceGroup => invoiceGroup.Invoices) // first we flatten the list of invoices and invoice groups - its a union of two lists
                .SelectMany(invoice => invoice.Observations)
                .GroupBy(observation => observation.GuestName) // then group by guest name
                .Where(group => group.Count() > 1) // filter out groups with more than one observation
                .Select(group => group.Key) // select the guest name
                .ToList(); // convert to a list
        }
        /// <summary>
        /// b) Create a LINQ expression for the total number of nights per travel agent for invoice groups issued in 2015.
        /// </summary>
        [TestMethod]
        public void Task_b_LINQ()
        {
            List<InvoiceGroup> invoiceGroups = new List<InvoiceGroup>();

            IEnumerable<TravelAgentInfo> numberOfNightsByTravelAgent =
                invoiceGroups.Where(igs => igs.IssueDate.Year == 2015) //Get invoice groups issued in 2015
                .SelectMany(igs => igs.Invoices.SelectMany(i => i.Observations))
                .GroupBy(observation => observation.TravelAgent)// Flatten the list of observations
                .Select(group => new TravelAgentInfo // select the travel agent and total number of nights
                {
                    TravelAgent = group.Key,
                    TotalNumberOfNights = group.Sum(observation => observation.NumberOfNights)
                })
                .ToList(); // convert to list. 

        }
        [TestMethod]
        public void Task_c_SQL()
        {
            List<InvoiceGroup> invoiceGroups = new List<InvoiceGroup>();

            IEnumerable<TravelAgentInfo> numberOfNightsByTravelAgent =
                invoiceGroups.Where(igs => igs.IssueDate.Year == 2015) //Get invoice groups issued in 2015
                .SelectMany(igs => igs.Invoices.SelectMany(i => i.Observations))
                .GroupBy(observation => observation.TravelAgent)// Flatten the list of observations
                .Select(group => new TravelAgentInfo // select the travel agent and total number of nights
                {
                    TravelAgent = group.Key,
                    TotalNumberOfNights = group.Sum(observation => observation.NumberOfNights)
                })
                .ToList(); // convert to list. 

        }
    }
}