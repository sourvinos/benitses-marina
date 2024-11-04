using API.Infrastructure.Classes;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp;
using System.Globalization;
using System.IO;
using API.Infrastructure.Helpers;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementPdfRepository : ILeaseAgreementPdfRepository {

        public string BuildPdf(LeaseAgreementVM leaseAgreement) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XFont logoFont = new("ACCanterBold", 20);
            XFont robotoMonoFont = new("RobotoMono", 7);
            XFont robotoMonoFontBig = new("RobotoMono", 8);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            AddLogo(gfx);
            AddHeader(gfx, logoFont);
            AddBoat(gfx, robotoMonoFont, leaseAgreement.Boat);
            AddInsurance(gfx, robotoMonoFont, leaseAgreement.Insurance);
            AddOwner(gfx, robotoMonoFont, leaseAgreement.Owner);
            AddBilling(gfx, robotoMonoFont, leaseAgreement.Billing);
            AddFee(gfx, robotoMonoFont, leaseAgreement.Fee, locale);
            AddPeriod(gfx, robotoMonoFont, leaseAgreement.Period);
            var filename = leaseAgreement.ReservationId + ".pdf";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + filename);
            document.Save(fullpathname);
            return filename;
        }

        public FileStreamResult OpenPdf(string filename) {
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        private static void AddLogo(XGraphics gfx) {
            XImage image = XImage.FromFile(Path.Combine("Images" + Path.DirectorySeparatorChar + "Background.jpg"));
            gfx.DrawImage(image, 40, 20, 100, 100);
        }

        private static void AddHeader(XGraphics gfx, XFont logoFont) {
            var top = 42;
            var left = 40;
            gfx.DrawString("Lease Agreement", logoFont, XBrushes.Black, new XPoint(left += 95, top));
        }

        private static void AddBoat(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementBoatVM boat) {
            var top = 150;
            var left = 40;
            gfx.DrawString(boat.Name, robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(boat.Flag, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.Loa, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.Beam, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.Draft, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.RegistryPort, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.RegistryNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.Type, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(boat.Usage, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddInsurance(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementInsuranceVM insurance) {
            var top = 250;
            var left = 40;
            gfx.DrawString(insurance.InsuranceCompany, robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(insurance.PolicyNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(insurance.PolicyEnds, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddOwner(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementPersonVM owner) {
            var top = 290;
            var left = 40;
            gfx.DrawString(owner.Name, robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(owner.Address, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(owner.TaxNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(owner.TaxOffice, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(owner.PassportNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(owner.Phones, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(owner.Email, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddBilling(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementPersonVM billing) {
            var top = 370;
            var left = 40;
            gfx.DrawString(billing.Name, robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(billing.Address, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(billing.TaxNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(billing.TaxOffice, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(billing.PassportNo, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(billing.Phones, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(billing.Email, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddFee(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementFeeVM fee, CultureInfo locale) {
            var top = 450;
            var left = 40;
            gfx.DrawString(fee.NetAmount.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(fee.VatAmount.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(fee.GrossAmount.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddPeriod(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementPeriodVM period) {
            var top = 490;
            var left = 40;
            gfx.DrawString(DateHelpers.FormatDateStringToLocaleString(period.FromDate), robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(DateHelpers.FormatDateStringToLocaleString(period.ToDate), robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static XSolidBrush SetTextColor(int value) {
            return value == 0 ? XBrushes.LightGray : XBrushes.Black;
        }

    }

}