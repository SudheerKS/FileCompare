using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Read_CSV_MVC.Controllers;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FIleTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FileUpladTests()
        {
            // arrange
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath("~/Uploads/"));
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            var sut = new HomeController();
            sut.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), sut);

            var file1Mock = new Mock<HttpPostedFileBase>();
            file1Mock.Setup(x => x.FileName).Returns("file1.pdf");
            var file2Mock = new Mock<HttpPostedFileBase>();
            file2Mock.Setup(x => x.FileName).Returns("file2.doc");
            var files = new[] { file1Mock.Object, file2Mock.Object };

            // act
            var actual = sut.Index(files);

            // assert
            file1Mock.Verify(x => x.SaveAs(@"~/Uploads/file1.pdf"));
            file2Mock.Verify(x => x.SaveAs(@"~/Uploads/file2.doc"));
        }
    }
}
