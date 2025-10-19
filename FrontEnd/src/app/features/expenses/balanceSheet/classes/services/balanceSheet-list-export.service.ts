import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { BalanceSheetExportVM } from '../view-models/export/balanceSheet-export-vm'
import { BalanceSheetVM } from '../view-models/list/balanceSheet-vm'

@Injectable({ providedIn: 'root' })

export class BalanceSheetExportService {

    private exportedRecords: BalanceSheetExportVM[]

    public buildList(records: BalanceSheetVM[]): BalanceSheetExportVM[] {
        this.exportedRecords = []
        records.forEach(record => {
            this.exportedRecords.push({
                supplier: record.supplier.description,
                bank: record.supplier.bank,
                iban: record.supplier.iban,
                amount: record.actualBalance
            })
        })
        return this.exportedRecords
    }

    public exportToExcel(exportedRecords: BalanceSheetExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportedRecords)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            worksheet["!cols"] = [{ wch: 50 }, { wch: 50 }, { wch: 50 }, { wch: 15 }]
            xlsx.utils.sheet_add_aoa(worksheet, [["Προμηθευτής", "Τράπεζα", "IBAN", "Ποσό"]], { origin: "A1" });
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'ΙΣΟΖΥΓΙΟ ΠΡΟΜΗΘΕΥΤΩΝ')
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