import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { ReservationListVM } from '../view-models/reservation-list-vm'
import { ReservationListExportVM } from '../view-models/reservation-list-export-vm'
// Custom

@Injectable({ providedIn: 'root' })

export class ReservationListExportService {

    private exportedRecords: ReservationListExportVM[]

    constructor(private dateHelperService: DateHelperService) { }

    public buildList(records: ReservationListVM[]): ReservationListExportVM[] {
        this.exportedRecords = []
        records.forEach(record => {
            this.exportedRecords.push({
                boat: record.boatName,
                customer: record.ownerName,
                loa: record.boatLoa,
                from: this.dateHelperService.formatISODateToLocale(record.fromDate),
                to: record.toDate,
                berths: record.joinedBerths,
                payment: record.paymentStatus.description,
                isAthenian: record.isAthenian ? "Y" : "",
                isOverdue: record.isOverdue ? "Y" : "",
                isDocked: record.isDocked ? "Y" : "",
                isDryDock: record.isDryDock ? "Y" : "",
                isFishingBoat: record.isFishingBoat ? "Y" : "",
                isRequest: record.isRequest ? "Y" : ""
            })
        })
        return this.exportedRecords
    }

    public exportToExcel(exportedRecords: ReservationListExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportedRecords)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            worksheet["!cols"] = [{ wch: 50 }, { wch: 50 }, { wch: 50 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 15 }]
            xlsx.utils.sheet_add_aoa(worksheet, [["ΣΚΑΦΟΣ", "ΠΕΛΑΤΗΣ", "LOA", "ΑΠΟ", "ΕΩΣ", "ΘΕΣΗ", "ΠΛΗΡΩΜΗ", "ATHENIAN", "ΕΚΠΡΟΘΕΣΜΟ", "ΕΛΛΙΜΕΝΙΣΜΕΝΟ", "DRY DOCK", "ΑΛΙΕΥΤΙΚΟ", "ΑΙΤΗΣΗ"]], { origin: "A1" });
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'ΗΜΕΡΟΛΟΓΙΟ ΚΡΑΤΗΣΕΩΝ')
        })
    }

    private saveAsExcelFile(buffer: any, fileName: string): void {
        const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
        const EXCEL_EXTENSION = '.xlsx'
        const data: Blob = new Blob([buffer], {
            type: EXCEL_TYPE
        })
        FileSaver.saveAs(data, fileName + EXCEL_EXTENSION)
    }

}