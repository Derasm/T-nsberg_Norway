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
        /**
         * Given corresponding database tables TravelAgent and Observation, both with a TravelAgent field being the primary and foreign key, respectively:

            c) Write a SQL query that finds all travel agents that does not have any observations.

            SELECT * FROM TravelAgent ...

            

         */
        [TestMethod]
        public void Task_c_SQL()
        {
            List<InvoiceGroup> invoiceGroups = new List<InvoiceGroup>();
            //Using left join to adhere TravelAgent table to Observation table
            string sqlQuery = "" +
                "SELECT TravelAgent.* " +
                "FROM TravelAgent " +
                "LEFT JOIN Observation ON TravelAgent.TravelAgent = Observation.TravelAgent " +
                "WHERE Observation.TravelAgent IS NULL"; // This assumes the field is nullable, and not just set to 0 or empty string.
                //If that is the case
                //"WHERE Observation.TravelAgent = 0" Instead.

        }

        //d) Write a SQL query that finds all travel agents that have more than two observations.
    [TestMethod]
        public void Task_d_SQL()
        {
            List<InvoiceGroup> invoiceGroups = new List<InvoiceGroup>();
            //Using left join to adhere TravelAgent table to Observation table
            string sqlQuery = "" +
               "SELECT TravelAgent.* " + //Select all travel agents only. * could have been replaced with TravelAgent.TravelAgent
               "FROM TravelAgent " + // Table to look at
               "JOIN Observation ON TravelAgent.TravelAgent = Observation.TravelAgent " + //Join the observation table to the travel agent table as union
               "GROUP BY TravelAgent.TravelAgent " + // group them by the travelagent field
               "HAVING COUNT(Observation.TravelAgent) > 2"; // HAVING filters based on the return of the COUNT portion.
        }
    }
}