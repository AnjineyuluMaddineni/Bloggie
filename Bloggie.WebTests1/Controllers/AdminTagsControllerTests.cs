using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bloggie.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Controllers.Tests
{
    [TestClass()]
    public class AdminTagsControllerTests
    {
        private ITagRepository _tagRepository=null;
        public AdminTagsControllerTests()
        {
            if (_tagRepository==null)
            {
                _tagRepository = new TagRepository();
            }
        }
        [TestMethod()]
        public void AddTest()
        {

        }
    }
}