import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { LeaseRenewalListExportVM } from '../view-models/lease-renewal-list-export-vm'
import { LeaseRenewalListVM } from '../view-models/lease-renewal-list-vm'

@Injectable({ providedIn: 'root' })

export class LeaseRenewalListExportService {

    private exportedRecords: LeaseRenewalListExportVM[]

    constructor(private dateHelperService: DateHelperService) { }

    public buildList(records: LeaseRenewalListVM[]): LeaseRenewalListExportVM[] {
        this.exportedRecords = []
        records.forEach(record => {
            this.exportedRecords.push({
                boat: record.boat.description,
                owner: record.boat.owner,
                loa: record.boat.loa,
                beam: record.boat.beam,
                leaseEnds: this.dateHelperService.formatISODateToLocale(record.leaseEnds)
            })
        })
        return this.exportedRecords
    }

    public exportToExcel(exportedRecords: LeaseRenewalListExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportedRecords)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            worksheet["!cols"] = [{ wch: 50 }, { wch: 50 }, { wch: 15 }, { wch: 15 }, { wch: 30 }]
            xlsx.utils.sheet_add_aoa(worksheet, [["ΣΚΑΦΟΣ", "ΔΙΑΧΕΙΡΙΣΤΗΣ", "LOA", "ΠΛΑΤΟΣ", "ΕΩΣ"]], { origin: "A1" });
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'ΑΝΑΝΕΩΣΕΙΣ ΣΥΜΒΑΣΕΩΝ')
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