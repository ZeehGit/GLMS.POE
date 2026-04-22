using GLMS.Web.Models;
using Xunit;

namespace GLMS.Tests
{
    public class WorkflowTests
    {
        // Helper mimics controller business rule
        private bool CanCreateServiceRequest(ContractStatus status)
        {
            return status != ContractStatus.Expired &&
                   status != ContractStatus.OnHold;
        }

        // Test 9: Active contract allows service request
        [Fact]
        public void ServiceRequest_ActiveContract_IsAllowed()
        {
            bool result = CanCreateServiceRequest(ContractStatus.Active);
            Assert.True(result);
        }

        // Test 10: Expired contract blocks service request
        [Fact]
        public void ServiceRequest_ExpiredContract_IsBlocked()
        {
            bool result = CanCreateServiceRequest(ContractStatus.Expired);
            Assert.False(result);
        }

        // Test 11: OnHold contract blocks service request
        [Fact]
        public void ServiceRequest_OnHoldContract_IsBlocked()
        {
            bool result = CanCreateServiceRequest(ContractStatus.OnHold);
            Assert.False(result);
        }

        // Test 12: Draft contract allows service request
        [Fact]
        public void ServiceRequest_DraftContract_IsAllowed()
        {
            bool result = CanCreateServiceRequest(ContractStatus.Draft);
            Assert.True(result);
        }
    }
}

/*
* Title: Testing with 'dotnet test'
* Author: Microsoft
* Date: 09 April 2026
* Version: 1
* Availability: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
*/

/*
* Title: xUnit.net
* Author: Getting Started with xUnit.net
* Date: 13 August 2025
* Version: 3
* Availability: https://xunit.net/docs/getting-started/v3/getting-started
*/