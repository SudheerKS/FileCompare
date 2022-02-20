using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Read_CSV_MVC.Controllers;
using System.Web.Mvc;

namespace FileTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UploadFile()
        {
            //Arrange
            HomeController stickerWebController = new home();
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath("~/UserMedia/UploadedStickers/")).Returns(Path.GetFullPath(@"\testfile"));
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            httpContextMock.Setup(x => x.User.Identity.Name).Returns("testcase@user.net");
            stickerWebController.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), stickerWebController);

            string filePath = Path.GetFullPath(@"testfile\falcon.jpg");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();
            uploadedFile.Setup(x => x.FileName).Returns("falcon.jpg");
            uploadedFile.Setup(x => x.ContentType).Returns("image/png");
            uploadedFile.Setup(x => x.InputStream).Returns(fileStream);
            HttpPostedFileBase[] httpPostedFileBases = { uploadedFile.Object };

            var exisitngFolder = db.Contents.Where(c => c.ContentType == ContentType.Folder).FirstOrDefault();

            //Act
            var result = stickerWebController.AddFiles(exisitngFolder.Id, "", httpPostedFileBases) as ActionResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}