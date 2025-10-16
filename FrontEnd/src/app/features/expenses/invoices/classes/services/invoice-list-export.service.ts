import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
import { InvoiceListExportVM } from '../view-models/invoice-list-export-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { InvoiceListVM } from '../view-models/invoice-list-vm'
// Custom

@Injectable({ providedIn: 'root' })

export class InvoiceListExportService {

    private exportedRecords: InvoiceListExportVM[]

    constructor(private dateHelperService: DateHelperService) { }

    public buildList(records: InvoiceListVM[]): InvoiceListExportVM[] {
        this.exportedRecords = []
        records.forEach(record => {
            this.exportedRecords.push({
                date: this.dateHelperService.createDateFromString(record.date),
                company: record.company.description,
                supplier: record.supplier.description,
                documentType: record.documentType.description,
                paymentMethod: record.paymentMethod.description,
                documentNo: record.documentNo,
                amount: record.amount
            })
        })
        return this.exportedRecords
    }

    public exportToExcel(exportedRecords: InvoiceListExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportedRecords)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            xlsx.utils.sheet_add_aoa(worksheet, [["Ημερομηνία", "Εταιρία", "Προμηθευτής", "Παραστατικό", "Τρόπος πληρωμής", "Νο παραστατικού", "Αξία"]], { origin: "A1" });
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'ΗΜΕΡΟΛΟΓΙΟ ΕΞΟΔΩΝ')
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