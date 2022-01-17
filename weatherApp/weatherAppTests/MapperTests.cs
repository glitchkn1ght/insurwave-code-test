//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 16/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherAppTests
{
    using NUnit.Framework;
    using weatherApp.Utility;

    [TestFixture]
    public class MapperTests
    {
        private StandardErrorMapper errorMapper;

        [TestCase(1002, 401)]
        [TestCase(2006, 401)]
        public void TheseApiCodes_MapTo401Unauthorized(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

        [TestCase(1003, 400)]
        [TestCase(1005, 400)]
        [TestCase(1006, 400)]
        [TestCase(9999, 400)]
        public void TheseApiCodes_MapTo400BadRequest(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

        [TestCase(2007, 403)]
        [TestCase(2008, 403)]
        public void TheseApiCodes_MapTo403Forbidden(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

        [TestCase(1234, 500)]
        [TestCase(5, 500)]
        [TestCase(0, 500)]
        public void OtherCodesOrDefault_MapTo500(int apiCode, int expectedHttpCode)
        {
            this.errorMapper = new StandardErrorMapper();

            int actual = this.errorMapper.MapApiErrorCode(apiCode);

            Assert.AreEqual(expectedHttpCode, actual);
        }

    }
}