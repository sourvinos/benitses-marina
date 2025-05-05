import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface PriceReadDto extends Metadata {

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
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
