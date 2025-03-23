import saveAs from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { StatisticsExportVM } from '../view-models/export/statistics-export-vm'
import { StatisticsVM } from '../view-models/list/statistics-vm'

@Injectable({ providedIn: 'root' })

export class StatisticsExportService {

    //#region variables

    private exportVM: StatisticsExportVM[]

    //#endregion

    //#region public methods

    public buildVM(records: StatisticsVM[]): StatisticsExportVM[] {
        this.exportVM = []
        records.forEach(record => {
            this.exportVM.push({
                supplier: record.supplier.description,
                previousBalance: record.previousBalance,
                debit: record.debit,
                credit: record.credit,
                periodBalance: 0,
                actualBalance: record.actualBalance
            })
        })
        return this.exportVM
    }

    public exportToExcel(x: StatisticsExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const _x = xlsx.utils.json_to_sheet(x)
            const workbook = xlsx.utils.book_new()
            xlsx.utils.book_append_sheet(workbook, _x, 'Τζίροι προμηθευτών', true)
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'ΤΖΙΡΟΙ ΠΡΟΜΗΘΕΥΤΩΝ')
        })
    }

    //#endregion

    //#region private methods

    private saveAsExcelFile(buffer: any, fileName: string): void {
        const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
        const EXCEL_EXTENSION = '.xlsx'
        const data: Blob = new Blob([buffer], {
            type: EXCEL_TYPE
        })
        saveAs.saveAs(data, fileName + EXCEL_EXTENSION)
    }

    //#endregion

}