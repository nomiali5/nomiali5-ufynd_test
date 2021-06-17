using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Task3WebApi.Controllers;
using WebExtraction.Model;

namespace WebApiUnitTests
{
    public class Task3ControllerUnitTests
    {
        [Test]
        public void Get_Test_SUCCESS()
        {
            // setup
            var controller = new Task3Controller();
            // Act on Test  
            var actionResult = controller.Get(7295, DateTime.Now.AddYears(-10));

            // Assert the result  
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Result);
            var resultObject = GetObjectResultContent<List<HotelRate>>(actionResult.Result);
            Assert.GreaterOrEqual(resultObject.Count, 1);
        }
        [Test]
        public void Get_Test_NotHotelFound()
        {
            // setup
            var controller = new Task3Controller();
            // Act on Test  
            var actionResult = controller.Get(1, DateTime.Now.AddYears(-10));

            // Assert the result  
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Result);
            var resultObject = GetObjectResultContent<List<HotelRate>>(actionResult.Result);
            Assert.AreEqual(resultObject.Count, 0);
        }

        [Test]
        public void Get_Test_NoMatchingRecords()
        {
            // setup
            var controller = new Task3Controller();
            // Act on Test  
            var actionResult = controller.Get(1, DateTime.Now);

            // Assert the result  
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Result);
            var resultObject = GetObjectResultContent<List<HotelRate>>(actionResult.Result);
            Assert.AreEqual(resultObject.Count, 0);
        }

        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}