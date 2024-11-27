using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementRepository : ILeaseAgreementRepository {

        public void BuildLeaseAgreement() {
            var document = new Document();
            Style style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            var section = document.AddSection();
            section.PageSetup.TopMargin = 40;
            LogoAndCompany(section);
            Spacer(section);
            Header(section);
            Spacer(section);
            VesselNameAndFlag(section);
            PortAndVesselRegistryNo(section);
            VesselDimensions(section);
            VesselTypeAndUse(section);
            MooringPeriod(section);
            InsuranceCompany(section);
            PolicyNoAndExpireDate(section);
            Spacer(section);
            PersonHeaders(section);
            PersonNames(section);
            PersonAddresses(section);
            PersonVATs(section);
            PersonTaxOffices(section);
            PersonIdNumbers(section);
            PersonPhoneNumbers(section);
            PersonEmails(section);
            Spacer(section);
            CaptainHeaders(section);
            CaptainDetails(section);
            Fees(section);
            SmallTerms(section);
            Spacer(section);
            Signatures(section);
            SignatureSpaces(section);
            SavePdf(document);
        }

        private static Row LogoAndCompany(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("5cm");
            table.AddColumn("11cm");
            Row row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph().AddImage("Images/Background.jpg").Width = "2.5cm";
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("MARITIME PROJECT Ι.Κ.Ε.").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("ΜΠΕΝΙΤΣΕΣ, 49084, ΚΕΡΚΥΡΑ").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Α.Φ.Μ. 801515394  Δ.Ο.Υ. ΚΕΡΚΥΡΑΣ").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Αρ. Γ.E.ΜΗ. 158323233000").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("ΤΗΛ. +30 26610 72627").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("https://benitsesmarina.com").Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Email: info@benitsesmarina.com").Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row Header(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("16cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΣΥΜΒΑΣΗ ΠΑΡΑΜΟΝΗΣ ΣΚΑΦΟΥΣ\nVESSEL'S BERTH LEASE AGREEMENT");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row VesselNameAndFlag(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονομα σκάφους");
            row.Cells[0].AddParagraph("Vessel's name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("ASPA V");
            row.Cells[2].AddParagraph("Σημαία");
            row.Cells[2].AddParagraph("Flag");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("GREEK");
            return row;
        }

        private static Row PortAndVesselRegistryNo(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Λιμάνι νηολόγησης");
            row.Cells[0].AddParagraph("Port of registry");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("CORFU");
            row.Cells[2].AddParagraph("Αρ. νηολογίου");
            row.Cells[2].AddParagraph("Registry No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("74656");
            return row;
        }

        private static Row VesselDimensions(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("2.34cm");
            table.AddColumn("3cm");
            table.AddColumn("2.34cm");
            table.AddColumn("3cm");
            table.AddColumn("2.32cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ολικό μήκος");
            row.Cells[0].AddParagraph("Overall length");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("15m");
            row.Cells[2].AddParagraph("Μέγιστο πλάτος");
            row.Cells[2].AddParagraph("Max beam");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("4.5m");
            row.Cells[4].AddParagraph("Μέγιστο βύθισμα");
            row.Cells[4].AddParagraph("Max draft");
            row.Cells[4].Borders.Right.Clear();
            row.Cells[5].AddParagraph("4.5m");
            return row;
        }

        private static Row VesselTypeAndUse(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Τύπος σκάφους");
            row.Cells[0].AddParagraph("Vessel type");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("M/Y");
            row.Cells[2].AddParagraph("Χρήση σκάφους");
            row.Cells[2].AddParagraph("Vessel use");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("PROFESSIONAL");
            return row;
        }

        private static Row MooringPeriod(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Εναρξη ελλιμενισμού");
            row.Cells[0].AddParagraph("Mooring from");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("01/01/2024");
            row.Cells[2].AddParagraph("Λήξη ελλιμενισμού");
            row.Cells[2].AddParagraph("Mooring until");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("31/12/2024");
            return row;
        }

        private static Row InsuranceCompany(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("4cm");
            table.AddColumn("12cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ασφαλιστική εταιρία");
            row.Cells[0].AddParagraph("Insurance company");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("AXA INSURANCE");
            return row;
        }

        private static Row PolicyNoAndExpireDate(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Αρ. ασφαλιστηρίου");
            row.Cells[0].AddParagraph("Policy No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("21.141605");
            row.Cells[2].AddParagraph("Ημερ/νία λήξης");
            row.Cells[2].AddParagraph("Valid until");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("31/12/2024");
            return row;
        }

        private static Row PersonHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Στοιχεία πλοιοκτήτη / Owner's details");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Στοιχεία τιμολόγησης / Invoice details");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row PersonNames(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονοματεπώνυμο");
            row.Cells[0].AddParagraph("Full name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("ΤΣΙΜΠΡΗΣ ΑΝΤΩΝΙΟΣ");
            row.Cells[2].AddParagraph("Ονοματεπώνυμο");
            row.Cells[2].AddParagraph("Full name");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("ΤΣΙΜΠΡΗΣ ΑΝΤΩΝΙΟΣ A");
            return row;
        }

        private static Row PersonAddresses(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Διεύθυνση κατοικίας");
            row.Cells[0].AddParagraph("Home address");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("ΟΛΥΜΠΙΑΣ 27, ΠΕΤΡΟΥΠΟΛΗ");
            row.Cells[2].AddParagraph("Διεύθυνση κατοικίας");
            row.Cells[2].AddParagraph("Home address");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("ΟΛΥΜΠΙΑΣ 27, ΠΕΤΡΟΥΠΟΛΗ A");
            return row;
        }

        private static Row PersonVATs(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΑΦΜ");
            row.Cells[0].AddParagraph("Tax No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("099863549");
            row.Cells[2].AddParagraph("ΑΦΜ");
            row.Cells[2].AddParagraph("Tax No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("099863549 A");
            return row;
        }

        private static Row PersonTaxOffices(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Greek");
            row.Cells[0].AddParagraph("English");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("Text");
            row.Cells[2].AddParagraph("Greek");
            row.Cells[2].AddParagraph("English");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("Text A");
            return row;
        }

        private static Row PersonIdNumbers(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("ΑΦΜ");
            row.Cells[0].AddParagraph("Tax No");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("099863549");
            row.Cells[2].AddParagraph("ΑΦΜ");
            row.Cells[2].AddParagraph("Tax No");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("099863549 A");
            return row;
        }

        private static Row PersonPhoneNumbers(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[0].AddParagraph("Phone numbers");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("Text");
            row.Cells[2].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[2].AddParagraph("Phone number");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("Text A");
            return row;
        }

        private static Row PersonEmails(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Email");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("email@server.com");
            row.Cells[2].AddParagraph("Email");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("email@server.com");
            return row;
        }

        private static Row CaptainHeaders(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("16cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 7;
            row.BottomPadding = 7;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Στοιχεία πλοιάρχου / Captain's details");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row CaptainDetails(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            table.AddColumn("3cm");
            table.AddColumn("5cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ονοματεπώνυμο");
            row.Cells[0].AddParagraph("Full name");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("Text");
            row.Cells[2].AddParagraph("Αρ. Τηλεφώνου");
            row.Cells[2].AddParagraph("Phone number");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("Text A");
            return row;
        }

        private static Row Fees(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(8);
            table.AddColumn("3cm");
            table.AddColumn("2.6cm");
            table.AddColumn("1cm");
            table.AddColumn("1cm");
            table.AddColumn("2.7cm");
            table.AddColumn("3cm");
            table.AddColumn("2.7cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Συμφωνηθέν τίμημα");
            row.Cells[0].AddParagraph("Agreed fee");
            row.Cells[0].Borders.Right.Clear();
            row.Cells[1].AddParagraph("EUR 99.225,81");
            row.Cells[2].AddParagraph("ΦΠΑ");
            row.Cells[2].AddParagraph("VAT");
            row.Cells[2].Borders.Right.Clear();
            row.Cells[3].AddParagraph("24%");
            row.Cells[3].Borders.Right.Clear();
            row.Cells[4].AddParagraph("EUR 99.999,99");
            row.Cells[5].AddParagraph("Σύνολο");
            row.Cells[5].AddParagraph("Total fee");
            row.Cells[5].Borders.Right.Clear();
            row.Cells[6].AddParagraph("EUR 99.000,00");
            return row;
        }

        private static Row SmallTerms(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ο πλοιοκτήτης δηλώνει ότι έλαβε γνώση όλων των όρων του ισχύοντος Ειδικού Κανονισμού Λειτουργίας της Μαρίνας (απόσπασμα του οποίου προσαρτάται στην παρούσα) και του ισχύοντος τιμοκαταλόγου, τους οποίους αποδέχεται ανεπιφύλακτα και οι οποίοι θα διέπουν την παρούσα.");
            row.Cells[1].AddParagraph("The shipowner declares that has taken full and clear notice of the Marina's Operational Rules, part of which is attached below (terms and conditions) and the current price list, on which the present agreement is based and which he expressly, unreservedly and irrevocably accepts.");
            return row;
        }

        private static Row Signatures(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(218, 238, 243);
            row.TopPadding = 2;
            row.BottomPadding = 2;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("Ο πελάτης / The client");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Για την Μαρίνα / For the Benitses Marina");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            return row;
        }

        private static Row SignatureSpaces(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0.1;
            table.Borders.Color = new Color(153, 162, 165);
            table.Format.Font.Size = Unit.FromPoint(7);
            table.AddColumn("8cm");
            table.AddColumn("8cm");
            Row row = table.AddRow();
            row.Shading.Color = new Color(255, 255, 255);
            row.TopPadding = 50;
            return row;
        }

        private static Row Spacer(Section section) {
            var table = section.AddTable();
            table.Borders.Width = 0;
            table.AddColumn("16cm");
            var row = table.AddRow();
            return row;
        }

        private static void SavePdf(Document document) {
            var pdfRenderer = new PdfDocumentRenderer { Document = document };
            pdfRenderer.RenderDocument();
            pdfRenderer.Save("Invoice.pdf");
        }

    }

}