import { BalanceSheetSupplierVM } from './balanceSheetSupplier-vm'

export interface BalanceSheetVM {

    supplier: BalanceSheetSupplierVM
    previousBalance: number
    debit: number
    credit: number
    actualBalance: number

}
