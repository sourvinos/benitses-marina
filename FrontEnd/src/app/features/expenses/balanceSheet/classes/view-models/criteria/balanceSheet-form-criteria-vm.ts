import { SimpleEntity } from "src/app/shared/classes/simple-entity"

export interface BalanceSheetFormCriteriaVM {

    company: SimpleEntity
    balanceFilter: SimpleEntity
    fromDate: string
    toDate: string

}
