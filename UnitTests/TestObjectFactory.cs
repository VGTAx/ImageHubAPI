using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
  public static class TestObjectFactory
  {
    public static ControllerContext CreateControllerContext(string? returnClaimType = null)
    {
      var fakeClaimsPrincipal = new Mock<ClaimsPrincipal>();
            
      if (returnClaimType != null)
      {
        fakeClaimsPrincipal
          .Setup(cp => cp.FindFirst("UserID"))
          .Returns(new Claim("UserID", "expectedValue"));
      }
      else
      {
        fakeClaimsPrincipal
        .Setup(cp => cp.FindFirst("UserID"))
        .Returns((string _) => null);
      }      

      // Создаем фейковый объект ControllerContext
      var fakeControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext
        {
          User = fakeClaimsPrincipal.Object,
        }
      };

      return fakeControllerContext;
    }
  }
}
