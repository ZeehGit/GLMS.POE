using Xunit;

namespace GLMS.Tests
{
    public class FileValidationTests
    {
        // Helper method that mimics the controller validation logic
        private bool IsValidPdfFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            return Path.GetExtension(fileName).ToLower() == ".pdf";
        }

        // Test 4: PDF file is accepted
        [Fact]
        public void FileValidation_PdfFile_IsValid()
        {
            bool result = IsValidPdfFile("agreement.pdf");
            Assert.True(result);
        }

        // Test 5: EXE file is rejected
        [Fact]
        public void FileValidation_ExeFile_IsInvalid()
        {
            bool result = IsValidPdfFile("malicious.exe");
            Assert.False(result);
        }

        // Test 6: DOCX file is rejected
        [Fact]
        public void FileValidation_DocxFile_IsInvalid()
        {
            bool result = IsValidPdfFile("document.docx");
            Assert.False(result);
        }

        // Test 7: Empty file name is rejected
        [Fact]
        public void FileValidation_EmptyFileName_IsInvalid()
        {
            bool result = IsValidPdfFile("");
            Assert.False(result);
        }

        // Test 8: Case insensitive — .PDF accepted
        [Fact]
        public void FileValidation_UppercasePdf_IsValid()
        {
            bool result = IsValidPdfFile("AGREEMENT.PDF");
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