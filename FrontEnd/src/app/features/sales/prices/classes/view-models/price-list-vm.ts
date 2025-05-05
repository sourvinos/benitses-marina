import { SimpleEntity } from "src/app/shared/classes/simple-entity"

export interface PriceListVM {

    id: number
    hullType: SimpleEntity
    periodType: SimpleEntity
    seasonType: SimpleEntity
    code: string
    description: string
    englishDescription: string
    isIndividual: boolean
    length: number
    netAmount: number
    vatAmount: number
    grossAmount: number

}
