using API.Infrastructure.Classes;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp;
using System.Globalization;
using System.IO;
using API.Infrastructure.Helpers;
using System.Drawing;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementPdfRepository : ILeaseAgreementPdfRepository {

        public string BuildPdf(LeaseAgreementVM leaseAgreement) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XFont tahoma = new("Tahoma", 8);
            XFont monoType = new("Tahoma", 8);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            AddLogo(gfx);
            AddCompany(gfx, tahoma);
            AddTitle(gfx, monoType);
            AddBoat(gfx, tahoma, leaseAgreement.Boat, leaseAgreement.Period);
            // AddInsurance(gfx, tahoma, leaseAgreement.Insurance);
            // AddOwner(gfx, tahoma, leaseAgreement.Owner);
            // AddBilling(gfx, tahoma, leaseAgreement.Billing);
            // AddCaptain(gfx, tahoma);
            // AddFee(gfx, tahoma, leaseAgreement.Fee, locale);
            // AddPeriod(gfx, robotoMonoFont, leaseAgreement.Period);
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
            XImage image = XImage.FromFile(Path.Combine("Images" + Path.DirectorySeparatorChar + "Logo.jpeg"));
            gfx.DrawImage(image, 40, 20, 100, 100);
        }

        private static void AddCompany(XGraphics gfx, XFont font) {
            var top = 50;
            var left = 200;
            gfx.DrawString("MARITIME PROJECT IKE", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("ΜΠΕΝΙΤΣΕΣ 49084, ΚΕΡΚΥΡΑ", font, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString("ΑΦΜ 801515394 ΔΟΥ ΚΕΡΚΥΡΑΣ", font, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString("ΓΕΜΗ 158323233000", font, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString("ΤΗΛ. +30 26610 72627", font, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString("info@benitsesmarina.com", font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddTitle(XGraphics gfx, XFont font) {
            var top = 130;
            var left = 40;
            var text = "Σύμβαση παραμονής σκάφους"; gfx.DrawString(text, font, XBrushes.Black, new XPoint(left, top));
            text = "Vessel berth lease agreement"; gfx.DrawString(text, font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddBoat(XGraphics gfx, XFont font, LeaseAgreementBoatVM boat, LeaseAgreementPeriodVM period) {
            var top = 160;
            var left = 40;
            gfx.DrawString("Ονομα σκάφους", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Boat name", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Λιμάνι νηολόγησης", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Port of registry", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Ολικό μήκος", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Length overall", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Μέγιστο πλάτος", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Max beam", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Μέγιστο βύθισμα", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Max draft", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Περίοδος ελλιμενισμού από", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Mooring period from", font, XBrushes.Black, new XPoint(left, top += 15));
            top = 160;
            left = 300;
            gfx.DrawString("Σημαία", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Flag", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Αρ. νηολογίου", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Registry No", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Τύπος σκάφους", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Boat type", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Χρήση", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Usage", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Περίοδος ελλιμενισμού έως", font, XBrushes.Black, new XPoint(left, top += 45));
            gfx.DrawString("Mooring period to", font, XBrushes.Black, new XPoint(left, top += 15));
            // top = 150;
            // left = 200;
            // gfx.DrawString("Σημαία", font, XBrushes.Black, new XPoint(left, top));
            // gfx.DrawString("Flag", font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString("Αρ. νηολογίου", font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString("Registry No", font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString("Χρήση", font, XBrushes.Black, new XPoint(left, top += 30));
            // gfx.DrawString("Usage", font, XBrushes.Black, new XPoint(left, top += 10));

            // gfx.DrawString("Περίοδος ελλιμενισμού έως", font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Name, font, XBrushes.Black, new XPoint(left, top));
            // gfx.DrawString(boat.Flag, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Loa, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Beam, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Draft, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.RegistryPort, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.RegistryNo, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Type, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(boat.Usage, font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddInsurance(XGraphics gfx, XFont font, LeaseAgreementInsuranceVM insurance) {
            var top = 310;
            var left = 40;
            gfx.DrawString("Ασφαλιστική εταιρία", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Insurance company", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Αρ. ασφαλιστηρίου", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Policy No", font, XBrushes.Black, new XPoint(left, top += 15));
            top = 340;
            left = 300;
            gfx.DrawString("Ημερομηνία λήξης", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Valid until", font, XBrushes.Black, new XPoint(left, top += 15));
            // gfx.DrawString(insurance.InsuranceCompany, font, XBrushes.Black, new XPoint(left, top));
            // gfx.DrawString(insurance.PolicyNo, font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddOwner(XGraphics gfx, XFont font, LeaseAgreementPersonVM owner) {
            var top = 370;
            var left = 40;
            gfx.DrawString("Στοιχεία πλοιοκτήτη / Owner details", font, XBrushes.Black, new XPoint(left, top));
            left = 40;
            gfx.DrawString("Ονοματεπώνυμο", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Full name", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Διεύθυνση", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Address", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("ΑΦΜ", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Tax No", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("ΔΟΥ", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Tax Office", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Τηλέφωνα", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Phone numbers", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Email", font, XBrushes.Black, new XPoint(left, top += 15));
            // gfx.DrawString(owner.Name, font, XBrushes.Black, new XPoint(left, top += 15));
            // gfx.DrawString(owner.Address, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(owner.TaxNo, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(owner.TaxOffice, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(owner.PassportNo, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(owner.Phones, font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(owner.Email, font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddBilling(XGraphics gfx, XFont font, LeaseAgreementPersonVM billing) {
            var top = 370;
            var left = 300;
            gfx.DrawString("Στοιχεία τιμολόγησης / Invoice details", font, XBrushes.Black, new XPoint(left += 60, top));
            left = 300;
            gfx.DrawString("Ονοματεπώνυμο", font, XBrushes.Black, new XPoint(left, top += 20));
            gfx.DrawString("Full name", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Διεύθυνση", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Address", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("ΑΦΜ", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Tax No", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("ΔΟΥ", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Tax Office", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Τηλέφωνα", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Phone numbers", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Email", font, XBrushes.Black, new XPoint(left, top += 15));
        }

        private static void AddCaptain(XGraphics gfx, XFont font) {
            var top = 550;
            var left = 40;
            gfx.DrawString("Στοιχεία πλοιάρχου / Captain details", font, XBrushes.Black, new XPoint(left, top));
            top = 565;
            left = 40;
            gfx.DrawString("Ονοματεπώνυμο", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Full name", font, XBrushes.Black, new XPoint(left, top += 15));
            left = 300;
            gfx.DrawString("Τηλέφωνα", font, XBrushes.Black, new XPoint(left, top -= 15));
            gfx.DrawString("Phones", font, XBrushes.Black, new XPoint(left, top += 15));
        }

        private static void AddFee(XGraphics gfx, XFont font, LeaseAgreementFeeVM fee, CultureInfo locale) {
            var top = 595;
            var left = 40;
            gfx.DrawString("Χρεώσεις / Charges", font, XBrushes.Black, new XPoint(left, top));
            left = 40;
            gfx.DrawString("Καθαρή αξία", font, XBrushes.Black, new XPoint(left, top += 15));
            gfx.DrawString("Net amount", font, XBrushes.Black, new XPoint(left, top += 15));
            top = 610;
            left = 210;
            gfx.DrawString("Αξία ΦΠΑ 24%", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("VAT amount 24%", font, XBrushes.Black, new XPoint(left, top += 15));
            top = 610;
            left = 400;
            gfx.DrawString("Σύνολο αξίας ", font, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString("Total amount", font, XBrushes.Black, new XPoint(left, top += 15));
            // gfx.DrawString(fee.NetAmount.ToString("N2", locale), font, XBrushes.Black, new XPoint(left, top));
            // gfx.DrawString(fee.VatAmount.ToString("N2", locale), font, XBrushes.Black, new XPoint(left, top += 10));
            // gfx.DrawString(fee.GrossAmount.ToString("N2", locale), font, XBrushes.Black, new XPoint(left, top += 10));
        }

        private static void AddPeriod(XGraphics gfx, XFont robotoMonoFont, LeaseAgreementPeriodVM period) {
            var top = 490;
            var left = 40;
            gfx.DrawString(DateHelpers.FormatDateStringToLocaleString(period.FromDate), robotoMonoFont, XBrushes.Black, new XPoint(left, top));
            gfx.DrawString(DateHelpers.FormatDateStringToLocaleString(period.ToDate), robotoMonoFont, XBrushes.Black, new XPoint(left, top += 15));
        }

        private static XSolidBrush SetTextColor(int value) {
            return value == 0 ? XBrushes.LightGray : XBrushes.Black;
        }

    }

}