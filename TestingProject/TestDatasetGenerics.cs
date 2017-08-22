using Microsoft.VisualStudio.TestTools.UnitTesting;
using services.Models;
using services.Models.Data;

namespace TestingProject
{

    /**
     * This test assumes CDMS_PROD_LOCAL database. 
     * 
     * There are three types of entities in CDMS:
     * 1) CDMS System Entities - system-level classes like:
     *      - projects
     *      - datasets
     *      - locations
     *      - activities
     *  2) CDMS "Core" Datasets - datasets that are dynamically loaded but
     *     are shared with everyone. These are the fisheries datasets like:
     *      - Adult Weir
     *      - Screw Trap
     *      - StreamNet_NOSA
     *  3) CDMS "Private" Datasets - datasets that are dynamically loaded but
     *     are not shared with everyone. These are datasets that tribes create
     *     that they want to have only within their organization.
     */
    [TestClass]
    public class TestDatasetGenerics
    {
        /**
         * Let's kick it off with a test to make sure an internal
         * CDMS system entity framework class is retrievable
         */ 
        [TestMethod]
        public void TestGetProjectById()
        {
            var db = ServicesContext.Current;
            Project project = db.Projects.Find(1140);

            Assert.AreEqual("Steelhead Supplementation Evaluation", project.Name);
        }


        /**
         * Test if a core CDMS dataset is being loaded properly. We'll use
         *  "Adult Weir" dataset as our test example.
         */ 
        [TestMethod]
        public void TestGetCoreDatasetByActivityId()
        {
            //var db = ServicesContext.Current;
            AdultWeir ds = new AdultWeir(412); //an activityID that exists 
            Assert.AreEqual(412, ds.Header.ActivityId); //if the header loaded, the activity id will be set
        }

        /**
         * Test if a private CDMS dataset is being loaded properly. We'll use
         *  "Appraisal" dataset as our test example.
         */
        [TestMethod]
        public void TestGetPrivateDatasetByActivityId()
        {
            //var db = ServicesContext.Current;
            Appraisal ds = new Appraisal(18699);
            ds.Header.Allotment = "537";
            Assert.AreEqual(18699, ds.Header.ActivityId); //if the header loaded, the activity id will be set
        }

    }
}
