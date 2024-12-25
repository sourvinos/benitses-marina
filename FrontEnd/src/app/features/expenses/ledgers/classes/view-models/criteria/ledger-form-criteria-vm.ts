import { SimpleEntity } from "src/app/shared/classes/simple-entity"

export interface LedgerFormCriteriaVM {

    company: SimpleEntity
    supplier: SimpleEntity
    fromDate: string
    toDate: string

}
