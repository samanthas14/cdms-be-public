using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using services.Controllers;
using services.Models;
using System.Collections.Generic;
using services.Models.Data;

namespace TestingProject
{

    /**
     * This test requires the CDMS_SAMPLE database.
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
            Project project = db.Projects.Find(1);

            Assert.AreEqual("Demo Watertemp Project", project.Name);
        }


        /**
         * Test if a core CDMS dataset is being loaded properly. We'll use
         *  "NOSA" dataset as our test example.
         */ 
        [TestMethod]
        public void TestGetCoreDatasetByActivityId()
        {
            var db = ServicesContext.Current;
            StreamNet_NOSA ds = new StreamNet_NOSA(13); //13 is an activityID that exists in the Sample database
            Assert.AreEqual(13, ds.Header.ActivityId); //if the header loaded, the activity id will be set
        }

        /**
         * Test if a private CDMS dataset is being loaded properly. We'll use
         *  "Appraisal" dataset as our test example.
         */
        [TestMethod]
        public void TestGetPrivateDatasetByActivityId()
        {
            var db = ServicesContext.Current;
            Appraisal ds = new Appraisal(1013);
            ds.Header.AllotmentName = "Test Appraisal";
            Assert.AreEqual(1013, ds.Header.ActivityId); //if the header loaded, the activity id will be set
        }

    }
}
